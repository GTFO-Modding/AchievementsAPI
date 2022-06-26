# EnemyMineDamageTrigger (ID: EnemyMineDamage)
A trigger activated when an enemy takes damage from a mine.

## Data - MainData
The data for this trigger.

### MainData

#### Enemies - [EnemyRestrictions](../Common/EnemyRestrictions.md)
The restrictions of the enemy being damaged.

#### Damage - [DamageRestrictionsData](#damagerestrictionsdata)
The damage requirements

#### IsLethal - [ValueRestriction](../Common/ValueRestriction.md)&lt;Boolean&gt;
Whether or not the damage must be lethal.

### DamageRestrictionsData

#### HasRestriction - Boolean
Whether or not there is a damage restriction in place.

#### Restriction - [MinMaxRestriction](../Common/MinMaxRestriction.md)&lt;Float&gt;
The range the damage must fall in.

## Example JSON
Must deal at least 50 damage from Giant with mine

```json
{
    "ID": "EnemyMineDamage",
    "Count": 1,
    "ConditionOverrides": {
        "HasOverrides": false,
        "AdditionalConditions": []
    },
    "Data": {
        "Enemies": {
            "UseWhiteList": true,
            "WhiteList": [
                16,
                28
            ],
            "BlackList": []
        },
        "Damage": {
            "HasRestriction": true,
            "Restriction": {
                "Min": {
                    "Enabled": true,
                    "Value": 50.0
                },
                "Max": {
                    "Enabled": false
                }
            }
        },
        "IsLethal": {
            "Enabled": false
        }
    }
}
```