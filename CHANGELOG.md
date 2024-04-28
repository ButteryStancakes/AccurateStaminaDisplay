# v2.0.3
- Movement is no longer considered hindered when climbing a ladder
# v2.0.2
- Fixed an oversight that caused AlwaysShow + InhalantInfo to create some ugly color combinations
# v2.0.1
- Fixed red bar still being visible while critically injured when using AlwaysShow
- When using ChangeColor or AlwaysShow, the bar will appear red now when you are "hindered" (stuck in spider webs or deep water)
# v2.0.0
- Complete code rewrite
- Simplified the config
- Should feel exactly the same in-game, but hopefully a lot cleaner and neater under the hood
- ShyHUD compatibility
# v1.2.0
- Added "AlwaysShowRedPortion" setting to display the last 20% of the stamina bar as red always
  - Similar to "EmptyEarly", this makes it easy to tell when releasing the sprint key would cause exhaustion
  - This still allows you to measure how long exhaustion will last, as well as how long you can keep sprinting before automatic exhaustion
  - The best of all worlds!
  - "ExhaustedRed" must be enabled and "EmptyEarly" must be disabled or this setting will not take effect
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