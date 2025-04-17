using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FeedbackPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private GameObject[] stars = new GameObject[3];

    private RectTransform rectTransform;
    private Vector2 hiddenPosition;
    private Vector2 shownPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        hiddenPosition = new Vector2(24.7499f, 350);
        shownPosition = new Vector2(24.7499f, 253.13f);

        rectTransform.anchoredPosition = hiddenPosition;
        gameObject.SetActive(false);
    }

    public void ShowFeedback(bool isCorrectWord, bool correctHandshape, bool correctLocation)
    {
        string message;
        Color color = Color.white;

        if (!isCorrectWord)
        {
            message = "Wrong word!";
            color = Color.red;
        }
        else if (correctHandshape && correctLocation)
        {
            message = "Perfect!";
            color = Color.green;
        }
        else if (!correctHandshape && !correctLocation)
        {
            message = "Wrong handshape \n and location!";
            color = Color.red;
        }
        else if (!correctHandshape)
        {
            message = "Wrong handshape!";
            color = Color.yellow;
        }
        else // Only location is wrong
        {
            message = "Wrong location!";
            color = Color.yellow;
        }

        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }

        UpdateStars(isCorrectWord, correctHandshape, correctLocation);

        gameObject.SetActive(true);
        rectTransform.anchoredPosition = hiddenPosition;

        LeanTween.move(rectTransform, shownPosition, 0.5f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(() => {
                LeanTween.delayedCall(displayTime, HideFeedback);
            });
    }

    private void UpdateStars(bool wordCorrect, bool handshapeCorrect, bool locationCorrect)
    {
        stars[0].SetActive(wordCorrect);
        stars[1].SetActive(handshapeCorrect);
        stars[2].SetActive(locationCorrect);
    }

    private void HideFeedback()
    {
        LeanTween.move(rectTransform, hiddenPosition, 0.5f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() => gameObject.SetActive(false));
    }
}
