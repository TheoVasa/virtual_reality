using UnityEngine;
using UnityEngine.SceneManagement;

public class BotDetectionArea : MonoBehaviour
{
    private bool BotColliderisActive; //Boolean to check if the Bot Collider is active or not

    public Transform Rig;
    public Transform Camera;

    private Vector3 initialRigPosition;
    private Quaternion initialRigRotation;

    void Start()
    {
        initialRigPosition = Rig.position;
        initialRigRotation = Rig.rotation;
    }

    void Update()
    {
        BotColliderisActive = gameObject.activeSelf; //Each frame check if the collider is active or not
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Tag of other" + other.gameObject.tag);
        //if the player collider enter the bot collider when the box collider is active then GameOver (or start increase stressbar if time to do it)
        if (other.CompareTag("Player") && BotColliderisActive)
        {
            Debug.Log("Enter into the Dection area of the Bot");
            Debug.Log("GameOver");
            ResetXRPlayerPosition();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload the scene
        }
    }

    void ResetXRPlayerPosition()
    {
        Vector3 cameraOffset = Camera.localPosition;

        Rig.position = initialRigPosition - new Vector3(cameraOffset.x, 0, cameraOffset.z);
        Rig.rotation = initialRigRotation;
    }
}
