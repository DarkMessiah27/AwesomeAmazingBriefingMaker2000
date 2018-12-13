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
        Briefing briefing;
        FileWriter fw;

        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
            LanguageProperty.OverrideMetadata(typeof(FrameworkElement), 
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name)));

            briefing = new Briefing(Country.Germany);
            fw = new FileWriter();

            InitializeComponent();
        }

        private bool HasNotSetMissionPath()
        {
            if (fw.HasPaths)
                return false;
            else
                return true;
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (HasNotSetMissionPath())
            {
                MessageBox.Show(Properties.Resources.PathNotSetErrorMessage, "Error!");
                return;
            }

            if (rbGermany.IsChecked == true)
                briefing.Country = Country.Germany;
            else if (rbOther.IsChecked == true)
                briefing.Country = Country.Other;

            #region Text extraction
            // Mission Notes
            briefing.Tabs.Find(t => t.Name.Contains("Mission Notes"))
                .Sections.Find(s => s.Name == "Victory Conditions:").Text = GetTextFromTextBox(tbVictoryConditions);
            briefing.Tabs.Find(t => t.Name.Contains("Mission Notes"))
                .Sections.Find(s => s.Name == "Defeat Conditions:").Text = GetTextFromTextBox(tbDefeatConditions);
            
            // Admin Tab
            Section endingMessagesSection = new Section(name: "Mission Ending:", fontColour: "#70db70", size: 14, text: new List<string> {
                        "These are used to call the mission endings that the mission maker has set up.",
                        "",
                        "Please be careful as a single click will end the mission immediately.",
                        ""
                    });
            Dictionary<int, List<string>> endingMessages = new Dictionary<int, List<string>>();

            int messageCounter = 1;
            foreach (TextBox tb in grAdminTab.Children.OfType<TextBox>())
            {
                if (string.IsNullOrWhiteSpace(tb.Text) == false)
                {
                    if (tb.Name.Contains("Button"))
                    {
                        string endingType = tb.Name.Contains("Victory") ? "Victory" : "Defeat";
                        string endingNumber = tb.Name.Substring(tb.Name.Length - 1);

                        string buttonText = tb.Text;
                        List<string> message = GetTextFromTextBox(grAdminTab.Children.OfType<TextBox>()
                            .Single(ch => ch.Name == "tb" + endingType + "Message" + endingNumber));
                        message.Add(endingType);

                        endingMessagesSection.Text.Add(string.Format(
                            "<execute expression=\'{0}Message{1} call FNC_EndMissionRequest\'>{2}</execute>", 
                            endingType, messageCounter, buttonText));
                        endingMessages.Add(messageCounter, message);
                        messageCounter++;
                    }
                }
            }

            if (DoesNotHaveEndingConditions(endingMessages))
            {
                MessageBox.Show(Properties.Resources.EndingConditionsNotSetError, "Error!");
                return;
            }

            try
            {
                briefing.Tabs.Find(t => t.Name.Contains("Admin Tab")).Sections[1] = endingMessagesSection;
            }
            catch (ArgumentOutOfRangeException)
            {
                briefing.Tabs.Find(t => t.Name.Contains("Admin Tab")).Sections.Add(endingMessagesSection);
            }
            briefing.EndingMessages = endingMessages;

            // Zeus Notes
            briefing.Tabs.Find(t => t.Name.Contains("Zeus Notes")).Sections.First().Text = GetTextFromTextBox(tbZeusNotes);

            // Situation
            briefing.Tabs.Find(t => t.Name.Contains("Situation")).Sections.First().Text = GetTextFromTextBox(tbSituation);

            // Mission
            briefing.Tabs.Find(t => t.Name.Contains("(Mission)")).Sections.First()
                .Text = GetTextFromTextBox(tbMissionStatement);
            briefing.Tabs.Find(t => t.Name.Contains("(Mission)")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == 'A').Text = GetTextFromTextBox(tbConceptOfOperation);
            briefing.Tabs.Find(t => t.Name.Contains("(Mission)")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '1').Text = GetTextFromTextBox(tbTimings);
            briefing.Tabs.Find(t => t.Name.Contains("(Mission)")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '2').Text = GetTextFromTextBox(tbControlMeasures);
            briefing.Tabs.Find(t => t.Name.Contains("(Mission)")).Sections.Where(s => s.Name != null).ToList()
                .Find(s => s.Name.First() == '3').Text = GetTextFromTextBox(tbRulesOfEngagement);

            // Intelligence
            briefing.Tabs.Find(t => t.Name.Contains("Intelligence")).Sections.Find(s => s.Name.First() == '1')
                .Text = GetTextFromTextBox(tbTerrain);
            briefing.Tabs.Find(t => t.Name.Contains("Intelligence")).Sections.Find(s => s.Name.First() == '2')
                .Text = GetTextFromTextBox(tbWeather);
            briefing.Tabs.Find(t => t.Name.Contains("Intelligence")).Sections.Find(s => s.Name.First() == '3')
                .Text = GetTextFromTextBox(tbCivilians);
            briefing.Tabs.Find(t => t.Name.Contains("Intelligence")).Sections.Find(s => s.Name.First() == '4')
                .Text = GetTextFromTextBox(tbPertinentInformation);

            // Enemy Forces
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

            // Friendly Forces
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

            // Signals
            List<string> codeWords = GetTextFromTextBox(tbCodewords);
            List<string> passwords = GetTextFromTextBox(tbPasswords);

            bool hasCodeWords = false;
            foreach (string cd in codeWords)
            {
                if (string.IsNullOrWhiteSpace(cd) == false)
                    hasCodeWords = true;
            }

            bool hasPasswords = false;
            foreach (string pw in passwords)
            {
                if (string.IsNullOrWhiteSpace(pw) == false)
                    hasPasswords = true;
            }

            if (hasCodeWords)
            {
                Section codeWordsSection = briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "3. Codewords:");
                if (codeWordsSection == null)
                {
                    //briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Add(
                    //    new Section(name: "3. Codewords:", fontColour: "#70db70", size: 14, text: codeWords));

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
                Section codeWordsSection = briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "3. Codewords:");

                if (codeWordsSection != null)
                    briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Remove(codeWordsSection);
            }

            if (hasPasswords)
            {
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
                Section passwordsSection = briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Find(s => s.Name == "4. Passwords:");

                if (passwordsSection != null)
                    briefing.Tabs.Find(t => t.Name.Contains("Signals")).Sections.Remove(passwordsSection);
            }

            // Assign localised text to Signal tab where necessary
            switch (briefing.Country)
            {
                case Country.Germany:
                    briefing.Tabs.Find(t => t.Name.Contains("Signals"))
                        .Sections.Find(s => s.Name == "A. Call Signs:").Text = Properties.Resources.GermanCallSigns.Split('\n').ToList();
                    briefing.Tabs.Find(t => t.Name.Contains("Signals"))
                        .Sections.Find(s => s.Name == "C. Radio Frequencies:").Text = Properties.Resources.GermanRadioFrequencies.Split('\n').ToList();
                    break;
                case Country.Japan:
                case Country.Russia:
                default:
                    briefing.Tabs.Find(t => t.Name.Contains("Signals"))
                        .Sections.Find(s => s.Name == "A. Call Signs:").Text = Properties.Resources.EnglishCallSigns.Split('\n').ToList();
                    briefing.Tabs.Find(t => t.Name.Contains("Signals"))
                        .Sections.Find(s => s.Name == "C. Radio Frequencies:").Text = Properties.Resources.EnglishRadioFrequencies.Split('\n').ToList();
                    break;
            }

            #endregion

            try
            {
                fw.GenerateBriefing(briefing);
                MessageBox.Show(Properties.Resources.GenerationSuccessMessage, "Success!");
            }
            catch (IOException)
            {
                MessageBox.Show(Properties.Resources.IOErrorMessage, "Error!");
            }
            catch (Exception ex)
            {
                fw.CreateErrorLog(ex);
                MessageBox.Show(Properties.Resources.GenericErrorMessage, "Error!");
            }
        }

        /// <summary>
        /// Extracts the text from a rich text box element.
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        private List<string> GetTextFromTextBox(TextBox tb)
        {
            List<string> lines = tb.Text.Split('\n').ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].Replace("\r", "");
            }

            return lines;
        }

        private void RbGermany_Checked(object sender, RoutedEventArgs e)
        {
            briefing.Country = Country.Germany;

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

        private bool DoesNotHaveEndingConditions(Dictionary<int, List<string>> endingMessages)
        {
            if (endingMessages.Count < 2)
                return true;

            int nrOfVictoryConditions = endingMessages.Values.Where(message => message.Last() == "Victory").Count();
            int nrOfDefeatConditions = endingMessages.Values.Where(message => message.Last() == "Defeat").Count();

            if (nrOfVictoryConditions == 0 || nrOfDefeatConditions == 0)
                return true;

            return false;
        }
    }
}
