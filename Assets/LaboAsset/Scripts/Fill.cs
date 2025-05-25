//Acid preparation and potion mix script developped by Iris (modifié pour contourner OnTriggerExit)

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
}

public class Fill : MonoBehaviour
{
    //Public
    public Renderer targetRenderer; //Mesh Renderer of the water inside becher
    public GameObject smokeParticule; //Particule effect of black smoke (incorrect mix)
    public GameObject correctParticule; //Particules effect with the number and the green smoke
    public GameObject waterStream; //WaterStream gameObject
    public AudioSource smokeSound; //Smoke sound effect

    //Private
    //Fill
    private float fillParameter = 0f; //Parameter of the fill of the water (from 0 to 0.08)
    //Color
    private Color finalSideColor; //Parameter of the color of the water
    private Color finalTopColor; //Parameter of the color of the side of the water
    private Color otherSideColor; //Parameter of colors of the other becher (collider)
    private Color otherTopColor;
    private Color currentMeanSideColor; //Parameter to follow up the color of the erlenmeyer
    private Color currentMeanTopColor;

    //Dictionary of the authorized pair of color and giving the output color corresponding
    public static Dictionary<ColorPair, Color> colorMix = new Dictionary<ColorPair, Color>()
    {
        { new ColorPair(new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 0.0f, 0.0f, 1.0f)), new Color(1.0f, 0.0f, 0.0f, 1.0f) }, //white/transparent + red = red
        { new ColorPair(new Color(1.0f, 0.0f, 0.0f, 1.0f), new Color(1.0f, 1.0f, 0.0f, 1.0f)), new Color(1.0f, 0.5f, 0.0f, 1.0f) }, //red + yellow = orange
        { new ColorPair(new Color(1.0f, 0.5f, 0.0f,1.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f)), new Color(1.0f, 0.7f, 0.3f, 1.0f) }, //orange + white = light orange
        { new ColorPair(new Color(1.0f, 0.7f, 0.3f, 1.0f), new Color(0.0f, 0.0f, 1.0f, 1.0f)), new Color(0.0f, 1.0f, 0.5f, 1.0f) }, //light orange + blue = light green
        { new ColorPair(new Color(0.0f, 1.0f, 0.5f,1.0f), new Color(0.5f, 0.5f, 0.5f, 1.0f)), new Color(0.0f, 0.35f, 0.2f, 1.0f) }, //light green + grey = dark green
        //Mix of same color is ok
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

    private float timeChangeColor = 0f; //Initialization of the iteration timeChangeColor
    public float durationChangeColor = 10f; //Speed of the change of the color
    //Trigger Event booleans
    private bool isTrigger = false; //Check that the box collider is trigger with the erlenmeyer
    private bool isTriggerSink = false; //Check that the box collider is trigger with the Sink collider
    private bool waterStreamisActive = false; //Check if waterstream is active for the other object
    private bool smokeParticuleActive = false; //Check if the smoke is active

    private HashSet<Collider> currentTriggers = new HashSet<Collider>(); //Keep track of current trigger contacts

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
        // check if the collider box is triggered by anything
        Collider[] hits = Physics.OverlapSphere(transform.position, 5, default);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("WaterStream") || hit.CompareTag("Sink"))
            {
                // Add collider to currentTriggers if not already present
                if (!currentTriggers.Contains(hit))
                {
                    currentTriggers.Add(hit);
                }   
            }      
        }
        //When the collider box is trigger by the initial becher and that the initial becher waterstream is active start the filling of the final becher
        if (isTrigger && waterStreamisActive)
        {
            //Fill change
            Fillfunction();

            //Color change
            ChangeColor();
        }
        else
        {
            //If no more trigger
            UpdateColor();
        }

        //Cleanup the liquid to redo
        if (isTriggerSink && waterStream.activeSelf)
        {
            //Clean up erlenmeyer liquid and desactivate the smoke
            Cleanup();
        }

        //Activation of the black smoke if incorrect solution mix
        Blacksmoke();

        //Correct Solution green smoke appeared with a number
        GreenSmoke();
    }

    //LateUpdate to track loss of triggers manually
    void LateUpdate()
    {
        // Get all current colliders in range
        var stillPresent = new HashSet<Collider>(Physics.OverlapSphere(transform.position, 5, default));

        // Check for any collider that is no longer present
        foreach (var col in new List<Collider>(currentTriggers))
        {
            if (!stillPresent.Contains(col))
            {
                //Reinitialize the two boolean at the end of the contact with the other erlenmeyer
                if (col.CompareTag("WaterStream"))
                {
                    isTrigger = false;
                    waterStreamisActive = false;
                }

                //Reinitialize the boolean at the end of the contact with the sink
                if (col.CompareTag("Sink"))
                {
                    isTriggerSink = false;
                }

                currentTriggers.Remove(col);
            }
        }
    }

    //When objects are inside the trigger zone
    private void OnTriggerStay(Collider other)
    {
        //Erlenmeyer_final is in contact with an other erlenmeyer
        if (other.CompareTag("WaterStream"))
        {
            //Track collider contact
            currentTriggers.Add(other);

            //Trigger is true
            isTrigger = true;

            //Obtain the Erlenmeyer_flask3 of the other erlenmeyer
            Transform water = other.transform.parent;
            Transform otherBecher = water?.transform.parent;
            Transform initialBecherMaterialTransform = otherBecher?.Find("Erlenmeyer_flask3");

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

        //Erlenmeyer_final is in contact with the sink
        if (other.CompareTag("Sink"))
        {
            //Track collider contact
            currentTriggers.Add(other);

            //Trigger sink is true
            isTriggerSink = true;
        }
    }

    void Fillfunction()
    {
        //Increase the water Fill parameter of the final becher in function of time (limit is 0.08)
        if (fillParameter < 0.08f)
        {
            fillParameter += 0.005f * Time.deltaTime;
        }

        //Update the water Fill parameter of the final becher with the new value
        if (targetRenderer.material.HasProperty("_Fill"))
        {
            targetRenderer.material.SetFloat("_Fill", fillParameter);
        }
    }

    void ChangeColor()
    {
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

    void UpdateColor()
    {
        //If the new color is the mean color then update finalSideColor and finalTopColor (important for smoke effect)
        if (targetRenderer.material.GetColor("_SideColor") == currentMeanSideColor && targetRenderer.material.GetColor("_TopColor") == currentMeanTopColor)
        {
            finalSideColor = currentMeanSideColor;
            finalTopColor = currentMeanTopColor;
            timeChangeColor = 0;
        }
    }

    void Cleanup()
    {
        //Decrease the water Fill parameter of the final becher in function of time (limit is 0.08)
        if (fillParameter > 0.00f)
        {
            fillParameter -= 0.1f * Time.deltaTime;
        }
        else //If (fillParameter == 0.0f)
        {
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
        }
    }

    void Blacksmoke()
    {
        //Activation of the black smoke if incorrect solution mix

        if (smokeParticuleActive)
        {
            //Activate the smoke
            smokeParticule.SetActive(true);
            //Make the smoke emitted up
            smokeParticule.transform.up = Vector3.forward * (-1);
            //Activate smoke explosion sound
            smokeSound.Play();
        }
        else
        {
            //Desactivate the smoke
            smokeParticule.SetActive(false);
        }
    }

    void GreenSmoke()
    {
        //Correct Solution green smoke appeared with a number
        if (finalSideColor == new Color(0.0f, 0.35f, 0.2f, 1.0f) && finalTopColor == new Color(0.0f, 0.35f, 0.2f, 1.0f))
        {
            //Activate the smoke and the number particules
            correctParticule.SetActive(true);
            //Make the smoke emmitted up
            correctParticule.transform.up = Vector3.up;
        }
        else
        {
            //Turn off the particule effect
            correctParticule.SetActive(false);
        }
    }
}