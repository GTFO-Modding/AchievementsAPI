# ValueRestriction
A restriction for a specific value.

Takes in a generic parameter which specifies the type of value this MinMaxRestriction holds.

For example, if you see `ValueRestriction<Float>`, then you would know it's a reference to this where all values of type `T` are replaced with `Float`

## Fields

### Enabled - Boolean
Whether or not this restriction is enabled.

### Value - *T*
The value of this restriction. Can be excluded if `Enabled` is set to false.

## Example JSON
These examples use ValueRestriction&lt;Boolean&gt; for whether or not the player must be aiming down sights.

Player must aim down sight:
```json
{
    "Enabled": true,
    "Value": true
}
```

Player must not be aiming down sight:
```json
{
    "Enabled": true,
    "Value": false
}
```

Doesn't matter if the player is aiming down sights:
```json
{
    "Enabled": false,
}
```