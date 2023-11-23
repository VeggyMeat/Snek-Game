using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public static class AOEEffect
{
    private static GameObject circle = null;
    private static GameObject rectangle = null;

    private static string jsonPath;

    public static void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Misc/AOEEffect.json";

        // loads in the text from the file
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        // gets the data from the json
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);

        // gets the objects for the circle and rectangle
        circle = Resources.Load<GameObject>(data["AOEEffectCircle"]);
        rectangle = Resources.Load<GameObject>(data["AOEEffectRectangle"]);
    }

    public static void CreateCircle(Vector2 position, float timeAlive, bool decay, Color colour, float radius)
    {
        if (circle is null)
        {
            throw new Exception("AOEEffect not setup");
        }

        // gets the circle that has been created
        GameObject circleCreated = Object.Instantiate(circle, position, Quaternion.identity);

        // gets the AOEEffectController
        AOEEffectControllerCircle controller = circleCreated.GetComponent<AOEEffectControllerCircle>();

        // sets up the controller
        controller.Setup(timeAlive, decay, colour, radius);
    }

    public static void CreateRectangle(Vector2 position, float timeAlive, bool decay, Color colour, float angle, float height, float width)
    {
        if (rectangle is null)
        {
            throw new Exception("AOEEffect not setup");
        }

        // gets the rectangle object that has been created
        GameObject rectangleCreated = Object.Instantiate(rectangle, position, Quaternion.identity);

        // gets the AOEEffectController
        AOEEffectControllerRectangle controller = rectangleCreated.GetComponent<AOEEffectControllerRectangle>();

        // sets up the controller
        controller.Setup(timeAlive, decay, colour, angle, height, width);
    }
}
