using UnityEngine;
using Engine; 
using System.Collections.Generic;
using Common;
using System;
using Model;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Assets.Scripts.Combat;

public class SLRPhonemeTest : MonoBehaviour
{
    public PhonemeExecutionEngine engine;
    [SerializeField] private BattleSystem battleSystem;
    private bool init;

    private int frame = 0;
    private List<string> levelSigns = new List<string>
    {
        "hesheit",
        "dad",
        "mom",
        "boy",
        "girl"
    };

    void Update()
    {
        if(!init) {
            // where initialization goes
            Debug.Log("Added Callback");
            engine.recognizer.AddCallback("print", pred => Debug.Log("Got Pred: " + pred));
            engine.recognizer.outputFilters.Clear();
            engine.recognizer.outputFilters.Add(new Thresholder<string>(0.1f));
            engine.recognizer.outputFilters.Add(new FocusSublistFilter<string>(levelSigns));

            init = true;
        }

        /*
        if (frame == 100) {
             frame = 0;
             //Debug.Log("Adding Callback!");
             engine.buffer.TriggerCallbacks();
             engine.recognizer.AddCallback("print", pred => Debug.Log("Got Pred: " + pred));
        }
        else frame++;
        */
        //Debug.Log(frame);
        // engine.buffer.trigger = new CapacityFullTrigger<HandLandmarkerResult>(80);
        if (battleSystem.CurrentState is PlayerTurn playerTurn && playerTurn.WaitingForSign) {
            engine.buffer.TriggerCallbacks();
        }
    }
}
