# EnemyRestrictions
Restrictions for specific enemies from EnemyDataBlock.

## Fields

### UseWhiteList - Boolean
Whether or not to use the whitelist or blacklist.

### WhiteList - Array<[DatablockReference](DatablockReference.md)>
Enemies allowed.

### BlackList - Array<[DatablockReference](DatablockReference.md)>
Enemies not allowed.

## Example JSON

Include only scouts
```json
{
    "UseWhiteList": true,
    "WhiteList": [
        "Scout",
        "Scout_Bullrush",
        "Scout_Shadow"
    ],
    "BlackList": []
}
```

Exclude baby strikers.
```json
{
    "UseWhiteList": false,
    "WhiteList": [],
    "BlackList": [
        38
    ]
}
```