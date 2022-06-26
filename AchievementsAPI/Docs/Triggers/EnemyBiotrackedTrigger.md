# EnemyBiotrackedTrigger (ID: EnemyBiotracked)
A trigger activated when an enemy is tagged with the biotracker.

## Data - MainData
The data for this trigger.

### MainData

#### Players - [PlayerRestrictions](../Common/PlayerRestrictions.md)
The restrictions for the player that tags the enemy

#### Enemies - [EnemyRestrictions](../Common/EnemyRestrictions.md)
The restrictions for enemies to be tagged.

#### UniqueOnly - Boolean
Whether or not the tags must be unique, or that this trigger wont activate if an enemy is retagged.

## Example JSON

Local Player must tag 5 unique scouts
```json
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
                "Scout",
                "Scout_Bullrush",
                "Scout_Shadow"
            ],
            "BlackList": []
        },
        "UniqueOnly": true
    }
}
```