using GameNetcodeStuff;
using HarmonyLib;

namespace AccurateStaminaDisplay.Patches
{
    [HarmonyPatch]
    class AccurateStaminaDisplayPatches
    {
        [HarmonyPatch(typeof(PlayerControllerB), "LateUpdate")]
        [HarmonyPostfix]
        public static void PlayerPostLateUpdate(int ___movementHinderedPrev)
        {
            NewStaminaMeter.UpdateMeter(___movementHinderedPrev);
        }
    }
}