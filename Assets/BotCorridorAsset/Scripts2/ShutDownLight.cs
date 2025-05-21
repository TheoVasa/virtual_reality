using UnityEngine;

public class ShutDownLight : MonoBehaviour
{
    public GameObject BotCollider; //Collider of the corresponding Bot
    public GameObject Light; //The Spot Light of the corresponding Bot
    private bool BotColliderisActive; //If the Bot Box collider is active or not

    // Update is called once per frame
    void Update()
    {
        BotColliderisActive = BotCollider.activeSelf; //Each frame check if the collider is active or not
    }

    void OnTriggerEnter(Collider other)
    {
        //if the ball enter in the collider of the switch then desactivate the spot light and the box collider of the bot
        if (other.CompareTag("Light_switch") && BotColliderisActive)
        {
            print("Touch the switch");
            print("Desactivate the light and the detection area of the bot");
            Light.SetActive(false);
            BotCollider.SetActive(false);
        }
    }
}
