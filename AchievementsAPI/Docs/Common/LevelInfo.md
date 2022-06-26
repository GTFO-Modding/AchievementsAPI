# LevelInfo
Information to reference a specific level

## Fields

### ExpeditionIndex - int
The Expedition Index, or the increment of the level starting at 0. To reference the third expedition in a tier, you would use a value of `2`

### Tier - eRundownTier
The Expedition tier. Can be a number (`1`, `2`, `3`, `4`, `5`, or `99`) or string (`"TierA"`, `"TierB"`, `"TierC"`, `"TierD"`, `"TierE"`, or `"Surface"`)

## Example JSON
Reference A1:
```json
{
    "ExpeditionIndex": 0,
    "Tier": "TierA"
}
```

Reference B3:
```json
{
    "ExpeditionIndex": 2,
    "Tier": 2
}
```