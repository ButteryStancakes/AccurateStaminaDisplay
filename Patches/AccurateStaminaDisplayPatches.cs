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
            if (Plugin.configExhaustedRed.Value)
                __instance.gameObject.AddComponent<StaminaColor>().player = __instance;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "LateUpdate")]
        [HarmonyPostfix]
        public static void LateUpdate(PlayerControllerB __instance)
        {
            if (__instance.sprintMeter > 0.1f && __instance.sprintMeter < 1f)
                __instance.sprintMeterUI.fillAmount = Mathf.Lerp(0.298f, 0.91f, (__instance.sprintMeter - 0.1f) / 0.9f);
        }
    }
}