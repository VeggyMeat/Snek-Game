using UnityEngine;

// COMPLETE

/// <summary>
/// Handles extra Vector2 operations
/// </summary>
public static class VectorWrapper
{
    /// <summary>
    /// Gets the angle from a Vector2 to another Vector2
    /// </summary>
    /// <param name="fromPos">The body's position</param>
    /// <param name="toPos">The enemy's position</param>
    /// <returns>Angle in radians between them</returns>
    public static float AngleTo(this Vector2 fromPos, Vector2 toPos)
    {
        float angle;

        // arctan of the triangle between the tower and the enemy
        float sine = Mathf.Atan2(Mathf.Abs(fromPos.y - toPos.y), Mathf.Abs(fromPos.x - toPos.x));

        if (fromPos.x > toPos.x)
        {
            // enemy from the right of fromwer (PI > angle)
            if (fromPos.y > toPos.y)
            {
                // enemy beneath fromwer (PI > angle > PI/2)
                angle = Mathf.PI / 2 + sine;
            }
            else
            {
                // enemy above fromwer (PI/2 > angle > 0)
                angle = Mathf.PI / 2 - sine;
            }
        }
        else
        {
            // enemy from the left of fromwer (angle > PI)
            if (fromPos.y > toPos.y)
            {
                // enemy beneath fromwer (3PI/2 > angle > PI)
                angle = 3 * Mathf.PI / 2 - sine;
            }
            else
            {
                // enemy above fromwer (2PI > angle > 3PI/2)
                angle = 3 * Mathf.PI / 2 + sine;
            }
        }

        return angle;
    }
}
