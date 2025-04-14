using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AchievementsBackButton : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(MainMenu);
        }
        else
        {
            Debug.LogError("Back button is not assigned in the Inspector!");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}