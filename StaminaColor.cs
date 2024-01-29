using GameNetcodeStuff;
using UnityEngine;
using UnityEngine.UI;

namespace AccurateStaminaDisplay
{
    internal class StaminaColor : MonoBehaviour
    {
        public const float METER_EMPTY = 0.298f;
        public const float METER_FULL = 0.91f;

        public static float minStamina = 0.1f;

        PlayerControllerB player;
        bool exhausted;
        Color normColor = new Color(1f, 0.4626f, 0f), exColor = new Color(0.9f, 0.1f, 0f);
        Gradient tzpGrad = new Gradient();
        Image meterOverlay;
        float overlayAmount;

        void Awake()
        {
            Color lime = new Color(0.4f, 1f, 0f);
            tzpGrad.SetKeys(new GradientColorKey[]
            {
                new GradientColorKey(normColor, 0f),
                new GradientColorKey(Color.yellow, 0.1f),
                new GradientColorKey(Color.yellow, 0.2f),
                new GradientColorKey(lime, 0.65f),
                new GradientColorKey(lime, 0.77f),
                new GradientColorKey(Color.white, 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            });
        }

        void LateUpdate()
        {
            if (player == null)
            {
                player = GameNetworkManager.Instance.localPlayerController;
                return;
            }

            if (Plugin.configAlwaysShowRedPortion.Value)
            {
                if (meterOverlay == null && Plugin.configExhaustedRed.Value && !Plugin.configEmptyEarly.Value)
                {
                    Transform transMeterOverlay = Instantiate(player.sprintMeterUI.transform, player.sprintMeterUI.transform.parent);
                    Destroy(transMeterOverlay.GetComponent<StaminaColor>());
                    meterOverlay = transMeterOverlay.GetComponent<Image>();
                    meterOverlay.color = exColor;
                    overlayAmount = Mathf.Lerp(METER_EMPTY, METER_FULL, 0.2f / 0.9f);
                }

                if (meterOverlay != null)
                    meterOverlay.fillAmount = Mathf.Min(overlayAmount, player.sprintMeterUI.fillAmount);
            }

            if (exhausted)
            {
                if (player.isSprinting || player.sprintMeter >= 0.3f)
                    exhausted = false;
            }
            else if (Plugin.configExhaustedRed.Value && !Plugin.configEmptyEarly.Value && !Plugin.configAlwaysShowRedPortion.Value && ((!player.isSprinting && player.sprintMeter < 0.3f) || player.isExhausted))
            {
                exhausted = true;
                player.sprintMeterUI.color = exColor;
            }
            else
            {
                if (Plugin.configInhalantInfo.Value && player.drunkness > 0f)
                    player.sprintMeterUI.color = tzpGrad.Evaluate(player.drunkness);
                else
                    player.sprintMeterUI.color = normColor;
            }
        }
    }
}
