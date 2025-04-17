using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedbackPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private RectTransform popupTransform;

    private Vector2 hiddenPosition = new Vector2(0.5f, 1.2f); // Above screen
    private Vector2 shownPosition = new Vector2(0.5f, 0.9f);  // Just below top

    private void Start()
    {
        popupTransform.anchorMin = new Vector2(0.5f, 0.5f);
        popupTransform.anchorMax = new Vector2(0.5f, 0.5f);
        popupTransform.pivot = new Vector2(0.5f, 0.5f);
        popupTransform.anchoredPosition = hiddenPosition;
        gameObject.SetActive(false);
    }

    public void Show(string message, bool[] starsStatus)
    {
        gameObject.SetActive(true);
        messageText.text = message;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < starsStatus.Length && starsStatus[i]);
        }

        popupTransform.anchoredPosition = hiddenPosition;

        LeanTween.move(popupTransform, shownPosition, 0.4f).setEaseOutBack().setOnComplete(() =>
        {
            LeanTween.delayedCall(displayDuration, () =>
            {
                LeanTween.move(popupTransform, hiddenPosition, 0.4f).setEaseInBack().setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
        });
    }
}
