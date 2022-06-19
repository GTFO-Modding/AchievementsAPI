# Achievement API
Achievements API is a plugin for GTFO that focuses on implementing an achievement system into the game.

A fixed achievement system would be stupid, and such this project has been designed as an API to allow for both
plugin developers to expand on the current built in features, but for Rundown makers to create achievements
for their rundown.

The system is broken down into 3 systems:
- **Achievements** - Contain achievement information--such as name, description, and icon--, conditions, and triggers.
- **Triggers** - Basic "Goals" that can be used with conditions for more customization. They include a `Count` so you could specify a trigger of tagging 5 enemies instead of 1 enemy. They also contain `ConditionOverrides` which is planned for allowing additional customization of the conditions, but currently only supports adding conditionals to the specific trigger.
- **Conditions** - The limiting factor to a "Trigger" that can make it more specialized/challenging, such as forcing the triggers to have the player crouched.

## Rundowns
Information to use this plugin in custom rundowns.

## Plugins API
Plugins can extend the behaviour of the achievement system by adding new achievements, conditions, or
triggers.

#### Special Thanks
- Project Zaero for suggesting this idea.