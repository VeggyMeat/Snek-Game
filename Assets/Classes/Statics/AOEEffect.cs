using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

// COMPLETE

/// <summary>
/// Handles the creation of AOEEffects
/// </summary>
public static class AOEEffect
{
    /// <summary>
    /// The prefab for the circle AOEEffect object
    /// </summary>
    private static GameObject circle = null;

    /// <summary>
    /// The prefab for the rectangle AOEEffect object
    /// </summary>
    private static GameObject rectangle = null;

    /// <summary>
    /// Holds the path to the json file containing information for the AOEEffect prefabs
    /// </summary>
    private const string jsonPath = "Assets/Resources/Jsons/Misc/AOEEffect.json";

    /// <summary>
    /// Called to setup the AOEEffect class by loading in the prefabs from the resources folder at the start of the game
    /// </summary>
    /// <exception cref="Exception">Throws an exception when the prefabs for the AOEEffects cannot be found</exception>
    public static void Setup()
    {
        // loads in the text from the file
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        // gets the data from the json
        Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);

        // gets the object for the circle
        circle = Resources.Load<GameObject>(data["AOEEffectCircle"].ToString());

        // throws an error if the circle is not found in the resource folder
        if (circle is null)
        {
            throw new Exception("AOEEffectCircle not found");
        }

        // gets the object for the rectangle
        rectangle = Resources.Load<GameObject>(data["AOEEffectRectangle"].ToString());

        // throws an error if the rectangle is not found in the resource folder
        if (rectangle is null)
        {
            throw new Exception("AOEEffectRectangle not found");
        }
    }

    /// <summary>
    /// Creates a circle AOEEffect
    /// </summary>
    /// <param name="position">The position where to create the circle AOEEffect</param>
    /// <param name="timeAlive">How long the AOEEffect should stay for</param>
    /// <param name="decay">Whether the colour should decay over time (true) or not (false)</param>
    /// <param name="colour">The initial colour of the AOEEffect</param>
    /// <param name="radius">The radius of the circle for the AOEEffect</param>
    public static void CreateCircle(Vector2 position, float timeAlive, bool decay, Color colour, float radius)
    {
        // gets the circle that has been created
        GameObject circleCreated = Object.Instantiate(circle, new Vector3(position.x, position.y, 5), Quaternion.identity);

        // gets the AOEEffectController
        AOEEffectControllerCircle controller = circleCreated.GetComponent<AOEEffectControllerCircle>();

        // sets up the controller
        controller.Setup(timeAlive, decay, colour, radius);
    }

    /// <summary>
    /// Creates a rectangle AOEEffect
    /// </summary>
    /// <param name="position">The position where to create the rectangle AOEEffect</param>
    /// <param name="timeAlive">How long the AOEEffect should stay for</param>
    /// <param name="decay">Whether the colour should decay over time (true) or not (false)</param>
    /// <param name="colour">The initial colour of the AOEEffect</param>
    /// <param name="angle">The angle of rotation in degrees that it should be rotated initially</param>
    /// <param name="height">The height of the AOEEffect</param>
    /// <param name="width">The width of the AOEEffect</param>
    public static void CreateRectangle(Vector2 position, float timeAlive, bool decay, Color colour, float angle, float height, float width)
    {
        // gets the rectangle object that has been created
        GameObject rectangleCreated = Object.Instantiate(rectangle, new Vector3(position.x, position.y, 5), Quaternion.identity);

        // gets the AOEEffectController
        AOEEffectControllerRectangle controller = rectangleCreated.GetComponent<AOEEffectControllerRectangle>();

        // sets up the controller
        controller.Setup(timeAlive, decay, colour, angle, height, width);
    }
}
