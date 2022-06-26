# IsCrouchedCondition (ID: IsCrouched)
A condition for when a player is crouching.

## Data - MainData
The data for this condition

### MainData

#### Restrictions - [PlayerRestrictions](../Common/PlayerRestrictions.md)
The restrictions of the crouching player.

# Example JSON

Local Player must be crouching.
```json
{
    "ID": "IsCrouched",
    "Data": {
        "Restrictions": {
            "LocalPlayer": {
                "Include": true
            },
            "Players": {
                "Include": false
            },
            "Bots": {
                "Include": false
            }
        }
    }
}
```