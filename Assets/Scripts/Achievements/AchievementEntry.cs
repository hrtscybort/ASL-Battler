using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementEntry : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text categoryTitle;
    [SerializeField] private TMP_Text earnedDescription;
    [SerializeField] private TMP_Text nextTierDescription;
    [SerializeField] private Image tierIcon;

    [Header("Tier Sprites")]
    [SerializeField] private Sprite bronzeSprite;
    [SerializeField] private Sprite silverSprite;
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite secretSprite;
    [SerializeField] private Sprite lockedSprite;

    public void DisplayAchievement(Achievement current, Achievement next, string categoryName)
    {
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
        
        if (next != null)
        {
            nextTierDescription.text = $"Next: {next.description} ({next.tier})";
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