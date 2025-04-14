using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadSceneAdditive(string sceneName)
    {
        if (!IsSceneLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void UnloadScene(string sceneName)
    {
        if (IsSceneLoaded(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
                return true;
        }
        return false;
    }

    public void ShowAchievementMenu()
    {
        SceneManager.LoadScene("Achievements");
    }
}