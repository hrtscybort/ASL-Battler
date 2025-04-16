using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ExpectedSignsDatabase : MonoBehaviour
{
    private Dictionary<string,string> _expectedHandshapes;
    private Dictionary<string,string> _expectedLocations;

    void Awake()
    {
        var handshapeText = Resources.Load<TextAsset>("category_to_handshape");
        var locationText  = Resources.Load<TextAsset>("category_to_minor_location");
        
        /*
        if (handshapeText == null || locationText == null)
            Debug.LogError("Make sure those JSONs are in Assets/Resources/");
        */

        _expectedHandshapes = JsonConvert
            .DeserializeObject<Dictionary<string,string>>(handshapeText.text);
        _expectedLocations  = JsonConvert
            .DeserializeObject<Dictionary<string,string>>(locationText.text);
    }
    /*
    public int CountCorrectFields(result)
    {
        int correct = 0;

        if (_expectedHandshapes.TryGetValue(result.wordDetected, out var expectedShape) &&
            (expectedShape == "N/A" || expectedShape == result.handshapeDetected))
        {
            correct++;
        }

        if (_expectedLocations.TryGetValue(result.wordDetected, out var expectedLoc) &&
            (expectedLoc == "N/A" || expectedLoc == result.locationDetected))
        {
            correct++;
        }

        return correct;
    }
    */
}
