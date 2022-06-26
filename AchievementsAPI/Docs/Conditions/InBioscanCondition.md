# InBioscanCondition (ID: InBioscan)
A condition for when a player is in the bioscan.

## Data - MainData
The data for this condition

### MainData

#### Restrictions - [PlayerRestrictions](../Common/PlayerRestrictions.md)
The restrictions of the player in the bioscan.

# Example JSON

Local Player must be in bioscan.
```json
{
    "ID": "InBioscan",
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