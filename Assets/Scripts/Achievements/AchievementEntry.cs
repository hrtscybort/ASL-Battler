using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text categoryTitle;
    [SerializeField] private TMP_Text earnedDescription;
    [SerializeField] private TMP_Text nextTierDescription;
    [SerializeField] private Image tierIcon;

    [SerializeField] private Sprite bronzeSprite;
    [SerializeField] private Sprite silverSprite;
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite secretSprite;
    [SerializeField] private Sprite lockedSprite;

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
        
        if (next != null && next != current)
        {
            nextTierDescription.text = $"Next: {next.description} ({next.tier})";
        }
        else
        {
            nextTierDescription.text = "Highest Tier Earned";
        }
    }
}