//Glass corrosion script developped by Iris

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GlassButtoncorrosion : MonoBehaviour
{
    public GameObject Glass; //Glass of the box

    public GameObject buttonInteractable; //XR interaction of the button
    private bool waterStreamisActive = false; //Check if waterstream is active for the other object
    private bool isTrigger = false; //Check that the box collider is trigger with the erlenmeyer
    private Color otherTopColor;
    private Color otherSideColor; //Parameter of colors of the final becher (collider)



    //Update is called once per frame
    void Update()
    {
        if (isTrigger && waterStreamisActive)
        {
            //Check if the erlenmeyer in contact as the right color (acid <-> dark green)
            if (otherSideColor == new Color(0.0f, 0.35f, 0.2f, 1.0f) && otherTopColor == new Color(0.0f, 0.35f, 0.2f, 1.0f))
            {
                //Desactivate the Glass (allowing access to door button)
                Glass.SetActive(false);
                //activate the XR interaction
                buttonInteractable.GetComponent<XRSimpleInteractable>().enabled = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WaterStream"))
        {
            //Trigger is true
            isTrigger = true;

            //Obtain the Erlenmeyer_flask3 of the other erlenmeyer
            Transform water = other.transform.parent;
            Transform otherBecher = water.transform.parent;
            Transform initialBecherMaterialTransform = otherBecher.transform.Find("Erlenmeyer_flask3");

            //Obtain the color of the other erlenmeyer
            if (initialBecherMaterialTransform != null)
            {
                //Obtain the Renderer of the other erlenmeyer
                Renderer initialBecherRenderer = initialBecherMaterialTransform.GetComponent<Renderer>();

                //Obtain the color of the other erlenmeyer
                if (initialBecherRenderer != null)
                {
                    otherSideColor = initialBecherRenderer.material.GetColor("_SideColor");
                    otherTopColor = initialBecherRenderer.material.GetColor("_TopColor");

                }
            }
            //Find out if WaterStream of the other erlenmeyer is active (more than 90° rotation)
            if (water != null)
            {
                waterStreamisActive = water.gameObject.activeSelf;
            }
        }
    }
}
