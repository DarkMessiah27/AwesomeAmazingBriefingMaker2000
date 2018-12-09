using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TheAwesomeAmazingBriefingMaker2000
{
    class FileWriter
    {
        private string briefingFilePath;
        private string endConditionsFilePath;

        public bool HasPaths { get; set; }
        
        public FileWriter()
        {
            HasPaths = false;
        }

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

        private void EditEndConditions(Dictionary<int, List<string>> messages)
        {
            List<string> lines = File.ReadAllLines(endConditionsFilePath).ToList();

            int startOfEndMessages = 0;
            int removalCounter = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("=== Admin/Zeus End Messages ==="))
                    startOfEndMessages = i + 1;

                if (startOfEndMessages > 0)
                    removalCounter++;
            }

            lines.RemoveRange(startOfEndMessages, removalCounter - 1);

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

            lines.Add("sleep (10);");
            
            File.WriteAllLines(endConditionsFilePath, lines);
        }

        public void CreateErrorLog(Exception ex)
        {
            string path = Directory.GetCurrentDirectory() + "\\errorLog.txt";

            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine(ex.Message);
                tw.WriteLine();
                tw.WriteLine(ex.StackTrace);
            }
        }
    }
}
