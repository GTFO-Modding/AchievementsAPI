# MinMaxRestriction
Takes in a generic parameter which specifies the type of value this MinMaxRestriction holds.

For example, if you see `MinMaxRestriction<Float>`, then you would know it's a reference to this where all values of type `T` are replaced with `Float`

## Fields

### Min - [ValueRestriction](ValueRestriction.md)&lt;*T*&gt;
The minimum value

### Max - [ValueRestriction](ValueRestriction.md)&lt;*T*&gt;
The maximum value.

## Example JSON
For a ValueRestriction&lt;Float&gt;, have a value between 0 and 4
```json
{
    "Min": {
        "Enabled": true,
        "Value": 0.0
    },
    "Max": {
        "Enabled": true,
        "Value": 4.0
    }
}
```

For a ValueRestriction&lt;Integer&gt;, have a value that must be greater than or equal to 4
```json
{
    "Min": {
        "Enabled": true,
        "Value": 4
    },
    "Max": {
        "Enabled": false
    }
}
```

For a ValueRestriction&lt;Double&gt;, have a value that must be less than or equal to 12
```json
{
    "Min": {
        "Enabled": false
    },
    "Max": {
        "Enabled": true,
        "Value": 12.0
    }
}
```