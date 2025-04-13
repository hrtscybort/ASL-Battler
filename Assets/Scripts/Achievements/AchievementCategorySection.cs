using UnityEngine;

public class AchievementCategorySection : MonoBehaviour
{
    [SerializeField] private string categoryName;
    [SerializeField] private Transform entriesContainer;
    
    public void ClearEntries()
    {
        foreach (Transform child in entriesContainer)
            Destroy(child.gameObject);
    }

    public string GetCategoryName() => categoryName;
    public Transform GetEntriesContainer() => entriesContainer;
}