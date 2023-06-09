using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Item
{
    internal string itemName;
    internal string itemDescription;
    
    internal virtual void Setup()
    {

    }

    internal void JsonSetup(string json)
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(json);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);
    }

    internal void BodyAdded()
    {

    }
}
