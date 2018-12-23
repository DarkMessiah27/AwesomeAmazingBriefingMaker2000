using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace TheAwesomeAmazingBriefingMaker2000
{
    class FileWriter
    {
        private string briefingFilePath;
        private string endConditionsFilePath;
        private readonly string previousBriefingsPath = Directory.GetCurrentDirectory() + "\\Saved Briefings\\";

        public bool HasPaths { get; set; }
        
        public FileWriter()
        {
            HasPaths = false;
        }

        /// <summary>
        /// Prompts the user to select the mission's folder, from which the paths to the
        /// briefing.sqf and endConditions.sqf are derived.
        /// </summary>
        public void SetPaths()
        {
            var dlg = new VistaFolderBrowserDialog
            {
                Description = "Select your mission folder"
            };

            if (dlg.ShowDialog() == true)
            {
                if (string.IsNullOrWhiteSpace(dlg.SelectedPath) == false)
                {
                    briefingFilePath = dlg.SelectedPath + "\\customization\\briefing.sqf";
                    endConditionsFilePath = dlg.SelectedPath + "\\customization\\endConditions.sqf";

                    HasPaths = true;
                }
            }
        }

        //public void SaveMainWindowContents(List<TextBox> textBoxes, List<RadioButton> radioButtons)
        //{
        //    string fileName = previousBriefingsPath + Utilities.CreateDatetimeSeed() + ".txt";
        //    Directory.CreateDirectory(previousBriefingsPath);
        //    var file = File.Create(fileName);
        //    file.Close();

        //    using (TextWriter tw = new StreamWriter(fileName))
        //    {
        //        foreach (var rb in radioButtons)
        //            tw.WriteLine(string.Format("{0}|{1}", rb.Name, rb.IsChecked));
        //        foreach (var tb in textBoxes)
        //            tw.WriteLine(string.Format("{0}|{1}", tb.Name, tb.Text));
        //    }
        //}

        /// <summary>
        /// Generates a briefing.sqf file from scratch, and edits an existing endConditions.sqf file.
        /// </summary>
        /// <param name="briefing"></param>
        public void GenerateBriefing(Briefing briefing)
        {
            if (File.Exists(briefingFilePath) == false)
            {
                var file = File.Create(briefingFilePath);
                file.Close();
            }

            using (TextWriter tw = new StreamWriter(briefingFilePath))
            {
                tw.WriteLine("#include \"core\\briefingCore.sqf\"");
                
                foreach (Tab tab in briefing.Tabs)
                {
                    WriteTab(tab, tw);
                }

                tw.WriteLine("DISPLAYBRIEFING();");
                tw.WriteLine("#include \"orbat.sqf\"");
            }

            EditEndConditions(briefing.EndingMessages);
        }

        /// <summary>
        /// Writes the contents of one Tab object into the briefing.sqf file.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="tw"></param>
        private void WriteTab(Tab tab, TextWriter tw)
        {
            if (tab.Name == "Admin Tab")
            {
                tw.WriteLine(Properties.Resources.ATRespawnWave);

                foreach (var section in tab.Sections)
                {
                    WriteSection(section, tw);
                }

                tw.WriteLine("ENDTAB;");
                tw.WriteLine("};");
                return;
            }
            else if (tab.Name == "Zeus Notes")
            {
                tw.WriteLine("if (!isNil \"God\" && {God isEqualTo player}) then {");
                tw.WriteLine(string.Format("NEWTAB(\"{0}\")", tab.Name));

                foreach (var section in tab.Sections)
                {
                    WriteSection(section, tw);
                }

                tw.WriteLine("ENDTAB;");
                tw.WriteLine("};");
                return;
            }

            tw.WriteLine(string.Format("NEWTAB(\"{0}\")", tab.Name));

            foreach (var section in tab.Sections)
            {
                WriteSection(section, tw);
            }

            tw.WriteLine("ENDTAB;");
        }

        /// <summary>
        /// Writes the content of one Section object into the briefing.sqf file.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="tw"></param>
        private void WriteSection(Section section, TextWriter tw)
        {
            if (string.IsNullOrWhiteSpace(section.Name) == false)
            {
                tw.WriteLine(string.Format("<br/><font color=\'{0}\' size=\'{1}\'>{2}</font>",
                    section.FontColour, section.Size, section.Name));
            }

            if (section.Text != null)
            {
                foreach (string line in section.Text)
                    tw.WriteLine("<br/>" + line);
            }

            tw.WriteLine("<br/>");
        }

        /// <summary>
        /// Edits the endConditions.sqf file.
        /// </summary>
        /// <param name="messages"></param>
        private void EditEndConditions(Dictionary<int, List<string>> messages)
        {
            // Read all the lines of an existing endConditions.sqf file.
            List<string> lines = File.ReadAllLines(endConditionsFilePath).ToList();

            // We need to find the line indicating the start of the end messages section of the file.
            // Once we've found it, we need to start counting all the lines that come after. 
            // These lines are (as of the time of writing) either empty or commented out code that is no longer used.
            int startOfEndMessages = 0;
            int removalCounter = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("=== Admin/Zeus End Messages ==="))
                    startOfEndMessages = i + 1;

                if (startOfEndMessages > 0)
                    removalCounter++;
            }

            // Remove the above-mentioned unneccesary lines.
            lines.RemoveRange(startOfEndMessages, removalCounter - 1);

            // Add each ending message.
            foreach (var message in messages)
            {
                string victoryType = message.Value.Last();

                lines.Add(string.Format("{0}Message{1} = \"", victoryType, message.Key));
                foreach (string messageLine in message.Value)
                {
                    if (messageLine == message.Value.Last())
                        continue;

                    lines.Add("<br/>" + messageLine);
                }
                lines.Add("<br/>");
                lines.Add("<br/>" + victoryType + "\";");
                lines.Add(string.Format("publicVariable \"{0}Message{1}\";", victoryType, message.Key));
            }

            // Add the sleep command back onto the end of the list (since it was removed in a previous step).
            lines.Add("sleep (10);");
            
            // Write the list to the file, overwriting its contents.
            File.WriteAllLines(endConditionsFilePath, lines);
        }

        /// <summary>
        /// In case of an unexpected error, create an error log with the exception and its stack trace.
        /// </summary>
        /// <param name="ex"></param>
        public void CreateErrorLog(Exception ex)
        {
            string path = Directory.GetCurrentDirectory() + "\\errorLog" + Utilities.CreateDatetimeSeed() + ".txt";

            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine(ex.Message);
                tw.WriteLine();
                tw.WriteLine(ex.StackTrace);
            }
        }
    }
}
