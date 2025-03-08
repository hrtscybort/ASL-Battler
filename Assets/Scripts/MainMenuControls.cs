using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControls : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Main");
    }
}
