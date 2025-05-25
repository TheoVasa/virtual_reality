using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Home_menu : MonoBehaviour
{
    [Header("Scene Names")]
    public string scene1Name; // Field for the first scene name
    public string scene2Name; // Field for the second scene name

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to load the first scene
    public void LoadScene1()
    {
        if (!string.IsNullOrEmpty(scene1Name))
        {
            SceneManager.LoadScene(scene1Name);
        }
        else
        {
            Debug.LogError("Scene 1 name is not set in the Inspector.");
        }
    }

    // Method to load the second scene
    public void LoadScene2()
    {
        if (!string.IsNullOrEmpty(scene2Name))
        {
            SceneManager.LoadScene(scene2Name);
        }
        else
        {
            Debug.LogError("Scene 2 name is not set in the Inspector.");
        }
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting game..."); // For testing in editor
        Application.Quit();
    }
}
