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

    /// <summary>
    /// The new value of the buff
    /// </summary>
    public float Value { get { return value; } }

    private Action<float, bool>? update;

    /// <summary>
    /// Sets up the buff
    /// </summary>
    /// <param name="update">The action that is called when this buff is changed</param>
    /// <param name="originalValue">The original value given to the buff to hold and return as .Value</param>
    public void Setup(Action<float, bool>? update, float originalValue)
    {
        this.update = update;
        this.originalValue = originalValue;
        UpdateValue(originalValue);
    }

    private void UpdateValue(float value)
    {
        this.value = (value + adder) * multiplier;
    }

    public void UpdateOriginalValue(float originalValue)
    {
        this.originalValue = originalValue;
        UpdateValue(originalValue);
    }

    /// <summary>
    /// Adds an effect to the buff
    /// </summary>
    /// <param name="amount">The change either being added or multiplied to the value</param>
    /// <param name="multiplicative">Whether the effect should be added to the value (false) or a multiplier effect (true)</param>
    /// <param name="duration">How long this effect should last (null = infinite)</param>
    public void AddBuff(float amount, bool multiplicative, float? duration)
    {
        // starts the coroutine with the respective buffs
        StartCoroutine(InternalAddBuff(amount, multiplicative, duration));
    }

    private IEnumerator InternalAddBuff(float amount, bool multiplicative, float? duration)
    {
        // if its any other value suggesting the duration should not end, set it to null
        if (duration <= 0 || duration == float.PositiveInfinity || duration == float.NegativeInfinity)
        {
            duration = null;
        }

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
        UpdateValue(originalValue);

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
            UpdateValue(originalValue);

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
