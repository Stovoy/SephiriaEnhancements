# Default behavior audit

The native settings entry, multiplayer joining/reconnection support and crash recovery load by default.
With reconnect support enabled, reloading a saved exploration automatically creates a
friends-only Steam room for two players. This covers both native saves and optional disk
checkpoints, using the resumed world and its current chapter rather than stale lobby metadata.
Other optional feature controllers and Harmony hooks require explicit opt-in.
Existing per-feature preferences do not count as consent: new `SephiriaEnhancements.OptIn.*`
keys default to false except crash recovery. Explicitly saved choices are respected. New feature IDs are also off unless explicitly opted in.

In Gameplay options, select General and enable the desired subsystem, then restart Sephiria.
Disabling an opt-in also takes effect after restarting. Individual subsystem settings still apply.
The suite switch remains a global runtime stop switch, including multiplayer support.

| Subsystem | Default |
| --- | --- |
| Joining, reconnecting and joining-player supplies | On |
| Multiplayer custom rules, rule announcements and world rule tablet | Off |
| Persistent area checkpoints and crash recovery | On |
| Item chat, middle-click sharing hints and prompts | Off |
| Backpack arrangement, reward highlights and inventory HUD | Off |
| Combat recording, DPS, battle reports, training statistics and hit feedback | Off |
| Targeting, lock-on and aim assistance | Off |
| Automatic casting | Off |
| Map enhancements and navigation | Off |
| Keyboard navigation and all associated page patches | Off |
| Combat visuals, transparency and relation outlines | Off |
| Companion, camera distance and resource bar numbers | Off |
| Defeat retry and cutscene skipping | Off |
| Journal/help additions and effect statistics | Off |
| Costume appearance, tier reward claiming and fixed exploration seeds | Off |
| Welcome messages, update checks and developer tools | Off |
| Support logs and Mod messages in Unity's player log | Off |
| Developer diagnostics | Off |

No mod shortcut map or fallback bindings are prepared when every optional input consumer is off.
Shared gameplay state supports crash recovery and other selected subsystems.
Secondary settings also start disabled/native: damage display, hit feedback, map, outlines,
targeting, combat visual presets/transparency, bar numbers, welcome/update checks and cutscene skips.

Support logging requires the explicit `-sephiria-enhancements-supportlog` launch argument.
Developer logging additionally requires a development build, the developer-tools opt-in and
`-sephiria-enhancements-devlog`. Logs owned by the game itself are outside the mod's control.
