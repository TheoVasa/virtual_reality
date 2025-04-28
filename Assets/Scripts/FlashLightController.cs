using UnityEngine;
using UnityEngine.InputSystem;

public class FlashLightController : MonoBehaviour
{
    public Light flashLight;
    private bool isFlashLightOn = false;
    private bool isFlashLightHold = false; 



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get components
        flashLight = GetComponentInChildren<Light>();
    }

    private void LightOn()
    {
        //turn on the light
        flashLight.enabled = true;
        isFlashLightOn = true;
    }
    private void LightOff()
    {
        //turn off the light
        flashLight.enabled = false;
        isFlashLightOn = false;
    }

    public void HoldHandlerLight()
    {
        //hold the light
        if (isFlashLightHold)
        {
            isFlashLightHold = false;
        }
        else
        {
            isFlashLightHold = true;
        }
    }

    public void ToggleFlashLight()
    {
        //toggle the light
        if (isFlashLightOn && isFlashLightHold)
        {
            LightOff();
        }
        else if (!isFlashLightOn && isFlashLightHold)
        {
            LightOn();
        }
    }
}
