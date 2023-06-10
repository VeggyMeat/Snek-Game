using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# nullable enable

public class Buff: MonoBehaviour
{
    private float multiplier = 1f;
    private float adder = 0f;

    private float value;
    private float originalValue;

    public float Value { get { return value; } }

    public Action<float, bool>? update;

    public void Setup (Action<float, bool>? update, float originalValue)
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

    public void AddBuff(float amount, bool multiplicative, float? duration)
    {
        // starts the coroutine with the respective buffs
        StartCoroutine(InternalAddBuff(amount, multiplicative, duration));
    }

    private IEnumerator InternalAddBuff(float amount, bool multiplicative, float? duration)
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

        if (duration is not null)
        {
            // waits
            yield return new WaitForSeconds((float)duration);
        }
        else
        {
            yield return null;
        }

        // if the effect is not permanent
        if (duration is not null)
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
