using UnityEngine;
using TMPro;

public class FeedbackTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    public void Show(string message, Color color)
    {
        messageText.text = message;
        messageText.color = color;

        LeanTween.scale(gameObject, Vector3.one, 0.2f).setEaseOutBack();
        LeanTween.delayedCall(1.5f, () => Destroy(gameObject));
    }
}
