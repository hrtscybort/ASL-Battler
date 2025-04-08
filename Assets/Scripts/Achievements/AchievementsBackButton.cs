using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AchievementsBackButton : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneLoader.Instance.UnloadScene("Achievements");
        
        SceneManager.LoadScene("Main Menu");
    }
}