using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Item
{
    protected string itemName;
    protected string itemDescription;

    protected string jsonPath;
    protected Dictionary<string, object> jsonData;

    public string ItemName { get => itemName; }
    public string ItemDescription { get => itemDescription; }
    
    internal virtual void Setup()
    {

    }

    protected virtual void JsonSetup()
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);

        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "itemName":
                    itemName = (string)jsonData[item];
                    break;
                case "itemDescription":
                    itemDescription = (string)jsonData[item];
                    break;
            }
        }
    }
}
