using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTeleporter : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string targetSceneName;

    IEnumerator TeleportAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        Debug.Log($"Teleporting to scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportAfterDelay(targetSceneName));
        }
    }
}