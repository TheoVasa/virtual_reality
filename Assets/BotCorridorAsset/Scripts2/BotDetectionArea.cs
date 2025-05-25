//Bot dection Area script developped by Iris

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BotDetectionArea : MonoBehaviour
{
    private bool BotColliderisActive; //Boolean to check if the Bot Collider is active or not

    public Transform Rig; //Transform of the Rig
    public Transform Camera; //Transform of the Camera
    public Transform initialPlayerPosition; //Initial Position of the Player for reload

    public GameObject gameOverHUD; //GameOver HUD

    public AudioSource gameOverSound; //GameOver Sécuritas voice

    public Animator BotAnimator; //Animator of the bot

    void Start()
    {
        //Start the initialization of the position of the Player (Rig and Camera) routine
        StartCoroutine(PositionRigAfterXRSetup());
    }

    IEnumerator PositionRigAfterXRSetup()
    {
        //Wait 1 frame for XR camera to initialize
        yield return null;

        //Obtain the position of Camera and Player
        Vector3 cameraOffset = Camera.localPosition;
        Rig.position = initialPlayerPosition.position - new Vector3(cameraOffset.x, 0, cameraOffset.z);
        Rig.rotation = initialPlayerPosition.rotation;
    }

    void Update()
    {
        //Each frame check if the collider is active or not
        BotColliderisActive = gameObject.activeSelf;
    }

    void OnTriggerEnter(Collider other)
    {
        //If the player collider enter the bot collider when the box collider is active then GameOver
        if (other.CompareTag("Player") && BotColliderisActive)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        //Start Gameover routine
        StartCoroutine(GameOverHUDDisplay()); 
    }

    IEnumerator GameOverHUDDisplay()
    {
        //Change animation of bot
        BotAnimator.SetTrigger("Gameover");
        //Wait 3 second
        yield return new WaitForSeconds(3f);
        //Activate voice of Sécuritas
        gameOverSound.Play();
        //Wait 3 second
        yield return new WaitForSeconds(3f);
        //Activate the GameOver UI
        gameOverHUD.SetActive(true);
        //Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Wait 1 second
        yield return new WaitForSeconds(1f);
        //Desactivate the GameOver UI
        gameOverHUD.SetActive(false);
    }
}
