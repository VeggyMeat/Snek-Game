using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# nullable enable

public class Buff
{
    private float multiplier = 1f;
    private float adder = 0f;

    private float value;
    private float originalValue;

    public float Value { get { return value; } }

    public Action<float, bool>? update;

    public Buff(Action<float, bool>? update, float originalValue)
    {
        this.update = update;
        this.originalValue = originalValue;
        updateValue(originalValue);
    }

    private void updateValue(float value)
    {
        this.value = (value + adder) * multiplier;
    }

    public void updateOriginalValue(float originalValue)
    {
        this.originalValue = originalValue;
        updateValue(originalValue);
    }

    public IEnumerator AddBuff(float amount, bool multiplicative, float duration)
    {

        // adds the buff
        if (multiplicative)
        {
            multiplier *= amount;
        }
        else
        {
            adder += amount;
        }

        // updates the value and tells the owner it was updated
        updateValue(originalValue);

        if (update is not null)
        {
            update(amount, multiplicative);
        }

        // waits
        yield return new WaitForSeconds(duration);

        // if the effect is not permanent
        if (duration != 0)
        {
            // removes the buff
            if (multiplicative)
            {
                multiplier /= amount;
            }
            else
            {
                adder -= amount;
            }

            // updates the value and tells the owner it was updated
            updateValue(originalValue);

            if (update is not null)
            {
                if (multiplicative)
                {
                    update(1 / amount, multiplicative);
                }
                else
                {
                    update(-amount, multiplicative);
                }
            }
        }
    }
}
