# ObjectiveCompleteTrigger (ID: CompleteObjective)
A trigger that is activated when an objective is completed.

## Data - MainData
The data for this trigger.

### MainData

#### ResetOnLevelEnd - Boolean
Whether or not the progress for this trigger resets when the level ends.

#### ObjectiveLayer - LG_LayerType
The layer of the objective. Can be a number (`0`, `1`, or `2`), or a string (`"MainLayer"`, `"SecondaryLayer"`, or `"ThirdLayer"`)

#### Objectives - [WardenObjectiveRestrictions](../Common/WardenObjectiveRestrictions.md)
Restrictions to activate for specific warden objectives.

# Example JSON

Player must complete R7A1 High
```json
{
    "ID": "CompleteObjective",
    "Count": 1,
    "ConditionOverrides": {
        "HasOverrides": false,
        "AdditionalConditions": []
    },
    "Data": {
        "ResetOnLevelEnd": true,
        "ObjectiveLayer": "MainLayer",
        "Objectives": {
            "UseWhiteList": true,
            "WhiteList": [
                146
            ],
            "BlackList": []
        }
    }
}
```