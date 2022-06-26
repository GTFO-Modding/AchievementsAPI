# LevelRestrictionsCondition (ID: LevelRestrictions)
A condition that will only allow triggers to be activated for specific levels.

## Data - MainData
The data for this condition

### MainData

#### Restrictions - [LevelRestrictions](../Common/LevelRestrictions.md)
The restrictions of levels allowed.

# Example JSON

Player must be on A1
```json
{
    "ID": "LevelRestrictions",
    "Data": {
        "Restrictions": {
            "UseWhiteList": true,
            "WhiteList": [
                {
                    "ExpeditionIndex": 0,
                    "Tier": "TierA"
                }
            ],
            "BlackList": []
        }
    }
}
```