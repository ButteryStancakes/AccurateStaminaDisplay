using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace AccurateStaminaDisplay.Patches
{
    [HarmonyPatch]
    class AccurateStaminaDisplayPatches
    {
        [HarmonyPatch(typeof(PlayerControllerB), "Awake")]
        [HarmonyPostfix]
        public static void Awake(PlayerControllerB __instance)
        {
            StaminaColor.minStamina = Plugin.configEmptyEarly.Value ? 0.3f : 0.1f;
            if (((Plugin.configExhaustedRed.Value && !Plugin.configEmptyEarly.Value) || Plugin.configInhalantInfo.Value) && !__instance.sprintMeterUI.GetComponent<StaminaColor>())
                __instance.sprintMeterUI.gameObject.AddComponent<StaminaColor>();
        }

        [HarmonyPatch(typeof(PlayerControllerB), "LateUpdate")]
        [HarmonyPostfix]
        public static void LateUpdate(PlayerControllerB __instance)
        {
            if (__instance.sprintMeter > StaminaColor.minStamina && __instance.sprintMeter < 1f)
                __instance.sprintMeterUI.fillAmount = Mathf.Lerp(StaminaColor.METER_EMPTY, StaminaColor.METER_FULL, (__instance.sprintMeter - StaminaColor.minStamina) / (1f - StaminaColor.minStamina));
        }
    }
}