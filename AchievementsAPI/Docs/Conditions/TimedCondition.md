# TimedCondition (ID: Timed)
A condition that will only allow triggers to be fully completed in a specific amount of time.

## Data - MainData
The data for this condition

### MainData

#### Time - double
The time allowed. The time will start when a trigger is first activated.

# Example JSON

Trigger must be completed in less than 5 seconds.
```json
{
    "ID": "Timed",
    "Data": {
        "Time": 5.0
    }
}
```