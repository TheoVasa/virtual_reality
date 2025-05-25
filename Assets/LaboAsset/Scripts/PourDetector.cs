//Pour detection script developped by Iris

using UnityEngine;
using UnityEngine.VFX;

public class PourDetector : MonoBehaviour
{
    public GameObject waterStream; //WaterAnimator object
    public Renderer targetRenderer; //Mesh Renderer of the water inside becher
    public float pourAngleThreshold = 90f; //The threshold angle to start the water stream
    public Transform topPosition; //Is the location of the top of the becher
    public VisualEffect vfx_water_stream; //VFX graph of the waterstream use to change the color
    public AudioSource waterPourSound; //Sound of Water Pour

    private Color color; //Color of the current erlenmeyer

    void Start()
    {
        //Obtain the position of the waterSteam object
        waterStream.transform.position = topPosition.position;
    }

    //Update is called once per frame
    private void Update()
    {
        //Obtain the angle of the current erlenmeyer
        float xAngle = NormalizeAngle(transform.rotation.eulerAngles.x); //x angle
        float zAngle = NormalizeAngle(transform.rotation.eulerAngles.z); //z angle

        //Make the waterstream always falling perpedicular to the floor: strangely Vector3 must be up
        waterStream.transform.up = Vector3.up;

        //The boolean of detection of cross threshold: if xAngle is more than threshold or zAngle is more than threshold then true else false.
        bool isCrossThreshold = Mathf.Abs(xAngle) > pourAngleThreshold || Mathf.Abs(zAngle) > pourAngleThreshold;

        //If cross the threshold then activate the waterStream animation
        if (isCrossThreshold)
        {
            //Activate the waterStream animation
            if (!waterStream.activeSelf)
            {
                waterStream.SetActive(true);
            }

            //Have for origin the point the highest among the different origin points
            waterStream.transform.position = topPosition.position;

            if (!waterPourSound.isPlaying)
            {
                waterPourSound.Play();
            }
        }
        else
        {
            //Desactivate the waterStream Animation
            waterStream.SetActive(false);

            if (waterPourSound.isPlaying)
            {
                waterPourSound.Stop();
            }
        }

        //Change the color of the waterstream with the current color of the erlenmyer
        color = targetRenderer.material.GetColor("_SideColor");
        vfx_water_stream.SetVector4("Color", color);

    }

    //Normalize the Angle (this code was found online)
    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}