using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class Wrapper
{
    public static void Setup<T>(this Dictionary<string, object> dict, ref T variable, string name) where T : IConvertible
    {
        // if its in the dictionary
        if (dict.ContainsKey(name))
        {
            // then set the variable to the value in the dictionary (type cast it)
            variable = (T)Convert.ChangeType(dict[name], typeof(T));
        }
    }

    public static void Setup(this Dictionary<string, object> dict, ref Color variable, string name)
    {
        // if its in the dictionary
        if (dict.ContainsKey(name))
        {
            string text = dict[name].ToString();

            // get rid of open and close brackets
            text = text.Replace("[", "");
            text = text.Replace("]", "");

            // get rid of all whitespace
            text = text.Replace(" ", "");

            // split by ',' seperator
            string[] split = text.Split(',');

            // if the array is not 3 or 4 items long
            if (split.Count() == 3)
            {
                variable = new Color(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
            }
            else if (split.Count() == 4)
            {
                variable = new Color(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            }
            else 
            {
                throw new Exception("Not a valid length for a colour");
            }
            
        }
    }

    public static void SetupAction<T>(this Dictionary<string, object> dict, ref T variable, string name, Action actionBefore, Action actionAfter, bool doAction)
    {
        // if its in the dictionary
        if (dict.ContainsKey(name))
        {
            if (actionBefore is not null)
            {
                if (doAction)
                {
                    actionBefore();
                }
            }

            variable = (T)Convert.ChangeType(dict[name], typeof(T));

            if (actionAfter is not null)
            {
                if (doAction)
                {
                    actionAfter();
                }
            }
        }
    }

    /// <summary>
    /// Returns a random item from the list
    /// </summary>
    /// <typeparam name="T">The type of class in the list</typeparam>
    /// <param name="ls">The list the item gets chosen from</param>
    /// <returns></returns>
    public static T RandomItem<T>(this List<T> ls)
    {
        return ls[UnityEngine.Random.Range(0, ls.Count - 1)];
    }

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

    /// <summary>
    /// knocks back an enemy away from a position
    /// </summary>
    /// <param name="enemy">The enemy to knockback</param>
    /// <param name="positionFrom">The origin of the knockback force</param>
    static public void KnockbackEnemy(EnemyController enemy, Transform positionFrom, float forceMultiplier)
    {
        enemy.selfRigid.AddForce((enemy.transform.position - positionFrom.position).normalized * forceMultiplier);
    }
}
