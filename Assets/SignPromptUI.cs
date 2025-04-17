using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text wordText;
    public TMP_Text WordText => wordText;
    // [SerializeField] private TMP_Text feedbackText;
    
    public bool IsDone { get; private set; }
    public float Score { get; private set; }

    public void Show(string word)
    {
        gameObject.SetActive(true);
        IsDone = false;
        wordText.text = word;
        //feedbackText.text = "Sign the word above";
    }

    public void Finish(float score)
    {
        Score = score;
        IsDone = true;
        
        /*
        if (score >= 0.9f) feedbackText.text = "Perfect!";
        else if (score >= 0.6f) feedbackText.text = "Good!";
        else if (score >= 0.3f) feedbackText.text = "Almost!";
        else feedbackText.text = "Try again!";
        */
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}