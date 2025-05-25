//Shutdown the light after collision script developped by Iris

using UnityEngine;

public class ShutDownLight : MonoBehaviour
{
    public GameObject BotCollider; //Collider of the corresponding Bot
    public GameObject Light; //The Spot Light of the corresponding Bot
    public AudioSource switchSound; //Sound of switch the light
    private bool BotColliderisActive; //If the Bot Box collider is active or not

    //Update is called once per frame
    void Update()
    {
        //Each frame check if the collider is active or not
        BotColliderisActive = BotCollider.activeSelf;
    }

    void OnTriggerEnter(Collider other)
    {
        //If the ball enter in the collider of the switch then desactivate the spot light and the box collider of the bot
        if (other.CompareTag("Light_switch") && BotColliderisActive)
        {
            Light.SetActive(false);
            BotCollider.SetActive(false);
            switchSound.Play();
        }
    }
}
