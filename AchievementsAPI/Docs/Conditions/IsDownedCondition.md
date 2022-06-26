# IsDownedCondition (ID: IsDowned)
A condition for when a player is downed.

## Data - MainData
The data for this condition

### MainData

#### Restrictions - [PlayerRestrictions](../Common/PlayerRestrictions.md)
The restrictions of the downed player.

# Example JSON

A non-local player must be downed.
```json
{
    "ID": "IsCrouched",
    "Data": {
        "Restrictions": {
            "LocalPlayer": {
                "Include": false
            },
            "Players": {
                "Include": true
            },
            "Bots": {
                "Include": true
            }
        }
    }
}
```