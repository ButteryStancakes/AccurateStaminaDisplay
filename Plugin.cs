using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace AccurateStaminaDisplay
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("ShyHUD", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "butterystancakes.lethalcompany.accuratestaminadisplay", PLUGIN_NAME = "Accurate Stamina Display", PLUGIN_VERSION = "2.0.1";
        public static ConfigEntry<bool> configInhalantInfo;
        public static ConfigEntry<string> configExhaustionIndicator;

        void Awake()
        {
            LoadConfig();

            new Harmony(PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }

        void LoadConfig()
        {
            configInhalantInfo = Config.Bind(
                "Extra",
                "InhalantInfo",
                false,
                "Adjusts the color of the stamina meter to reflect the amount of TZP inhaled. (Light = yellow, heavy = green, \"overdose\" = white)");

            configExhaustionIndicator = Config.Bind(
                "Misc",
                "ExhaustionIndicator",
                "AlwaysShow",
                new ConfigDescription(
                "How the stamina meter displays exhaustion. You become exhausted when stamina hits 0% or if you release the sprint key while stamina is 20% or lower.\n" +
                "\"Empty\" will make the stamina bar display as empty for the last 20% of stamina, just like the original game. " +
                "\"ChangeColor\" will turn the bar red when you are currently exhausted. " +
                "\"AlwaysShow\" will always display the last 20% of the bar as red. " +
                "\"DontShow\" will not display any special indicator for exhaustion.", 
                new AcceptableValueList<string>("AlwaysShow", "ChangeColor", "DontShow", "Empty")));

            // if player is using the default for the above setting, there's the possibility old configs might need migration
            if (configExhaustionIndicator.Value == "AlwaysShow")
            {
                // load the old settings
                bool alwaysShowRedPortion = Config.Bind("Extra", "AlwaysShowRedPortion", true, "Legacy setting, use \"ExhaustionIndicator\" instead").Value;
                bool exhaustedRed = Config.Bind("Extra", "ExhaustedRed", true, "Legacy setting, use \"ExhaustionIndicator\" instead").Value;
                bool emptyEarly = Config.Bind("Miscellaneous", "EmptyEarly", false, "Legacy setting, use \"ExhaustionIndicator\" instead").Value;

                // handle migration if necessary
                if (emptyEarly)
                    configExhaustionIndicator.Value = "Empty";
                else if (exhaustedRed)
                {
                    if (alwaysShowRedPortion)
                        configExhaustionIndicator.Value = "AlwaysShow";
                    else
                        configExhaustionIndicator.Value = "ChangeColor";
                }
                else
                    configExhaustionIndicator.Value = "DontShow";

                // remove all old entries
                Config.Remove(Config["Extra", "AlwaysShowRedPortion"].Definition);
                Config.Remove(Config["Extra", "ExhaustedRed"].Definition);
                Config.Remove(Config["Miscellaneous", "EmptyEarly"].Definition);
                
                // save new ExhaustionIndicator value and remove orphaned entries
                Config.Save();
            }
        }
    }
}