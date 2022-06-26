# ItemRestrictions
Restrictions for specific items from ItemDataBlock.

## Fields

### UseWhiteList - Boolean
Whether or not to use the whitelist or blacklist.

### WhiteList - Array<[DatablockReference](DatablockReference.md)>
Items allowed.

### BlackList - Array<[DatablockReference](DatablockReference.md)>
Items not allowed.

## Example JSON

Include only Mapper and Biotracker
```json
{
    "UseWhiteList": true,
    "WhiteList": [
        "GEAR_Mapper",
        "GEAR_MotionTracker"
    ],
    "BlackList": []
}
```

Exclude Bulkhead Key
```json
{
    "UseWhiteList": false,
    "WhiteList": [],
    "BlackList": [
        146
    ]
}
```