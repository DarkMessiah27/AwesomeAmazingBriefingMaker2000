﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheAwesomeAmazingBriefingMaker2000.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TheAwesomeAmazingBriefingMaker2000.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to _adminState = call BIS_fnc_admin;
        ///_uid = getPlayerUID player;
        ///if (
        ///(_adminState != 0) ||
        ///(God isEqualTo player) ||
        ///(_uid == &quot;76561198006804011&quot;) ||
        ///(_uid == &quot;76561197989925440&quot;) ||
        ///(_uid == &quot;76561197970317496&quot;) ||
        ///(_uid == &quot;76561197983143701&quot;) ||
        ///(_uid == &quot;76561197985738940&quot;) ||
        ///(_uid == &quot;76561198096113294&quot;)
        ///) then {
        ///NEWTAB(&quot;Admin Tab&quot;)
        ///&lt;br/&gt;The server admin, the Zeus (if present), and all Council members have access to these options.
        ///&lt;br/&gt;
        ///&lt;br/&gt;&lt;font color=&apos;#70db70&apos; size=&apos;14&apos;&gt;Respawn Wave:&lt;/ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ATRespawnWave {
            get {
                return ResourceManager.GetString("ATRespawnWave", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 2. Panzer-Division.
        /// </summary>
        internal static string ClanName {
            get {
                return ResourceManager.GetString("ClanName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1x Tank crew.
        /// </summary>
        internal static string DefaultEnglishAttachments {
            get {
                return ResourceManager.GetString("DefaultEnglishAttachments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1x Company consisting of:
        ///
        ///    1x Company HQ
        ///    1x Reserve Section
        ///
        ///    2x Platoons, each consiting of:
        ///        1x Platoon HQ
        ///        3x Sections.
        /// </summary>
        internal static string DefaultEnglishFriendlyComposition {
            get {
                return ResourceManager.GetString("DefaultEnglishFriendlyComposition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1x Panzerkampfwagen IV Ausf. H.
        /// </summary>
        internal static string DefaultGermanAssets {
            get {
                return ResourceManager.GetString("DefaultGermanAssets", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1x Panzerbesatzung.
        /// </summary>
        internal static string DefaultGermanAttachments {
            get {
                return ResourceManager.GetString("DefaultGermanAttachments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1x Kompanie consisting of:
        ///
        ///    1x Kompanietrupp
        ///    1x Ersatztruppen
        ///
        ///    2x Züge, each consiting of:
        ///        1x Zugtrupp
        ///        3x Gruppen.
        /// </summary>
        internal static string DefaultGermanFriendlyComposition {
            get {
                return ResourceManager.GetString("DefaultGermanFriendlyComposition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You need to fill in at least one victory and one defeat condition in the Admin Tab!.
        /// </summary>
        internal static string EndingConditionsNotSetError {
            get {
                return ResourceManager.GetString("EndingConditionsNotSetError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to These are used to call the mission endings that the mission maker has set up.
        ///
        ///Please be careful as a single click will end the mission immediately.
        ///.
        /// </summary>
        internal static string EndingMessagesText {
            get {
                return ResourceManager.GetString("EndingMessagesText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HQ - Company HQ
        ///
        ///1-0: 1 Platoon HQ
        ///1-1: 1 Platoon, 1 Section
        ///1-2: 1 Platoon, 2 Section
        ///1-3: 1 Platoon, 3 Section
        ///
        ///2-0: 2 Platoon HQ
        ///2-1: 2 Platoon, 1 Section
        ///2-2: 2 Platoon, 2 Section
        ///2-3: 2 Platoon, 3 Section
        ///
        ///Eva - The tank..
        /// </summary>
        internal static string EnglishCallSigns {
            get {
                return ResourceManager.GetString("EnglishCallSigns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enemy Forces.
        /// </summary>
        internal static string EnglishEnemyForces {
            get {
                return ResourceManager.GetString("EnglishEnemyForces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Friendly Forces.
        /// </summary>
        internal static string EnglishFriendlyForces {
            get {
                return ResourceManager.GetString("EnglishFriendlyForces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Intelligence.
        /// </summary>
        internal static string EnglishIntelligence {
            get {
                return ResourceManager.GetString("EnglishIntelligence", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mission.
        /// </summary>
        internal static string EnglishMission {
            get {
                return ResourceManager.GetString("EnglishMission", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Frequency 51.00 MHz - Company Net
        ///Frequency 31.00 MHz - 1 Platoon Sub-Net
        ///Frequency 54.00 MHz - 2 Platoon Sub-Net.
        /// </summary>
        internal static string EnglishRadioFrequencies {
            get {
                return ResourceManager.GetString("EnglishRadioFrequencies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Signals.
        /// </summary>
        internal static string EnglishSignals {
            get {
                return ResourceManager.GetString("EnglishSignals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Situation.
        /// </summary>
        internal static string EnglishSituation {
            get {
                return ResourceManager.GetString("EnglishSituation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your briefing was successfully generated! Please run your mission in the editor and check if everything is correct..
        /// </summary>
        internal static string GenerationSuccessMessage {
            get {
                return ResourceManager.GetString("GenerationSuccessMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Something went wrong while trying to generate the briefing. An error log was created in the location of the Briefing Maker&apos;s .exe. Please send it to Brauer..
        /// </summary>
        internal static string GenericErrorMessage {
            get {
                return ResourceManager.GetString("GenericErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HQ - Kompanietrupp
        ///
        ///1-0: 1 Zug, Zugtrupp
        ///1-1: 1 Zug, 1 Gruppe
        ///1-2: 1 Zug, 2 Gruppe
        ///1-3: 1 Zug, 3 Gruppe
        ///
        ///2-0: 2 Zug, Zugtrupp
        ///2-1: 2 Zug, 1 Gruppe
        ///2-2: 2 Zug, 2 Gruppe
        ///2-3: 2 Zug, 3 Gruppe
        ///
        ///Eva - The tank..
        /// </summary>
        internal static string GermanCallSigns {
            get {
                return ResourceManager.GetString("GermanCallSigns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Feindliche Kräfte.
        /// </summary>
        internal static string GermanEnemyForces {
            get {
                return ResourceManager.GetString("GermanEnemyForces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Eigene Kräfte.
        /// </summary>
        internal static string GermanFriendlyForces {
            get {
                return ResourceManager.GetString("GermanFriendlyForces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aufklärung.
        /// </summary>
        internal static string GermanIntelligence {
            get {
                return ResourceManager.GetString("GermanIntelligence", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Auftrag.
        /// </summary>
        internal static string GermanMission {
            get {
                return ResourceManager.GetString("GermanMission", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (Examples are in German, as that is the way the prowords are described in our radio manual.)
        ///
        ///Ende - Out, reply not required
        ///
        ///Kommen - Over, reply to me
        ///
        ///Actual - Used for the commander of a unit.
        ///Example: &apos;1 Actual&apos; is the Zugführer of 1. Zug, not his Funker, who would identify with &apos;1-0&apos;..
        /// </summary>
        internal static string GermanProwords {
            get {
                return ResourceManager.GetString("GermanProwords", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Frequency 51.00 MHz - Kompanie Net
        ///Frequency 31.00 MHz - 1 Zug Sub-Net
        ///Frequency 54.00 MHz - 2 Zug Sub-Net.
        /// </summary>
        internal static string GermanRadioFrequencies {
            get {
                return ResourceManager.GetString("GermanRadioFrequencies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gefechtssignale.
        /// </summary>
        internal static string GermanSignals {
            get {
                return ResourceManager.GetString("GermanSignals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lage.
        /// </summary>
        internal static string GermanSituation {
            get {
                return ResourceManager.GetString("GermanSituation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waving like an idiot - I&apos;m friendly, don&apos;t shoot me
        ///Spamming Q and E, aka the &apos;friendly dance&apos; - I&apos;m friendly, don&apos;t shoot me.
        /// </summary>
        internal static string HandSignals {
            get {
                return ResourceManager.GetString("HandSignals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find the briefing.sqf or endConditions.sqf. Please make sure you select the root folder of your mission (the folder that has the mission.sqm in it)..
        /// </summary>
        internal static string IOErrorMessage {
            get {
                return ResourceManager.GetString("IOErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tekigun.
        /// </summary>
        internal static string JapaneseEnemyForces {
            get {
                return ResourceManager.GetString("JapaneseEnemyForces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yuukouteki na riki.
        /// </summary>
        internal static string JapaneseFriendlyForces {
            get {
                return ResourceManager.GetString("JapaneseFriendlyForces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chisei.
        /// </summary>
        internal static string JapaneseIntelligence {
            get {
                return ResourceManager.GetString("JapaneseIntelligence", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mokuteki.
        /// </summary>
        internal static string JapaneseMission {
            get {
                return ResourceManager.GetString("JapaneseMission", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shingou.
        /// </summary>
        internal static string JapaneseSignals {
            get {
                return ResourceManager.GetString("JapaneseSignals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Joukyou.
        /// </summary>
        internal static string JapaneseSituation {
            get {
                return ResourceManager.GetString("JapaneseSituation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You need to set the path to your mission folder first! Please click the &quot;Select mission folder&quot; button and select the root folder of your mission (the folder with the mission.sqm in it)..
        /// </summary>
        internal static string PathNotSetErrorMessage {
            get {
                return ResourceManager.GetString("PathNotSetErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вражеские Силы.
        /// </summary>
        internal static string RussianEnemyForces {
            get {
                return ResourceManager.GetString("RussianEnemyForces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Дружественные Силы.
        /// </summary>
        internal static string RussianFriendlyForces {
            get {
                return ResourceManager.GetString("RussianFriendlyForces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Разведка.
        /// </summary>
        internal static string RussianIntelligence {
            get {
                return ResourceManager.GetString("RussianIntelligence", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Задача.
        /// </summary>
        internal static string RussianMission {
            get {
                return ResourceManager.GetString("RussianMission", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to сигнальная.
        /// </summary>
        internal static string RussianSignals {
            get {
                return ResourceManager.GetString("RussianSignals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ситуация.
        /// </summary>
        internal static string RussianSituation {
            get {
                return ResourceManager.GetString("RussianSituation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Green - Go signal
        ///Red - Stop/halt signal
        ///Purple - Request support
        ///Orange - Need a medic
        ///Blue - Friendly position
        ///Yellow - Regroup on that position
        ///White - Used for concealment only.
        /// </summary>
        internal static string SmokeSignals {
            get {
                return ResourceManager.GetString("SmokeSignals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon trident {
            get {
                object obj = ResourceManager.GetObject("trident", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
    }
}
