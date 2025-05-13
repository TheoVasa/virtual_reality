using UnityEngine;

public class GlassButtoncorrosion : MonoBehaviour
{
    public GameObject Glass; //Glass of the box
    private bool waterStreamisActive = false; //check if waterstream is active for the other object
    private bool isTrigger = false; //check that the box collider is trigger with the erlenmeyer
    private Color otherTopColor;
    private Color otherSideColor; //parameter of colors of the final becher (collider)



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger && waterStreamisActive)
        {
            print("Yes");
            if (otherSideColor == new Color(0.0f, 0.35f, 0.2f, 1.0f) && otherTopColor == new Color(0.0f, 0.35f, 0.2f, 1.0f))
            {
                print("Good");
                Glass.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WaterStream"))
        {
            print("WaterStream find in glass");
            isTrigger = true;

            Transform water = other.transform.parent;

            Debug.Log("Parent of other is: " + water.gameObject.name);

            Transform otherBecher = water.transform.parent;
            Debug.Log("Parent of water must be Erlemeyer Red: " + otherBecher.gameObject.name);

            Transform initialBecherMaterialTransform = otherBecher.transform.Find("Erlenmeyer_flask3");
            Debug.Log("Child of Becher must be Erlenmeyer_flask3: " + initialBecherMaterialTransform.gameObject.name);

            if (initialBecherMaterialTransform != null)
            {
                print("Find material");
                // Try to get the Renderer from the parent
                Renderer initialBecherRenderer = initialBecherMaterialTransform.GetComponent<Renderer>();

                if (initialBecherRenderer != null)
                {
                    print("Find renderer");
                    otherSideColor = initialBecherRenderer.material.GetColor("_SideColor");
                    otherTopColor = initialBecherRenderer.material.GetColor("_TopColor");

                }
            }
            if (water != null) // check if the code find the WaterAnimator
            {
                print("WaterAnimator found");
                waterStreamisActive = water.gameObject.activeSelf;
            }
        }
    }
}
