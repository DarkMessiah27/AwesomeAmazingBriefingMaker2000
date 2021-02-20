using System.Collections.Generic;

namespace TheAwesomeAmazingBriefingMaker2000
{
    class Section
    {
        public string Name { get; set; }

        public string FontColour { get; set; }

        public int Size { get; set; }

        public List<string> Text { get; set; }

        public Section()
        {
            Text = new List<string>();
        }

        public Section(List<string> text)
        {
            Text = text;
        }
        
        public Section(string name, string fontColour, int size, List<string> text = null)
        {
            Name = name;
            FontColour = fontColour;
            Size = size;

            if (text != null)
                Text = text;
            else
                Text = new List<string>();
        }
    }
}
