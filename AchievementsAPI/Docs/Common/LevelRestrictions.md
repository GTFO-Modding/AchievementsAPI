# LevelRestrictions
Restrictions for specific levels.

## Fields

### UseWhiteList - Boolean
Whether or not to use the whitelist or blacklist.

### WhiteList - Array<[LevelInfo](LevelInfo.md)>
The levels that are valid.

### BlackList - Array<[LevelInfo](LevelInfo.md)>
The levels that are invalid.

## Example JSON

Include only A1-A5:
```json
{
    "UseWhiteList": true,
    "WhiteList": [
        {
            "ExpeditionIndex": 0,
            "Tier": "Tier_A"
        },
        {
            "ExpeditionIndex": 1,
            "Tier": "Tier_A"
        },
        {
            "ExpeditionIndex": 2,
            "Tier": "Tier_A"
        },
        {
            "ExpeditionIndex": 3,
            "Tier": "Tier_A"
        },
        {
            "ExpeditionIndex": 4,
            "Tier": "Tier_A"
        }
    ],
    "BlackList": []
}
```

Exclude E1:
```json
{
    "UseWhiteList": false,
    "WhiteList": [],
    "BlackList": [
        {
            "ExpeditionIndex": 0,
            "Tier": 5
        }
    ]
}
```