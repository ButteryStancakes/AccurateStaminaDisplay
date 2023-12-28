using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace AccurateStaminaDisplay
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "butterystancakes.lethalcompany.accuratestaminadisplay", PLUGIN_NAME = "Accurate Stamina Display", PLUGIN_VERSION = "1.0.0W";
        public static ConfigEntry<bool> configExhaustedRed;

        void Awake()
        {
            configExhaustedRed = Config.Bind("Extra", "ExhaustedRed", false, "Turns the stamina meter red when you are exhausted (unable to sprint/jump).");

            Harmony harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }
    }
}