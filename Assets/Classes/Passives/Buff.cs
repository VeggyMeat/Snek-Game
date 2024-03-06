using System;
using System.Collections;
using UnityEngine;

# nullable enable

// COMPLETE

/// <summary>
/// Controls the buffs on a specific value for an enemy or body
/// Is attatched to a game object which holds this value
/// </summary>
internal class Buff: MonoBehaviour
{
    /// <summary>
    /// The multiplier of the buff (multiplied after the adder is added)
    /// </summary>
    private float multiplier = 1f;

    /// <summary>
    /// The flat value added to the buff (before the multiplier)
    /// </summary>
    private float adder = 0f;

    private float originalValue;

    /// <summary>
    /// The new value of the buff
    /// </summary>
    public float Value 
    { 
        get 
        { 
            return (originalValue + adder) * multiplier;
        } 
    }

    private Action<float, bool>? update;

    /// <summary>
    /// Sets up the buff
    /// </summary>
    /// <param name="update">The action that is called when this buff is changed</param>
    /// <param name="originalValue">The original value given to the buff to hold and return as .Value</param>
    internal void Setup(Action<float, bool>? update, float originalValue)
    {
        this.update = update;
        this.originalValue = originalValue;
    }

    internal void UpdateOriginalValue(float originalValue)
    {
        this.originalValue = originalValue;
    }

    /// <summary>
    /// Adds an effect to the buff
    /// </summary>
    /// <param name="amount">The change either being added or multiplied to the value</param>
    /// <param name="multiplicative">Whether the effect should be added to the value (false) or a multiplier effect (true)</param>
    /// <param name="duration">How long this effect should last (null = infinite)</param>
    internal void AddBuff(float amount, bool multiplicative, float? duration)
    {
        // starts the coroutine with the respective buffs
        StartCoroutine(InternalAddBuff(amount, multiplicative, duration));
    }

    /// <summary>
    /// Adds an effect to the buff
    /// </summary>
    /// <param name="amount">The change either being added or multiplied to the value</param>
    /// <param name="multiplicative">Whether the effect should be added to the value (false) or a multiplier effect (true)</param>
    /// <param name="duration">How long this effect should last (null = infinite)</param>
    /// <returns></returns>
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

        // calls the update method if it exists
        if (update is not null)
        {
            update(amount, multiplicative);
        }

        // of the duration is not infinite
        if (duration is not null)
        {
            // waits for the duration seconds to pass
            yield return new WaitForSeconds((float)duration);

            // removes the buff
            if (multiplicative)
            {
                multiplier /= amount;
            }
            else
            {
                adder -= amount;
            }

            // calls the update method if it exists
            if (update is not null)
            {
                if (multiplicative)
                {
                    // indicates the amount is a divisor, and the factor
                    update(1 / amount, multiplicative);
                }
                else
                {
                    // indicates the amount is a negative, and the amount
                    update(-amount, multiplicative);
                }
            }
        }
        else
        {
            yield return null;
        }
    }
}
