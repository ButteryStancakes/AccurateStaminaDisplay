# v1.1.2
- Added "EmptyEarly" setting to restore the vanilla game's behavior of displaying an empty stamina meter for the last 20%
  - This makes it easier to tell when releasing the sprint key would lead to early exhaustion
  - However, this makes it more difficult to tell exactly how much longer exhaustion will last
# v1.1.1
- Adjusted the green used by the inhalant palette
# v1.1.0
- "InhalantInfo" configuration setting added
  - Gradually shifts the color of the meter as you inhale TZP for endurance
  - Light usage = yellow. Heavy usage = green. Overdose = white.
- Refactored how the player script is referenced
# v1.0.3
- Final fix for the flickering bug
# v1.0.2
- Meter no longer turns red when critically injured (seems this also caused the flickering bug)
# v1.0.1
- "ExhaustedRed" configuration setting is now enabled by default
- Fixed a bug that caused meter color to erroneously flicker
# v1.0.0
- Initial release