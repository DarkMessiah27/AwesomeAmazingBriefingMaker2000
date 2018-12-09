using System;
using System.Collections.Generic;
using System.Linq;

namespace TheAwesomeAmazingBriefingMaker2000
{
    class Tab
    {
        public string Name { get; set; }
        public List<Section> Sections { get; set; }
        public bool IsSideSpecific { get; set; }
        
        public Tab(string name, List<Section> sections, bool isSideSpecific = true)
        {
            Name = name;
            IsSideSpecific = isSideSpecific;
            Sections = sections;
        }
    }
}
