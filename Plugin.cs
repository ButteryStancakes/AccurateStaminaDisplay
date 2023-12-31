using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace AccurateStaminaDisplay
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "butterystancakes.lethalcompany.accuratestaminadisplay", PLUGIN_NAME = "Accurate Stamina Display", PLUGIN_VERSION = "1.1.0";
        public static ConfigEntry<bool> configExhaustedRed, configInhalantInfo;

        void Awake()
        {
            configExhaustedRed = Config.Bind("Extra", "ExhaustedRed", true, "Turns the stamina meter red when you are exhausted (unable to sprint).");
            configInhalantInfo = Config.Bind("Extra", "InhalantInfo", false, "Adjusts the color of the stamina meter to reflect the amount of TZP inhaled.");

            new Harmony(PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }
    }
}