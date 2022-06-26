# ItemRestrictions
Restrictions for specific objectives from WardenObjectiveDataBlock.

## Fields

### UseWhiteList - Boolean
Whether or not to use the whitelist or blacklist.

### WhiteList - Array<[DatablockReference](DatablockReference.md)>
Warden Objectives allowed.

### BlackList - Array<[DatablockReference](DatablockReference.md)>
Warden Objectives not allowed.

## Example JSON

Include only R7A1 and R7C1 Objectives
```json
{
    "UseWhiteList": true,
    "WhiteList": [
        146,
        150
    ],
    "BlackList": []
}
```

Exclude E1 Objective
```json
{
    "UseWhiteList": false,
    "WhiteList": [],
    "BlackList": [
        "Reactor_Startup_Waves_4_R7E1"
    ]
}
```