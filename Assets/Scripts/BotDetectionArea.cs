using UnityEngine;

public class BotDetectionArea : MonoBehaviour
{

    private bool BotColliderisActive; //Boolean to check if the Bot Collider is active or not

    // Update is called once per frame
    void Update()
    {
        BotColliderisActive = gameObject.activeSelf; //Each frame check if the collider is active or not
        Debug.Log("Botcollider is active" + BotColliderisActive);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Tag of other" + other.gameObject.tag);
        //if the player collider enter the bot collider when the box collider is active then GameOver (or start increase stressbar if time to do it)
        if (other.CompareTag("PlayerCollider") && BotColliderisActive)
        {
            Debug.Log("Enter into the Dection area of the Bot");
            Debug.Log("GameOver");
        }
    }

    void OnTriggerStay(Collider other)
    {
        //if want to implement the stressbar then must increase the stressbar more and more if the player is in the detection area of the bots
    }
}
