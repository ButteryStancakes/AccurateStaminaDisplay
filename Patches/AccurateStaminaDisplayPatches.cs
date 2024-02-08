using GameNetcodeStuff;
using HarmonyLib;

namespace AccurateStaminaDisplay.Patches
{
    [HarmonyPatch]
    class AccurateStaminaDisplayPatches
    {
        [HarmonyPatch(typeof(PlayerControllerB), "LateUpdate")]
        [HarmonyPostfix]
        public static void PlayerPostLateUpdate()
        {
            NewStaminaMeter.UpdateMeter();
        }
    }
}