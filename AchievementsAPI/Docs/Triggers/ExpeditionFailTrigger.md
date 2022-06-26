# ExpeditionFailTrigger (ID: FailExpedition)
A trigger that is activated when the player fails the expedition.

## Data
None.

# Example JSON

Player must fail 5 expeditions.
```json
{
    "ID": "FailExpedition",
    "Count": 5,
    "ConditionOverrides": {
        "HasOverrides": false,
        "AdditionalConditions": []
    }
}
```