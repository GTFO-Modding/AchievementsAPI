# EnemyDeathTrigger (ID: EnemyDeath)
A trigger activated when an enemy dies.

## Data - MainData
The data for this trigger.

### MainData

#### Players - [PlayerRestrictions](../Common/PlayerRestrictions.md)
The restrictions for the player killing the enemy.

#### Enemies - [EnemyRestrictions](../Common/EnemyRestrictions.md)
The restrictions of the enemy killed.

## Example JSON
Team must kill 10 Baby Strikers

```json
{
    "ID": "EnemyDeath",
    "Count": 10,
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
                "Include": true
            },
            "Bots": {
                "Include": true
            }
        },
        "Enemies": {
            "UseWhiteList": true,
            "WhiteList": [
                38
            ],
            "BlackList": []
        }
    }
}
```