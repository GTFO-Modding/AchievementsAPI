# ExpeditionSuccessTrigger (ID: CompleteExpedition)
A trigger that is activated when the player completes an expedition.

## Data
None.

# Example JSON

Player must complete 5 expeditions.
```json
{
    "ID": "CompleteExpedition",
    "Count": 5,
    "ConditionOverrides": {
        "HasOverrides": false,
        "AdditionalConditions": []
    }
}
```