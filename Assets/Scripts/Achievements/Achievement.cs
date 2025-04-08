using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievements/Achievement")]
public class Achievement : ScriptableObject
{
    public enum Tier { Bronze, Silver, Gold, Secret }
    public enum Category 
    { 
    MonsterHunter, 
    BossSlayer, 
    SignWizard, 
    ComboSigner, 
    AllAchievements 
    }

    public string achievementID;
    public string title;
    public string description;
    public Tier tier;
    public Category category;
    public int targetValue;
    public bool isHidden;
    public Sprite icon;
    public bool isUnlocked;

    public System.Action<Achievement> OnUnlocked;
    public void Unlock()
    {
        isUnlocked = true;
        OnUnlocked?.Invoke(this);
    }
}
