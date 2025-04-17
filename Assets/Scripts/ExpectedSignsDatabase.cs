using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ExpectedSignsDatabase : MonoBehaviour
{
    private Dictionary<string,string> _expectedHandshapes;
    private Dictionary<string,string> _expectedLocations;
    private bool correctHandshape = false; 
    private bool correctLocation = false;
    public bool CorrectHandshape => correctHandshape;
    public bool CorrectLocation => correctLocation;

    void Awake()
    {
        var handshapeText = Resources.Load<TextAsset>("category_to_handshape");
        var locationText  = Resources.Load<TextAsset>("category_to_minor_loc");
        
        /*
        if (handshapeText == null || locationText == null)
            Debug.LogError("Make sure those JSONs are in Assets/Resources/");
        */

        _expectedHandshapes = JsonConvert
            .DeserializeObject<Dictionary<string,string>>(handshapeText.text);
        _expectedLocations  = JsonConvert
            .DeserializeObject<Dictionary<string,string>>(locationText.text);
    }

    public int CountCorrectFields(PhonemePrediction result)
    {
        int correct = 0;

        if (_expectedHandshapes.TryGetValue(result.gloss, out var expectedShape) &&
            (expectedShape == "N/A" || expectedShape == result.handshape))
        {
            correctHandshape = true;
            correct++;
        }

        if (_expectedLocations.TryGetValue(result.gloss, out var expectedLoc) &&
            (expectedLoc == "N/A" || expectedLoc == result.minorLocation))
        {
            correctHandshape = true;
            correct++;
        }

        return correct;
    }
}
