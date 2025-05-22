using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BotDetectionArea : MonoBehaviour
{
    private bool BotColliderisActive; //Boolean to check if the Bot Collider is active or not

    public Transform Rig;
    public Transform Camera;
    public Transform initialPlayerPosition;

    public GameObject gameOverHUD;

    void Start()
    {
        StartCoroutine(PositionRigAfterXRSetup());
    }

    IEnumerator PositionRigAfterXRSetup()
    {
        yield return null; // wait 1 frame for XR camera to initialize

        Vector3 cameraOffset = Camera.localPosition;
        Rig.position = initialPlayerPosition.position - new Vector3(cameraOffset.x, 0, cameraOffset.z);
        Rig.rotation = initialPlayerPosition.rotation;
    }

    void Update()
    {
        BotColliderisActive = gameObject.activeSelf; //Each frame check if the collider is active or not
    }

    void OnTriggerEnter(Collider other)
    {
        //if the player collider enter the bot collider when the box collider is active then GameOver (or start increase stressbar if time to do it)
        if (other.CompareTag("Player") && BotColliderisActive)
        {
            Debug.Log("Enter into the Dection area of the Bot");
            Debug.Log("GameOver");
            GameOver();
        }
    }

    void GameOver()
    {
        StartCoroutine(GameOverHUDDisplay()); 
    }

    IEnumerator GameOverHUDDisplay()
    {
        gameOverHUD.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(1f);
        gameOverHUD.SetActive(false);
    }
}
