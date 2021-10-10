using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace TheAwesomeAmazingBriefingMaker2000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Briefing briefing;
        private FileWriter fw;
        
        public MainWindow()
        {
            // Set the applicaton's culture info settings to British English, used for the spell check.
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
            LanguageProperty.OverrideMetadata(typeof(FrameworkElement), 
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name)));

            // New briefing object is created with the country set to Germany by default. This can be changed by the user in the UI later.
            briefing = new Briefing(Country.Germany);
            fw = new FileWriter();

            InitializeComponent();

            this.Title = $"The Awesome Amazing Briefing Maker 2000 ({Properties.Resources.VersionNumber})";
        }
        
        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Check if the user has set the path to the mission folder before attempting to generate the briefing.
            if (!fw.HasPaths)
            {
                MessageBox.Show(Properties.Resources.PathNotSetErrorMessage, "Error!");
                return;
            }

            // Change the briefing's country if necessary.
            if (rbGermany.IsChecked == true)
                briefing.Country = Country.Germany;
            else if (rbOther.IsChecked == true)
                briefing.Country = Country.Other;

            #region Text extraction

            // ------------------------------------------------------------------------------------------------------------
            //
            // The app will go over each tab in the UI, finding and editing the appropriate Tab and Section objects of the 
            // briefing object. Most of these Tabs and Sections were setup in advance when the briefing object was created.
            //
            // ------------------------------------------------------------------------------------------------------------

            #region Mission Notes
            briefing.Tabs.Find(t => t.Name.Contains("Mission Notes"))
                .Sections.Find(s => s.Name == "Victory Conditions:").Text = GetTextFromTextBox(tbVictoryConditions);
            briefing.Tabs.Find(t => t.Name.Contains("Mission Notes"))
                .Sections.Find(s => s.Name == "Defeat Conditions:").Text = GetTextFromTextBox(tbDefeatConditions);
            #endregion

            #region Admin Tab
            // Set up the default text that shows up above the ending messages first.
            Section endingMessagesSection = new Section(name: "Mission Ending:", fontColour: "#70db70", size: 14, text:
                Properties.Resources.AdminTabEndingMessagesTextTop.Split('\n').ToList());

            Dictionary<int, List<string>> endingMessages = new Dictionary<int, List<string>>();
            int messageCounter = 1;
            bool firstDefeatMessageReached = false;
            // Cycle through each of the text boxes on the admin tab.
            foreach (TextBox tb in grAdminTab.Children.OfType<TextBox>())
            {
                // Ignore empty text boxes.
                if (string.IsNullOrWhiteSpace(tb.Text) == false)
                {
                    // Use the text boxes labeled as button as the starting point.
                    if (tb.Name.Contains("Button"))
                    {
                        // Determine the victory type of the text box.
                        string endingType = tb.Name.Contains("Victory") ? "Victory" : "Defeat";
                        // Get the number of the text box.
                        string endingNumber = tb.Name.Substring(tb.Name.Length - 1);

                        // Extract the text that will become the button label in the in-game briefing.
                        string buttonText = tb.Text;

                        // Extract the text that will become the message shown after the in-game ending is called.
                        List<string> message = GetTextFromTextBox(grAdminTab.Children.OfType<TextBox>()
                            .Single(ch => ch.Name == "tb" + endingType + "Message" + endingNumber));
                        message.Add(endingType);

                        if (endingType == "Defeat" && !firstDefeatMessageReached)
                        {
                            firstDefeatMessageReached = true;
                            endingMessagesSection.Text.Add("");
                        }

                        // Store the text of the button and the end screen in the dictionary, using the correct sqf format.
                        endingMessagesSection.Text.Add(string.Format(
                            "<execute expression=\'{0}Message{1} call Olsen_FW_FNC_EndMissionRequest\'>{2}</execute>", 
                            endingType, messageCounter, buttonText));
                        endingMessages.Add(messageCounter, message);

                        // Increment the message counter used to give each ending condition a unique number.
                        // (Will be used later to make sure the variable names of the ending conditions match in
                        // both the endingConditions.sqf and the briefing.sqf.
                        messageCounter++;
                    }
                }
            }

            // Add the message that shows up below the ending messages.
            endingMessagesSection.Text.AddRange(Properties.Resources.AdminTabEndingMessagesTextBottom.Split('\n').ToList());

            // Check if the user filled in a valid number of ending conditions.
            // If not, show an error message and end generation.
            if (DoesNotHaveEndingConditions(endingMessages))
            {
                MessageBox.Show(Properties.Resources.EndingConditionsNotSetError, "Error!");
                return;
            }

            try
            {
                // Try to override the second section of the admin tab with the ending messages.
                // This will be the case if the user is generating more than once.
                briefing.Tabs.Find(t => t.Name.Contains("Admin Tab")).Sections[1] = endingMessagesSection;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If it is the first time the user has hit the Generate button, the second section will not yet exist so it must be created.
                briefing.Tabs.Find(t => t.Name.Contains("Admin Tab")).Sections.Add(endingMessagesSection);
            }

            // Store the ending messages in the briefing.
            // This is used later on for the endingConditions.sqf
            briefing.EndingMessages = endingMessages;
            #endregion

            #region Zeus Notes and Situation
            // Zeus Notes
            briefing.Tabs.Find(t => t.Name.Contains("Zeus Notes")).Sections.First().Text = GetTextFromTextBox(tbZeusNotes);

            // Situation
            briefing.Tabs.Find(t => t.Name.Contains("Situation")).Sections.First().Text = GetTextFromTextBox(tbSituation);
            #endregion
            
            #region Mission
            briefing.Tabs.Find(t => t.Name.Contains("Mission") && !t.Name.Contains("Notes")).Sections.First()
                .Text = GetTextFromTextBox(tbMissionStatement);
            briefing.Tabs.Find(t => t.Name.Contains("Mission") && !t.Name.Contains("Notes")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == 'A').Text = GetTextFromTextBox(tbConceptOfOperation);
            briefing.Tabs.Find(t => t.Name.Contains("Mission") && !t.Name.Contains("Notes")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '1').Text = GetTextFromTextBox(tbTimings);
            briefing.Tabs.Find(t => t.Name.Contains("Mission") && !t.Name.Contains("Notes")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '2').Text = GetTextFromTextBox(tbControlMeasures);
            briefing.Tabs.Find(t => t.Name.Contains("Mission") && !t.Name.Contains("Notes")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '3').Text = GetTextFromTextBox(tbRulesOfEngagement);
            #endregion

            #region Intelligence
            briefing.Tabs.Find(t => t.Name.Contains("Intelligence")).Sections.Find(s => s.Name.First() == '1')
                .Text = GetTextFromTextBox(tbTerrain);
            briefing.Tabs.Find(t => t.Name.Contains("Intelligence")).Sections.Find(s => s.Name.First() == '2')
                .Text = GetTextFromTextBox(tbWeather);
            briefing.Tabs.Find(t => t.Name.Contains("Intelligence")).Sections.Find(s => s.Name.First() == '3')
                .Text = GetTextFromTextBox(tbCivilians);
            briefing.Tabs.Find(t => t.Name.Contains("Intelligence")).Sections.Find(s => s.Name.First() == '4')
                .Text = GetTextFromTextBox(tbPertinentInformation);
            #endregion

            #region Enemy Forces
            briefing.Tabs.Find(t => t.Name.Contains("Enemy Forces")).Sections.First()
                .Text = GetTextFromTextBox(tbNameEnemyForces);
            briefing.Tabs.Find(t => t.Name.Contains("Enemy Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '1').Text = GetTextFromTextBox(tbEnemyComposition);
            briefing.Tabs.Find(t => t.Name.Contains("Enemy Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '2').Text = GetTextFromTextBox(tbLocation);
            briefing.Tabs.Find(t => t.Name.Contains("Enemy Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '3').Text = GetTextFromTextBox(tbPossibleEnemyActions);
            briefing.Tabs.Find(t => t.Name.Contains("Enemy Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '4').Text = GetTextFromTextBox(tbDefensiveFires);
            briefing.Tabs.Find(t => t.Name.Contains("Enemy Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '5').Text = GetTextFromTextBox(tbEnemyAirPresence);
            briefing.Tabs.Find(t => t.Name.Contains("Enemy Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '6').Text = GetTextFromTextBox(tbFutureIntentions);
            #endregion

            #region Friendly Forces
            briefing.Tabs.Find(t => t.Name.Contains("Friendly Forces")).Sections.First()
                .Text = GetTextFromTextBox(tbNameFriendlyForces);
            briefing.Tabs.Find(t => t.Name.Contains("Friendly Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '1').Text = GetTextFromTextBox(tbFriendlyComposition);
            briefing.Tabs.Find(t => t.Name.Contains("Friendly Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '2').Text = GetTextFromTextBox(tbAttachments);
            briefing.Tabs.Find(t => t.Name.Contains("Friendly Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '3').Text = GetTextFromTextBox(tbAssets);
            briefing.Tabs.Find(t => t.Name.Contains("Friendly Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '4').Text = GetTextFromTextBox(tbSupportingFires);
            briefing.Tabs.Find(t => t.Name.Contains("Friendly Forces")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '5').Text = GetTextFromTextBox(tbFriendlyAirPresence);
            #endregion

            #region Signals
            List<string> codeWords = GetTextFromTextBox(tbCodewords);
            List<string> passwords = GetTextFromTextBox(tbPasswords);

            // The Codewords section is optional and will only be created if the user filled anything in.
            if (!codeWords.All(s => s == "Unknown"))
            {
                // Check if a codewords section already exists.
                // (This would be the case if the user clicks generate more than once)
                // If it does, override it; if not, create it.
                Section codeWordsSection = briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "3. Codewords:");
                if (codeWordsSection == null)
                {
                    briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Insert(6,
                        new Section(name: "3. Codewords:", fontColour: "#70db70", size: 14, text: codeWords));
                }
                else
                {
                    briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "3. Codewords:").Text = codeWords;
                }
            }
            else
            {
                // If a code words section exists, it means the user already generated the briefing at least once.
                // However, if the text box was now left empty, it needs to be removed.
                Section codeWordsSection = briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "3. Codewords:");

                if (codeWordsSection != null)
                    briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Remove(codeWordsSection);
            }

            // The Passwords section is optional and will only be created if the user filled anything in.
            if (!passwords.All(s => s == "Unknown"))
            {
                // Check if a passwords section already exists.
                // (This would be the case if the user clicks generate more than once)
                // If it does, override it; if not, create it.
                Section passwordsSection = briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "4. Passwords:");
                if (passwordsSection == null)
                {
                    briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Add(
                        new Section(name: "4. Passwords:", fontColour: "#70db70", size: 14, text: passwords));
                }
                else
                {
                    briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "4. Passwords:").Text = passwords;
                }
            }
            else
            {
                // If a password section exists, it means the user already generated the briefing at least once.
                // However, if the text box was now left empty, it needs to be removed.
                Section passwordsSection = briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "4. Passwords:");

                if (passwordsSection != null)
                    briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Remove(passwordsSection);
            }
            #endregion

            // Assign localised text to Signal tab where necessary.
            switch (briefing.Country)
            {
                case Country.Germany:
                    briefing.Tabs.Find(t => t.Name.Contains("Signals"))
                        .Sections.Find(s => s.Name == "A. Call Signs:").Text = Properties.Resources.GermanCallSigns.Split('\n').ToList();
                    briefing.Tabs.Find(t => t.Name.Contains("Signals"))
                        .Sections.Find(s => s.Name == "C. Radio Frequencies:").Text = Properties.Resources.GermanRadioFrequencies.Split('\n').ToList();
                    break;
                case Country.Japan:
                    // Not yet supported, will drop through to default.
                case Country.Russia:
                    // Not yet supported, will drop through to default.
                default:
                    // Default language is English.
                    briefing.Tabs.Find(t => t.Name.Contains("Signals"))
                        .Sections.Find(s => s.Name == "A. Call Signs:").Text = Properties.Resources.EnglishCallSigns.Split('\n').ToList();
                    briefing.Tabs.Find(t => t.Name.Contains("Signals"))
                        .Sections.Find(s => s.Name == "C. Radio Frequencies:").Text = Properties.Resources.EnglishRadioFrequencies.Split('\n').ToList();
                    break;
            }
            #endregion // End of text extraction region.

            try
            {
                fw.GenerateBriefing(briefing);
                MessageBox.Show(Properties.Resources.GenerationSuccessMessage, "Success!");
            }
            catch (IOException)
            {
                // An IO exception might occur if the application wasn't able to open, find, or edit the files.
                MessageBox.Show(Properties.Resources.IOErrorMessage, "Error!");
            }
            catch (Exception ex)
            {
                // If a different exception occurs that was not forseen, create an error log and show an error message.
                fw.CreateErrorLog(ex);
                MessageBox.Show(Properties.Resources.GenericErrorMessage, "Error!");
            }

            //List<TextBox> textBoxes = new List<TextBox>();
            //List<RadioButton> radioButtons = new List<RadioButton>();

            //GetChildControl(this, ref textBoxes, ref radioButtons);
            
            //fw.SaveMainWindowContents(textBoxes, radioButtons);
        }


        //private void GetChildControl(Visual visual, ref List<TextBox> textBoxes, ref List<RadioButton> radioButtons)
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
        //    {
        //        var lc = LogicalChildren;
                
        //        Visual childVisual = (Visual)VisualTreeHelper.GetChild(visual, i);

        //        if (childVisual is TextBox)
        //            textBoxes.Add((TextBox)childVisual);
        //        else if (childVisual is RadioButton)
        //            radioButtons.Add((RadioButton)childVisual);

        //        GetChildControl(childVisual, ref textBoxes, ref radioButtons);
        //    }
        //}

        /// <summary>
        /// Extracts the text from a text box element.
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        private List<string> GetTextFromTextBox(TextBox tb)
        {
            List<string> lines = tb.Text.Split('\n').ToList();

            if (lines.All(s => s == ""))
            {
                lines = new List<string> { "Unknown" };
                return lines;
            }
            else
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i] = lines[i].Replace("\r", "");
                }

                return lines;
            }
        }

        private void RbGermany_Checked(object sender, RoutedEventArgs e)
        {
            briefing.Country = Country.Germany;
            briefing.SetLocalisedHeaderNames();

            foreach (var checkbox in grFriendlyForces.Children.OfType<CheckBox>())
            {
                if (checkbox.IsChecked == true)
                {
                    checkbox.IsChecked = false;
                    checkbox.IsChecked = true;
                }
                else
                {
                    checkbox.IsChecked = true;
                    checkbox.IsChecked = false;
                }
            }
        }

        private void RbOther_Checked(object sender, RoutedEventArgs e)
        {
            briefing.Country = Country.Other;
            briefing.SetLocalisedHeaderNames();
            
            foreach (var checkbox in grFriendlyForces.Children.OfType<CheckBox>())
            {
                if (checkbox.IsChecked == true)
                {
                    checkbox.IsChecked = false;
                    checkbox.IsChecked = true;
                }
                else
                {
                    checkbox.IsChecked = true;
                    checkbox.IsChecked = false;
                }
            }
        }

        private void CbUseDefaultFriendlyCompositionChanged(object sender, RoutedEventArgs e)
        {
            tbFriendlyComposition.Text = string.Empty;
            if (((CheckBox)sender).IsChecked == true)
            {
                switch (briefing.Country)
                {
                    case Country.Germany:
                        tbFriendlyComposition.Text = Properties.Resources.DefaultGermanFriendlyComposition;
                        break;
                    default:
                        tbFriendlyComposition.Text = Properties.Resources.DefaultEnglishFriendlyComposition;
                        break;
                }
            }
        }

        private void CbUseDefaultFriendlyForcesNameChanged(object sender, RoutedEventArgs e)
        {
            tbNameFriendlyForces.Text = string.Empty;
            if (((CheckBox)sender).IsChecked == true)
            {
                switch (briefing.Country)
                {
                    case Country.Germany:
                        tbNameFriendlyForces.Text = Properties.Resources.ClanName;
                        break;
                    default:
                        break;
                }
            }
        }

        private void CbUseDefaultAttachmentsChanged(object sender, RoutedEventArgs e)
        {
            tbAttachments.Text = string.Empty;
            if (((CheckBox)sender).IsChecked == true)
            {
                switch (briefing.Country)
                {
                    case Country.Germany:
                        tbAttachments.Text = Properties.Resources.DefaultGermanAttachments;
                        break;
                    default:
                        tbAttachments.Text = Properties.Resources.DefaultEnglishAttachments;
                        break;
                }
            }
        }

        private void CbUseDefaultAssetsChanged(object sender, RoutedEventArgs e)
        {
            tbAssets.Text = string.Empty;
            if (((CheckBox)sender).IsChecked == true)
            {
                switch (briefing.Country)
                {
                    case Country.Germany:
                        tbAssets.Text = Properties.Resources.DefaultGermanAssets;
                        break;
                    default:
                        break;
                }
            }
        }

        private void BtnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fw.SetPaths();
            }
            catch (Exception ex)
            {
                fw.CreateErrorLog(ex);
                MessageBox.Show(Properties.Resources.GenericErrorMessage, "Error!");
            }
        }

        /// <summary>
        /// Checks if the user has filled in at least one victory and at least one defeat condition.
        /// </summary>
        /// <param name="endingMessages">All the ending conditions extracted from the text boxes on the admin tab.</param>
        /// <returns></returns>
        private bool DoesNotHaveEndingConditions(Dictionary<int, List<string>> endingMessages)
        {
            // Having less than two ending conditions means either the victory or the defeat condition is missing.
            if (endingMessages.Count < 2)
                return true;

            // Count the number of victory and defeat conditions.
            int nrOfVictoryConditions = endingMessages.Values.Where(message => message.Last() == "Victory").Count();
            int nrOfDefeatConditions = endingMessages.Values.Where(message => message.Last() == "Defeat").Count();

            // Return true if either of them is 0.
            if (nrOfVictoryConditions == 0 || nrOfDefeatConditions == 0)
                return true;
            
            return false;
        }
    }
}
