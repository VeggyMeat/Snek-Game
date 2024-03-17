using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

// COMPLETE

/// <summary>
/// Handles the variables from a json file
/// </summary>
public class JsonVariable
{
    /// <summary>
    /// Holds the data from the json file
    /// </summary>
    private List<Dictionary<string, object>> variablesList;

    /// <summary>
    /// The current index of the variables list (normally refers to the current level)
    /// </summary>
    private int index;

    /// <summary>
    /// The current set of variables
    /// </summary>
    private Dictionary<string, object> variables;

    /// <summary>
    /// The current set of variables
    /// </summary>
    public Dictionary<string, object> Variables { get => variables; }

    /// <summary>
    /// Path to the Json file
    /// </summary>
    private string path;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonVariable"/> class.
    /// </summary>
    /// <param name="path">The path to the json file</param>
    public JsonVariable(string path)
    {
        this.path = path;

        LoadJson();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonVariable"/> class.
    /// </summary>
    /// <param name="path">The path to the json file</param>
    /// <param name="index">The initial index value the JsonVariable should have</param>
    public JsonVariable(string path, int index) : this(path)
    {
        for (int i = 0; i < index; i++)
        {
            IncreaseIndex();
        }
    }

    /// <summary>
    /// Loads in the json file into the variables list
    /// </summary>
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

    /// <summary>
    /// Increases the index of the variables list, and updates the variables
    /// </summary>
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
