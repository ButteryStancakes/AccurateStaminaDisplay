using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace AccurateStaminaDisplay
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "butterystancakes.lethalcompany.accuratestaminadisplay", PLUGIN_NAME = "Accurate Stamina Display", PLUGIN_VERSION = "1.1.2";
        public static ConfigEntry<bool> configExhaustedRed, configInhalantInfo, configEmptyEarly;

        void Awake()
        {
            configExhaustedRed = Config.Bind("Extra", "ExhaustedRed", true, "Turns the stamina meter red when you are exhausted (unable to sprint).");
            configInhalantInfo = Config.Bind("Extra", "InhalantInfo", false, "Adjusts the color of the stamina meter to reflect the amount of TZP inhaled.");
            configEmptyEarly = Config.Bind("Miscellaneous", "EmptyEarly", false, "This partly re-enables the vanilla game's behavior, where the last 20% of the stamina bar is displayed as empty. This may make it easier to tell when releasing the sprint button will lead to early exhaustion, but will make it more difficult to tell how much longer exhaustion will last.\nThis setting is still compatible with InhalantInfo, but ExhaustedRed will not apply if this setting is enabled.");

            new Harmony(PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }
    }
}