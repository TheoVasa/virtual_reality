using UnityEngine;
using UnityEngine.SceneManagement; // Added for scene management

public class TPtoScene : MonoBehaviour
{
    // Public string to specify the scene name in the Inspector
    public string sceneNameToLoad;

    // This public method can be assigned to a UI Button's OnClick event
    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(sceneNameToLoad))
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }
        else
        {
            Debug.LogError("Scene name to load is not specified in the TPtoScene script.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
