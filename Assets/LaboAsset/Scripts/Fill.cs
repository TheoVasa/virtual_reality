using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public struct ColorPair
{
    public Color Color1;
    public Color Color2;
    public ColorPair(Color a, Color b)
    {
        Color1 = a;
        Color2 = b;
    }

    public bool Equals(ColorPair other)
    {
        return Color1 == other.Color1 && Color2 == other.Color2;
    }

    public int GetHashCode()
    {
        return HashCode.Combine(Color1, Color2);
    }
}


public class Fill : MonoBehaviour
{
    //Public
    public Renderer targetRenderer; //Mesh Renderer of the water inside becher
    public GameObject smokeParticule; //Particule effect of black smoke (incorrect mix)
    public GameObject correctParticule; //Particules effect with the number and the green smoke
    public GameObject waterStream; //WaterStream gameObject

    //Private
    //Fill
    private float fillParameter = 0f; //parameter of the fill of the water (from 0 to 0.08)
    //Color
    private Color finalSideColor; //parameter of the color of the water
    private Color finalTopColor; //parameter of the color of the side of the water
    private Color otherSideColor; //parameter of colors of the other becher (collider)
    private Color otherTopColor;
    private Color currentMeanSideColor;
    private Color currentMeanTopColor;
    public static Dictionary<ColorPair, Color> colorMix = new Dictionary<ColorPair, Color>()
    {
        { new ColorPair(new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 0.0f, 0.0f, 1.0f)), new Color(1.0f, 0.0f, 0.0f, 1.0f) }, //white/transparent + red = red
        { new ColorPair(new Color(1.0f, 0.0f, 0.0f, 1.0f), new Color(1.0f, 1.0f, 0.0f, 1.0f)), new Color(1.0f, 0.5f, 0.0f, 1.0f) }, //red + yellow = orange
        { new ColorPair(new Color(1.0f, 0.5f, 0.0f,1.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f)), new Color(1.0f, 0.7f, 0.3f, 1.0f) }, //orange + white = light orange
        { new ColorPair(new Color(1.0f, 0.7f, 0.3f, 1.0f), new Color(0.0f, 0.0f, 1.0f, 1.0f)), new Color(0.0f, 1.0f, 0.5f, 1.0f) }, //light orange + blue = light green
        { new ColorPair(new Color(0.0f, 1.0f, 0.5f,1.0f), new Color(0.5f, 0.5f, 0.5f, 1.0f)), new Color(0.0f, 0.35f, 0.2f, 1.0f) }, //light green + grey = dark green
        //Mx of same color is ok
        { new ColorPair(new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f)), new Color(1.0f, 1.0f, 1.0f, 0.0f) }, //white/transparent + transparent = transparent
        { new ColorPair(new Color(1.0f, 0.0f, 0.0f, 1.0f), new Color(1.0f, 0.0f, 0.0f, 1.0f)), new Color(1.0f, 0.0f, 0.0f, 1.0f) }, //red + red = red
        { new ColorPair(new Color(1.0f, 1.0f, 0.0f, 1.0f), new Color(1.0f, 1.0f, 0.0f, 1.0f)), new Color(1.0f, 1.0f, 0.0f, 1.0f) }, //yellow + yellow = yellow
        { new ColorPair(new Color(1.0f, 0.5f, 0.0f,1.0f), new Color(1.0f, 0.5f, 0.0f,1.0f)), new Color(1.0f, 0.5f, 0.0f,1.0f) }, //orange + orange = orange
        { new ColorPair(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f)), new Color(1.0f, 1.0f, 1.0f, 1.0f) }, //white + white = white
        { new ColorPair(new Color(1.0f, 0.7f, 0.3f, 1.0f), new Color(1.0f, 0.7f, 0.3f, 1.0f)), new Color(1.0f, 0.7f, 0.3f, 1.0f) }, //light orange + light orange = light orange
        { new ColorPair(new Color(0.0f, 0.0f, 1.0f, 1.0f), new Color(0.0f, 0.0f, 1.0f, 1.0f)), new Color(0.0f, 0.0f, 1.0f, 1.0f) }, //blue + blue = blue
        { new ColorPair(new Color(0.0f, 1.0f, 0.5f,1.0f), new Color(0.0f, 1.0f, 0.5f,1.0f)), new Color(0.0f, 1.0f, 0.5f,1.0f) }, //light green + light green = light green
        { new ColorPair(new Color(0.5f, 0.5f, 0.5f, 1.0f), new Color(0.5f, 0.5f, 0.5f, 1.0f)), new Color(0.5f, 0.5f, 0.5f, 1.0f) }, //grey + grey = grey
        { new ColorPair(new Color(0.0f, 0.35f, 0.2f, 1.0f), new Color(0.0f, 0.35f, 0.2f, 1.0f)), new Color(0.0f, 0.35f, 0.2f, 1.0f) }, //dark green + dark green = dark green
        //Continuity if stop trigger and redo
        { new ColorPair(new Color(1.0f, 0.5f, 0.0f,1.0f), new Color(1.0f, 1.0f, 0.0f, 1.0f)), new Color(1.0f, 0.5f, 0.0f,1.0f) }, //orange + jaune = orange
        { new ColorPair(new Color(1.0f, 0.7f, 0.3f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f)), new Color(1.0f, 0.7f, 0.3f, 1.0f) }, //light orange + white = light orange
        { new ColorPair(new Color(0.0f, 1.0f, 0.5f,1.0f), new Color(0.0f, 0.0f, 1.0f, 1.0f)), new Color(0.0f, 1.0f, 0.5f,1.0f) }, //light green + blue = light green
        { new ColorPair(new Color(0.0f, 0.35f, 0.2f, 1.0f), new Color(0.5f, 0.5f, 0.5f, 1.0f)), new Color(0.0f, 0.35f, 0.2f, 1.0f) } //dark green + grey = dark green
    };

    private float timeChangeColor = 0f;
    public float durationChangeColor = 5f; //speed of the change of the color
    //Trigger Event booleans
    private bool isTrigger = false; //check that the box collider is trigger with the erlenmeyer
    private bool isTriggerSink = false; //check that the box collider is trigger with the Sink collider
    private bool waterStreamisActive = false; //check if waterstream is active for the other object
    private bool smokeParticuleActive = false; //xheck if the smoke is active

    //Start
    void Start()
    {
        //Obtain the value of the TopColor and of the SideColor of the final becher
        finalTopColor = targetRenderer.material.GetColor("_TopColor");
        finalSideColor = targetRenderer.material.GetColor("_SideColor");
    }

    //Update is called once per frame
    void Update()
    {
        //Check the Renderer
        if (targetRenderer != null)
        {
            Debug.Log("Target Renderer is attached to: " + targetRenderer.gameObject.name);
        }

        //When the collider box is trigger by the initial becher and that the initial becher waterstream is active start the filling of the final becher
        if (isTrigger && waterStreamisActive)
        {
            //Fill change

            //Increase the water Fill parameter of the final becher in function of time (limit is 0.08)
            if (fillParameter < 0.08f)
            {
                fillParameter += 0.005f * Time.deltaTime;
            }

            //Update the water Fill parameter of the final becher with the new value
            if (targetRenderer.material.HasProperty("_Fill"))
            {
                targetRenderer.material.SetFloat("_Fill", fillParameter);
                //Debug.Log("Fill parameter set to: " + fillParameter);
            }
            else
            {
                Debug.LogError("The material does not have a '_Fill' property.");
            }

            //Color change
            var colorSidePair = new ColorPair(finalSideColor, otherSideColor);
            var colorTopPair = new ColorPair(finalTopColor, otherTopColor);

            //Check if the color pair is possible (present in the dictionary)
            if (colorMix.TryGetValue(colorSidePair, out Color meanSideColor) && colorMix.TryGetValue(colorTopPair, out Color meanTopColor))
            {
                //Obtain the new color
                currentMeanSideColor = meanSideColor;
                currentMeanTopColor = meanTopColor;

                //Change the color gradually
                if (timeChangeColor < durationChangeColor)
                {
                    print("Good color");
                    timeChangeColor += Time.deltaTime;
                    float time = timeChangeColor / durationChangeColor;

                    //Change the color from the original colors of the final becher to the means color of the final and initial bechers
                    Color newFinalSideColor = Color.Lerp(finalSideColor, meanSideColor, time);
                    Color newFinalTopColor = Color.Lerp(finalTopColor, meanTopColor, time);

                    //Update the new colors to the colors parameter of the final becher
                    targetRenderer.material.SetColor("_SideColor", newFinalSideColor);
                    targetRenderer.material.SetColor("_TopColor", newFinalTopColor);
                }
            }
            else
            {
                print("Wrong color");
                //New color is black
                Color newFinalSideColor = new Color(0.0f, 0.0f, 0.0f);
                Color newFinalTopColor = new Color(0.0f, 0.0f, 0.0f);

                //Update the new colors to the colors parameter of the final becher
                targetRenderer.material.SetColor("_SideColor", newFinalSideColor);
                targetRenderer.material.SetColor("_TopColor", newFinalTopColor);

                //Update finalSideColor and finalTopColor to activate the smoke later
                finalSideColor = newFinalSideColor;
                finalTopColor = newFinalTopColor;

                //Change the boolean of the black smoke to true
                smokeParticuleActive = true;
            }
        }
        else
        {
            //If no more trigger
            //If the new color is the mean color then update finalSideColor and finalTopColor (important for smoke effect)
            if (targetRenderer.material.GetColor("_SideColor") == currentMeanSideColor && targetRenderer.material.GetColor("_TopColor") == currentMeanTopColor)
            {
                print("Equal to end");
                finalSideColor = currentMeanSideColor;
                finalTopColor = currentMeanTopColor;
                timeChangeColor = 0;
            }

            print("Don't detect");
        }

        if (waterStreamisActive)
        {
            print("water active");
        }

        //Trash the liquid to redo
        if (isTriggerSink && waterStream.activeSelf)
        {
            print("Enter Trash");
            //Vide l'eau et inactive la smoke
            //Decrease the water Fill parameter of the final becher in function of time (limit is 0.08)
            if (fillParameter > 0.00f)
            {
                print("Vide le liquide");
                fillParameter -= 0.1f * Time.deltaTime;
            }
            else//if (fillParameter == 0.0f)
            {
                print("Finish to trash");
                smokeParticuleActive = false;
                //Change the color to the white/transparent original color
                targetRenderer.material.SetColor("_SideColor", new Color(1.0f, 1.0f, 1.0f, 0.0f));
                targetRenderer.material.SetColor("_TopColor", new Color(1.0f, 1.0f, 1.0f, 0.0f));

                //Reset the finalSideColor and finalTopColor to transparent
                finalSideColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                finalTopColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }

            //Update the water Fill parameter of the final becher with the new value
            if (targetRenderer.material.HasProperty("_Fill"))
            {
                targetRenderer.material.SetFloat("_Fill", fillParameter);
                //Debug.Log("Fill parameter set to: " + fillParameter);
            }
            else
            {
                Debug.LogError("The material does not have a '_Fill' property.");
            }

        }

        //Activation of the black smoke if incorrect solution mix
        if (smokeParticuleActive)
        {
            print("smoke active");
            smokeParticule.SetActive(true); //Activate the smoke
            smokeParticule.transform.up = Vector3.forward * (-1); //Make the smoke emitted up
        }
        else
        {
            print("smoke desactivate");
            smokeParticule.SetActive(false); //Desactivate the smoke
        }

        //Correct Solution green smoke appeared with a number
        if (finalSideColor == new Color(0.0f, 0.35f, 0.2f, 1.0f) && finalTopColor == new Color(0.0f, 0.35f, 0.2f, 1.0f))
        {
            print("Correct mix");
            correctParticule.SetActive(true); //Activate the smoke and the number particules
            correctParticule.transform.up = Vector3.up; //Make the smoke emmitted up
        }
        else
        {
            correctParticule.SetActive(false); //turn off the particule effect
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WaterStream"))
        {
            print("Trigger");
            isTrigger = true;

            Transform water = other.transform.parent; //root.Find("WaterAnimator");

            Debug.Log("Parent of other is: " + water.gameObject.name); //TO CHECK that we assign correctly to WaterAnimator

            Transform otherBecher = water.transform.parent;
            Debug.Log("Parent of water must be Erlemeyer Red: " + otherBecher.gameObject.name); //TO CHECK that we assign correctly to WaterAnimator

            Transform initialBecherMaterialTransform = otherBecher.transform.Find("Erlenmeyer_flask3"); //other.transform.root.Find("Erlenmeyer_flask3");//Transform inialBecherTransform = water.transform.parent; //Initial Becher
            Debug.Log("Child of Becher must be Erlenmeyer_flask3: " + initialBecherMaterialTransform.gameObject.name);

            if (initialBecherMaterialTransform != null)
            {
                // Try to get the Renderer from the parent
                Renderer initialBecherRenderer = initialBecherMaterialTransform.GetComponent<Renderer>();

                if (initialBecherRenderer != null)
                {
                    otherSideColor = initialBecherRenderer.material.GetColor("_SideColor");
                    Debug.Log("Parent's material side color: " + otherSideColor);

                    otherTopColor = initialBecherRenderer.material.GetColor("_TopColor");
                    Debug.Log("Parent's material top color: " + otherTopColor);

                }
            }

            if (water != null) // check if the code find the WaterAnimator
            {
                print("WaterAnimator found");
                waterStreamisActive = water.gameObject.activeSelf;
            }
            else
            {
                print("WaterAnimator not found");
            }
        }

        if (other.CompareTag("Sink"))
        {
            print("Trigger Sink Trash");
            isTriggerSink = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterStream"))
        {
            print("End");
            isTrigger = false;
            waterStreamisActive = false;
        }
    }

}
