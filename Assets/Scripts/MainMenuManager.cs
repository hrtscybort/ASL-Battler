using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button achievementsButton;
    
    void Start()
    {
        achievementsButton.onClick.RemoveAllListeners();
        achievementsButton.onClick.AddListener(ShowAchievements);
    }

    void ShowAchievements()
    {
        SceneLoader.Instance.LoadSceneAdditive("Achievements");
    }
}