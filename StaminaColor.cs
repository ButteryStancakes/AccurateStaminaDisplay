using GameNetcodeStuff;
using UnityEngine;

namespace AccurateStaminaDisplay
{
    internal class StaminaColor : MonoBehaviour
    {
        public PlayerControllerB player;
        bool exhausted;
        Color normColor = new Color(1f, 0.4626f, 0f), exColor = new Color(0.9f, 0.1f, 0f);

        void LateUpdate()
        {
            if (exhausted)
            {
                if ((player.sprintMeter >= 0.3f && !player.isExhausted) || player.isSprinting)
                {
                    exhausted = false;
                    player.sprintMeterUI.color = normColor;
                }
            }
            else if (!player.isSprinting && (player.isExhausted || player.sprintMeter < 0.3f))
            {
                exhausted = true;
                player.sprintMeterUI.color = exColor;
            }
        }
    }
}
