using System.Collections.Generic;
using System.Linq;

namespace TheAwesomeAmazingBriefingMaker2000
{
    class Briefing
    {
        public List<Tab> Tabs { get; set; }
        public Dictionary<int, List<string>> EndingMessages { get; set; }
        public Country Country { get; set; }
        
        public Briefing(Country country)
        {
            Country = country;
            SetupTabs();
        }

        private void SetupTabs()
        {
            Tabs = new List<Tab>
            {
                new Tab("Mission Notes", new List<Section>
                {
                    new Section(text: new List<string> { "Time limit is 120 minutes." }),
                    new Section(name: "Victory Conditions:", fontColour: "#FF8C00", size: 14),
                    new Section(name: "Defeat Conditions:", fontColour: "#FF8C00", size: 14)
                }, false),
                new Tab("Game Mastering", new List<Section>
                {
                    new Section(text: new List<string> { "This mission requires a dedicated Zeus to play correctly." })
                }, false),
                new Tab("Admin Tab", new List<Section>
                {
                    new Section(text: new List<string>())
                }, false),
                new Tab("Zeus Notes", new List<Section>{
                    new Section(name: "If you are not going to Zeus this mission, do not read this tab.", fontColour: "#FF8C00", size: 20)
                }, false),
                new Tab(string.Format("I. {0} (Situation):", GetHeaderResource("Situation")), new List<Section>{
                    new Section()
                }),
                new Tab(string.Format("II. {0} (Mission):", GetHeaderResource("Mission")), new List<Section>{
                    new Section(),
                    new Section(name: "A. Concept of the Operation", fontColour: "#FF8C00", size: 16),
                    new Section(name: "B. Coordinating Instructions", fontColour: "#FF8C00", size: 16),
                    new Section(name: "1. Timings:", fontColour: "#70db70", size: 14),
                    new Section(name: "2. Control Measures:", fontColour: "#70db70", size: 14),
                    new Section(name: "3. Rules of Engagement:", fontColour: "#70db70", size: 14)
                }),
                new Tab(string.Format("III. {0} (Intelligence):", GetHeaderResource("Intelligence")), new List<Section>{
                    new Section(name: "A. Overview", fontColour: "#FF8C00", size: 16),
                    new Section(name: "1. Terrain:", fontColour: "#70db70", size: 14),
                    new Section(name: "2. Weather:", fontColour: "#70db70", size: 14),
                    new Section(name: "3. Civilian Presence:", fontColour: "#70db70", size: 14),
                    new Section(name: "4. Pertinent Information:", fontColour: "#70db70", size: 14)
                }),
                new Tab(string.Format("III. B. {0} (Enemy Forces):", GetHeaderResource("Enemy Forces")), new List<Section>{
                    new Section(),
                    new Section(name: "1. Composition:", fontColour: "#70db70", size: 14),
                    new Section(name: "2. Location:", fontColour: "#70db70", size: 14),
                    new Section(name: "3. Possible Enemy Actions:", fontColour: "#70db70", size: 14),
                    new Section(name: "4. Defensive Fires:", fontColour: "#70db70", size: 14),
                    new Section(name: "5. Enemy Air Presence:", fontColour: "#70db70", size: 14),
                    new Section(name: "6. Future intentions:", fontColour: "#70db70", size: 14)
                }),
                new Tab(string.Format("III. B. {0} (Friendly Forces):", GetHeaderResource("Friendly Forces")), new List<Section>{
                    new Section(),
                    new Section(name: "1. Composition:", fontColour: "#70db70", size: 14),
                    new Section(name: "2. Attachments/Detachments:", fontColour: "#70db70", size: 14),
                    new Section(name: "3. Assets:", fontColour: "#70db70", size: 14),
                    new Section(name: "4. Supporting fires:", fontColour: "#70db70", size: 14),
                    new Section(name: "5. Friendly Air Presence:", fontColour: "#70db70", size: 14)
                }),
                new Tab(string.Format("IV. {0} (Signals):", GetHeaderResource("Signals")), new List<Section> {
                    new Section(name: "A. Call Signs:", fontColour: "#FF8C00", size: 16, text: Properties.Resources.GermanCallSigns.Split('\n').ToList()),
                    new Section(name: "B. Prowords:", fontColour: "#FF8C00", size: 16, text: Properties.Resources.Prowords.Split('\n').ToList()),
                    new Section(name: "C. Radio Frequencies:", fontColour: "#FF8C00", size: 16, text: Properties.Resources.RadioFrequencies.Split('\n').ToList()),
                    new Section(name: "D. Special Signals", fontColour: "#FF8C00", size: 16),
                    new Section(name: "1. Hand Signals:", fontColour: "#70db70", size: 14, text: Properties.Resources.HandSignals.Split('\n').ToList()),
                    new Section(name: "2. Smoke Signals:", fontColour: "#70db70", size: 14, text: Properties.Resources.SmokeSignals.Split('\n').ToList())
                })
            };
        }

        private string GetHeaderResource(string headerName)
        {
            switch (Country)
            {
                case Country.Germany:
                    switch (headerName)
                    {
                        case "Situation":
                            return Properties.Resources.GermanSituation;
                        case "Mission":
                            return Properties.Resources.GermanMission;
                        case "Intelligence":
                            return Properties.Resources.GermanIntelligence;
                        case "Enemy Forces":
                            return Properties.Resources.GermanEnemyForces;
                        case "Friendly Forces":
                            return Properties.Resources.GermanFriendlyForces;
                        case "Signals":
                            return Properties.Resources.GermanSignals;
                        default:
                            return "";
                    }
                case Country.Russia:
                    switch (headerName)
                    {
                        case "Situation":
                            return Properties.Resources.RussianSituation;
                        case "Mission":
                            return Properties.Resources.RussianMission;
                        case "Intelligence":
                            return Properties.Resources.RussianIntelligence;
                        case "Enemy Forces":
                            return Properties.Resources.RussianEnemyForces;
                        case "Friendly Forces":
                            return Properties.Resources.RussianFriendlyForces;
                        case "Signals":
                            return Properties.Resources.RussianSignals;
                        default:
                            return "";
                    }
                case Country.Japan:
                default:
                    switch (headerName)
                    {
                        case "Situation":
                            return Properties.Resources.EnglishSituation;
                        case "Mission":
                            return Properties.Resources.EnglishMission;
                        case "Intelligence":
                            return Properties.Resources.EnglishIntelligence;
                        case "Enemy Forces":
                            return Properties.Resources.EnglishEnemyForces;
                        case "Friendly Forces":
                            return Properties.Resources.EnglishFriendlyForces;
                        case "Signals":
                            return Properties.Resources.EnglishSignals;
                        default:
                            return "";
                    }
            }
        }
    }

    enum Country
    {
        Other,
        Germany,
        Russia,
        Japan
    }
}
