using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonVariable
{
    /// <summary>
    /// Holds the data from the json file
    /// </summary>
    private List<Dictionary<string, object>> variablesList;

    private int index;

    private Dictionary<string, object> variables;

    public Dictionary<string, object> Variables { get => variables; }

    /// <summary>
    /// Path to the Json file
    /// </summary>
    private string path;

    public JsonVariable(string path)
    {
        this.path = path;

        LoadJson();
    }

    public JsonVariable(string path, int index) : this(path)
    {
        for (int i = 0; i < index; i++)
        {
            IncreaseIndex();
        }
    }

    private void LoadJson()
    {
        // loads in the text from the json file
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        reader.Close();

        // converts the text to the variable list and returns it
        variablesList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);

        variables = variablesList[0];
    }

    public void IncreaseIndex()
    {
        index++;

        // Updates 'variables' for each item in the next dictionary
        foreach (KeyValuePair<string, object> pair in variablesList[index])
        {
            variables[pair.Key] = pair.Value;
        }
    }
}
