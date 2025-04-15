using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementEntry : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text categoryTitle;
    [SerializeField] private TMP_Text earnedDescription;
    [SerializeField] private TMP_Text nextTierDescription;
    [SerializeField] private Image tierIcon;

    [Header("Tier Icons")]
    [SerializeField] private Sprite bronzeSprite;
    [SerializeField] private Sprite silverSprite;
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite secretSprite;
    [SerializeField] private Sprite lockedSprite;

    public void DisplayAchievement(Achievement current, Achievement next, string categoryName)
    {
        Debug.Log($"Displaying Achievement: {categoryName} - {current.description}");

        categoryTitle.text = categoryName.ToUpper();

        if (current.isUnlocked)
        {
            earnedDescription.text = current.description;
            tierIcon.sprite = GetTierSprite(current.tier);
        }
        else
        {
            earnedDescription.text = "Not Yet Earned";
            tierIcon.sprite = lockedSprite;
        }

        if (!current.isUnlocked)
        {
            nextTierDescription.text = $"Next: {current.description}";
        }
        else if (next != null && next != current)
        {
            if (current.tier == Achievement.Tier.Gold && next.tier == Achievement.Tier.Secret)
            {
                nextTierDescription.text = "Next: ??? (Secret)";
            }
            else
            {
                nextTierDescription.text = $"Next: {next.description}";
            }
        }
        else
        {
            nextTierDescription.text = "Highest Tier Earned";
        }
    }

    private Sprite GetTierSprite(Achievement.Tier tier)
    {
        return tier switch
        {
            Achievement.Tier.Bronze => bronzeSprite,
            Achievement.Tier.Silver => silverSprite,
            Achievement.Tier.Gold => goldSprite,
            Achievement.Tier.Secret => secretSprite,
            _ => lockedSprite
        };
    }
}