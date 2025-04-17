using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text wordText;
    public TMP_Text WordText => wordText;
    
    public bool IsDone { get; private set; }
    public float Score { get; private set; }

    public void Show(string word)
    {
        gameObject.SetActive(true);
        IsDone = false;
        wordText.text = word;
    }

    public void Finish(float score)
    {
        Score = score;
        IsDone = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}