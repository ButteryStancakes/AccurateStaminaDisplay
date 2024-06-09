using UnityEngine;
using UnityEngine.UI;
using GameNetcodeStuff;
using BepInEx.Bootstrap;
using System.Reflection;

namespace AccurateStaminaDisplay
{
    internal static class NewStaminaMeter
    {
        // vanilla considers 0.1 stamina to be empty, not 0
        const float STAMINA_EMPTY = 0.1f;
        // 0.3 stamina is the "exhaustion threshold" - if you release sprint while below 0.3 you become exhausted, once you regen 0.3 or more, exhaustion ends
        const float STAMINA_EXHAUSTED = 0.3f;
        // 0.29767 and 0.911 roughly correlate to the start and end points on the meter graphic
        const float METER_EMPTY = 0.29767f, METER_FULL = 0.911f;

        // default stamina meter color (orange)
        static readonly Color NORM_COLOR = new(1f, 0.4626f, 0f);
        // exhausted stamina meter color (red)
        static readonly Color EX_COLOR = new(0.9f, 0.1f, 0f);

        // references/controls
        static PlayerControllerB player;
        static float minStamina;
        static bool exhausted;

        // secondary meter, overlaid for AlwaysShow
        static Image meterOverlay;
        static readonly float OVERLAY_MAX = Mathf.Lerp(METER_EMPTY, METER_FULL, Mathf.InverseLerp(STAMINA_EMPTY, 1f, STAMINA_EXHAUSTED));

        // gradient, used to sample color for InhalantInfo
        static Gradient tzpGrad;
        // lime shade used by the above
        static readonly Color LIME = new(0.4f, 1f, 0f);
        // ranges for "drunkness" - or how much TZP inhaled. mostly anecdotal magic numbers
        const float TZP_LIGHT_MIN = 0.1f, TZP_LIGHT_MAX = 0.2f, TZP_HEAVY_MIN = 0.65f, TZP_HEAVY_MAX = 0.77f;

        // ShyHUD compatibility
        static CanvasRenderer meterAlpha, overlayAlpha;

        // used for hindrance
        static readonly FieldInfo playerMovementHinderedPrev = typeof(PlayerControllerB).GetField("movementHinderedPrev", BindingFlags.NonPublic | BindingFlags.Instance);
		
		// prevents flickering when dismounnting ladders after hindrance
		static bool wasClimbing;

        internal static void UpdateMeter()
        {
            // not initialized yet, skip processing for this frame and initialize
            if (player == null)
            {
                Init();
                return;
            }

            // controls what is considered "empty" on the meter; either the "exhaustion threshold", or when sprinting automatically ends
            minStamina = Plugin.configExhaustionIndicator.Value == ExhaustionIndicator.Empty ? STAMINA_EXHAUSTED : STAMINA_EMPTY;

            // first calculate what percentage of "true stamina" the player actually has left (dependent on above)
            float trueStamina = Mathf.InverseLerp(minStamina, 1f, player.sprintMeter);
            // then calculate what portion of the bar to display based on that percentage
            player.sprintMeterUI.fillAmount = Mathf.Lerp(METER_EMPTY, METER_FULL, trueStamina);
			
			// fix hindrance flickering with ladders
			if (player.isClimbingLadder)
				wasClimbing = true;
            else if (player.thisController.isGrounded)
                wasClimbing = false;

            // simulate PlayerControllerB.movementHinderedPrev of local player
            bool hindered = (int)playerMovementHinderedPrev.GetValue(player) > 0 && !wasClimbing && !player.jetpackControls;

            // is the bar using a non-standard color because of TZP?
            bool recoloredTZP = Plugin.configInhalantInfo.Value && player.drunkness > 0f && tzpGrad != null;

            // "AlwaysShow" is treated as "ChangeColor" when player has endurance, because otherwise it's pretty ugly...
            // now also considers "hindrance" same as exhaustion
            bool changeColor = Plugin.configExhaustionIndicator.Value == ExhaustionIndicator.ChangeColor || (Plugin.configExhaustionIndicator.Value == ExhaustionIndicator.AlwaysShow && (hindered || recoloredTZP));

            if (exhausted)
            {
                // check if bar needs to change back from red
                if (!changeColor || player.isSprinting || (player.sprintMeter >= STAMINA_EXHAUSTED && !hindered))
                    exhausted = false;
            }
            else if (changeColor && (player.isExhausted || hindered))
            {
                // can't sprint anymore; turn bar red
                exhausted = true;
                player.sprintMeterUI.color = EX_COLOR;
            }
            else
            {
                // not exhausted/hindered, might need to sample TZP color
                if (recoloredTZP)
                    player.sprintMeterUI.color = tzpGrad.Evaluate(player.drunkness);
                // otherwise default color
                else
                    player.sprintMeterUI.color = NORM_COLOR;
            }

            // process the "AlwaysShow" overlay
            if (meterOverlay != null)
            {
                // in case LethalConfig user changes settings mid-game
                // some special cases treat "AlwaysShow" as "ChangeColor" (TZP endurance or hindrance)
                if (Plugin.configExhaustionIndicator.Value == ExhaustionIndicator.AlwaysShow && !exhausted && !player.criticallyInjured && !recoloredTZP)
                {
                    meterOverlay.fillAmount = Mathf.Min(player.sprintMeterUI.fillAmount, OVERLAY_MAX);
                    // ShyHUD compatibility
                    if (overlayAlpha != null)
                        overlayAlpha.SetAlpha(meterAlpha.GetAlpha());
                    meterOverlay.enabled = player.sprintMeterUI.enabled;
                    meterOverlay.gameObject.SetActive(true);
                }
                else
                    meterOverlay.gameObject.SetActive(false);
            }
        }

        static void Init()
        {
            // pull player reference again
            player = GameNetworkManager.Instance.localPlayerController;

            // initialize the red portion of the bar for AlwaysShow
            if (meterOverlay == null && player != null)
            {
                Transform transMeterOverlay = Object.Instantiate(player.sprintMeterUI.transform, player.sprintMeterUI.transform.parent);
                meterOverlay = transMeterOverlay.GetComponent<Image>();
                meterOverlay.color = EX_COLOR;
            }

            // initialize the gradient used for TZP sampling
            if (tzpGrad == null)
            {
                tzpGrad = new Gradient();
                tzpGrad.SetKeys(new GradientColorKey[]
                {
                    new GradientColorKey(NORM_COLOR, 0f),
                    new GradientColorKey(Color.yellow, TZP_LIGHT_MIN),
                    new GradientColorKey(Color.yellow, TZP_LIGHT_MAX),
                    new GradientColorKey(LIME, TZP_HEAVY_MIN),
                    new GradientColorKey(LIME, TZP_HEAVY_MAX),
                    new GradientColorKey(Color.white, 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                });
            }

            // ShyHUD compatibility
            if (Chainloader.PluginInfos.ContainsKey("ShyHUD") && player != null && meterOverlay != null)
            {
                meterAlpha = player.sprintMeterUI.GetComponent<CanvasRenderer>();
                overlayAlpha = meterOverlay.GetComponent<CanvasRenderer>();
            }
        }
    }
}
