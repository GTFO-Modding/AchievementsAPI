# ReviveTrigger (ID: Revive)
A trigger activated when a player is revived.

## Data - MainData
The data for this trigger.

### MainData

#### Players - [PlayerRestrictions](../Common/PlayerRestrictions.md)
The restrictions for the player who was revived.

## Example JSON

Local Player must revive teammates 20 times.
```json
{
    "ID": "Revive",
    "Count": 20,
    "ConditionOverrides": {
        "HasOverrides": false,
        "AdditionalConditions": []
    },
    "Data": {
        "Players": {
            "LocalPlayer": {
                "Include": false
            },
            "Players": {
                "Include": false
            },
            "Bots": {
                "Include": false
            }
        }
    }
}
```