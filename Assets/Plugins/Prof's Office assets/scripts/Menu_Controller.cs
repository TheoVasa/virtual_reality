using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Menu_Controller : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("Name of the scene to load for the Home Screen.")]
    public string homeScreenSceneName = "Home Screen"; // Default value, can be changed in Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to restart the current level
    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Method to load the Home Screen scene
    public void LoadHomeScreen()
    {
        if (!string.IsNullOrEmpty(homeScreenSceneName))
        {
            SceneManager.LoadScene(homeScreenSceneName);
        }
        else
        {
            Debug.LogError("Home Screen scene name is not set in the Menu_Controller script.");
        }
    }

    // Method to skip to the next level
    public void SkipLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // Check if the next scene index is within the bounds of the build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene to skip to. This is the last scene or scenes are not in build settings.");
            // Optionally, you could load the home screen or a 'game completed' screen here
            // For example:
            // if (!string.IsNullOrEmpty(homeScreenSceneName))
            // {
            //     SceneManager.LoadScene(homeScreenSceneName);
            // }
        }
    }

    // Method to quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in Editor
#else
        Application.Quit(); // Quits the application in a build
#endif
    }
}
