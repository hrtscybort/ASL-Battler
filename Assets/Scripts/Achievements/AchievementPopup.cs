using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour
{
    [SerializeField] private Image tierIcon;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private float displayTime = 3f;
    
    private RectTransform rectTransform;
    private Vector2 hiddenPosition = new Vector2(0, -150);
    private Vector2 shownPosition = new Vector2(0, 50);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = hiddenPosition;
        gameObject.SetActive(false);
    }

    public void Show(Achievement achievement, Sprite tierSprite)
    {
        gameObject.SetActive(true);
        
        tierIcon.sprite = tierSprite;
        titleText.text = achievement.title;
        descText.text = achievement.description;
        
        LeanTween.move(rectTransform, shownPosition, 0.5f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(() => {
                LeanTween.delayedCall(displayTime, Hide);
            });
    }

    private void Hide()
    {
        LeanTween.move(rectTransform, hiddenPosition, 0.5f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() => gameObject.SetActive(false));
    }
}