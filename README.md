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

Achievement information is stored in `/RundownFolder/Custom/AchievementAPI/`. Currently two files are used by AchievementAPI in this directory:
- `achievements.json`
- `achievement-progress.json`

#### achievements.json
A custom way of defining achievements.

Currently defined them as:
```json
{
    "Achievements": [
        {
            "Name": string,
            "Description": string,
            "Icon": string|null,
            "IsSecret": boolean,
            "SoundEffect": {
                "Enabled": boolean,
                "CustomSoundOverride": uint
            },
            "AchievementPoints": int,

            // Triggers can be defined in two ways:
            // 1.)
            "Triggers": [
                {
                    "ID": TriggerID,
                    "Count": int,
                    "ConditionOverrides": {
                        "HasOverrides": boolean,
                        // Additional Conditions can be defined in two ways.
                        // 1.)
                        "AdditionalConditions": [
                            {
                                "ID": ConditionID,
                                // Specific Per Condition
                                "Data": ConditionData
                            }
                        ],
                        // 2.)
                        "AdditionalConditions": {
                            "ConditionID": {
                                // Specific Per Condition
                                "Data": ConditionData
                            }
                        },
                    },
                    // Specific Per Trigger
                    "Data": TriggerData
                }
            ],
            // 2.)
            "Triggers": {
                TriggerID: {
                    "Count": int,
                    "ConditionOverrides": {
                        "HasOverrides": boolean,
                        // Additional Conditions can be defined in two ways.
                        // 1.)
                        "AdditionalConditions": [
                            {
                                "ID": ConditionID,
                                // Specific Per Condition
                                "Data": ConditionData
                            }
                        ],
                        // 2.)
                        "AdditionalConditions": {
                            ConditionID: {
                                // Specific Per Condition
                                "Data": ConditionData
                            }
                        },
                    },
                    // Specific Per Trigger
                    "Data": TriggerData
                }
            },
            // Can be defined in two ways:
            // 1.)
            "Conditions": [
                {
                    "ID": ConditionID,
                    // Specific Per Condition
                    "Data": ConditionData
                }
            ],
            // 2.)
            "Conditions": {
                ConditionID: {
                    // Specific Per Condition
                    "Data": ConditionData
                }
            },
            "ID": AchievementID
        }
    ]
}
```
This format may change, but this is the current format as of now.

For reference, here's an example json file for an achievement for tagging 5 scouts only on A1.
```json
{
    "Achievements": [
        {
            "Name": "Scout!!!",
            "Description": "Tag 5 Scouts using the Bio Tracker",
            "Icon": null,
            "IsSecret": false,
            "SoundEffect": {
                "Enabled": false,
                "CustomSoundOverride": 0
            },
            "AchievementPoints": 10,
            "Triggers": [
                {
                    "ID": "EnemyBiotracked",
                    "Count": 5,
                    "ConditionOverrides": {
                        "HasOverrides": false,
                        "AdditionalConditions": []
                    },
                    "Data": {
                        "Players": {
                            "LocalPlayer": {
                                "Include": true
                            },
                            "Players": {
                                "Include": false
                            },
                            "Bots": {
                                "Include": false
                            }
                        },
                        "Enemies": {
                            "UseWhiteList": true,
                            "WhiteList": [
                                20, // Scout
                                41, // Scout Bullrush
                                40 // Scout Shadow
                            ],
                            "BlackList": []
                        },
                        // force unique tags
                        "UniqueOnly": true
                    }
                }
            ],
            "Conditions": [
                {
                    "ID": "LevelRestrictions",
                    "Data": {
                        "UseWhiteList": true,
                        "WhiteListedLevels": [
                            {
                                "ExpeditionIndex": 0,
                                "Tier": 1
                            }
                        ],
                        "BlackListedLevels": []
                    }
                }
            ],
            "ID": "Basic-Enemy-Tag"
        }
    ]
}
```

Please note: For conditions defined in `AdditionalConditions`, conditions defined in `Conditions`, and triggers defined in `Triggers`, please ensure the `ID` property comes first if you are using the list format. An error will occur if it's not first, due to the plugin not knowing how to handle the data associated with the Condition or Trigger.

#### achievement-progress.json
Stores all progress relates to achievements.

Currently looks like:
```json
{
  "Achievements": [
    {
      "AchievementID": AchievementID,
      "Progress": {
        "Triggers": [
          {
            "ID": TriggerID,
            // increment is used when defining duplicate triggers inside an achievement.
            // this is how it knows which trigger is which
            "Increment": Increment,
            "Progress": {
              // The number of times this trigger has been triggered.
              "TriggerCount": int
            }
          }
        ]
      }
    }
  ]
}
```

## Plugins API
Plugins can extend the behaviour of the achievement system by adding new achievements, conditions, or
triggers.

Conditions and Triggers should be registered before `GameDataInit.Initialize()` runs, and Achievements should be defined after
`GameDataInit.Initialize()` runs.

#### Conditions
Conditions are pretty basic. There are two "types" of Conditions: One with data, and one without.

Generally, the pattern for implementing an achievement condition looks like this:
```cs
public class ConditionName : AchievementCondition
{
    public const string ID = "ConditionId";

    public override string GetID()
    {
        return ID;
    }

    public override bool IsMet()
    {
        // Logic
    }
}
```

For example, take a look at the `IsDownedCondition`:
```cs
public sealed class IsDownedCondition : AchievementCondition<IsDownedCondition.CustomData>
{
    public const string ID = "IsDowned";

    public override string GetID()
    {
        return ID;
    }

    public override bool IsMet()
    {
        return this.Data.IsValid();
    }

    public sealed class CustomData : ConditionData
    {
        public PlayerRestrictions? Restrictions { get; set; }

        public bool IsValid()
        {
            return this.Restrictions?.CheckForOnePlayer((player) => player.Locomotion.m_currentStateEnum == PlayerLocomotion.PLOC_State.Downed) ?? true;
        }
    }
}
```

Here, Data is needed, and thus another type is needed which extends `ConditionData`. Then pass that type in as a generic parameter to `AchievementCondition`

To register a condition, use `AchievementManager`:

```cs
AchievementManager.Registry.Conditions.Register<MyCondition>();
```
#### Triggers
Triggers are a little more complex. They have to manage achievement progress, as well as
actually setup triggering the AchievementAPI.

Generally, the pattern for implementing an achievement trigger looks like this:
```cs
[TriggerPatches(typeof(Patches))]
public class TriggerName : AchievementTrigger
{
    public const string ID = "TriggerId";

    public override string GetID()
    {
        return ID;
    }

    public override void Trigger(object?[] data, ref AchievementTriggerProgress progress)
    {
        progress.TriggerCount++;
    }

    private static class Patches
    {
        // Harmony Patches here
        public static void MyListenerPatch()
        {
            AchievementManager.Trigger(ID);
        }
    }
}
```

It's a bit more involved! There now is a method called `Trigger`, which takes in `AchievementTriggerProgress`, and an array of data.
This is to allow transferring data between the listener patch and the trigger itself.

For example, take a look at the `ExpeditionFailTrigger`:
```cs
[TriggerPatches(typeof(Patches))]
public class ExpeditionFailTrigger : AchievementTrigger
{
    public const string ID = "FailExpedition";

    public override string GetID()
    {
        return ID;
    }

    public override void Trigger(object?[] data, ref AchievementTriggerProgress progress)
    {
        progress.TriggerCount++;
    }
    private static class Patches
    {
        [HarmonyPatch(typeof(GS_ExpeditionFail), nameof(GS_ExpeditionFail.Enter))]
        [HarmonyPostfix]
        public static void ExpeditionFail()
        {
            AchievementManager.ActivateTrigger(ID);
        }
    }
}
```

For more in depth information on sending data over from the listener, look at `EnemyBiotrackedTrigger.cs`

To register a trigger, once again use `AchievementManager`:

```cs
AchievementManager.Registry.Triggers.Register<MyTrigger>();
```

#### Achievements
To create achievements, create a instantiate a new `AchievementDefinition`. Customize it to your liking.
Finally, to register it, use `AchievementManager`:

```cs
AchievementManager.Registry.Achievements.Register(myAchievement);
```


#### Special Thanks
- Project Zaero for suggesting this idea.