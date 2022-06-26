# PlayerRestrictions
Representation of restrictions for players.

## Fields

### LocalPlayer - [PlayerGroupRestrictions](PlayerGroupRestrictions.md)
Restrictions for the local player.

### Players - [PlayerGroupRestrictions](PlayerGroupRestrictions.md)
Restrictions for the non-local players.

### Bots - [PlayerGroupRestrictions](PlayerGroupRestrictions.md)
Restrictions for bots.

## Example JSON

```json
{
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
```