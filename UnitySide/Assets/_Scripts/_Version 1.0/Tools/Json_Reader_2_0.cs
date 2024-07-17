using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Json_Reader_2_0 : MonoBehaviour
{
    [Header("Instances")] public TextAsset JsonToRead;

    void Start()
    {
        Dictionary<string, object> parsedData = ParseJson(JsonToRead.text);
        
        foreach (var kvp in parsedData)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value}");
        }
    }

    Dictionary<string, object> ParseJson(string json)
    {
        JArray jsonArray = JArray.Parse(json);

        Dictionary<string, object> combinedData = new Dictionary<string, object>();

        foreach (JObject item in jsonArray)
        {
            foreach (var property in item.Properties())
            {
                if (!combinedData.ContainsKey(property.Name))
                {
                    combinedData[property.Name] = new List<object>();
                }

                ((List<object>)combinedData[property.Name]).Add(property.Value);
            }
        }

        return combinedData;
    }
}
