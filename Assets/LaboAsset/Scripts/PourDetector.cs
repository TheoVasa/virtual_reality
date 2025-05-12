using UnityEngine;
using UnityEngine.VFX;

public class PourDetector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public GameObject waterStream; //the WaterAnimator object
    public Renderer targetRenderer; //Mesh Renderer of the water inside becher

    public float pourAngleThreshold = 90f; //the threshold angle to start the water stream
    public Transform topPosition; //Is the location of the top of the becher
    public VisualEffect vfx_water_stream; //VFX graph of the waterstream use to change the color

    private Color color; //Color of the current erlenmeyer

    void Start()
    {
        //waterStream.SetActive(false);
        waterStream.transform.position = topPosition.position;
    }

    // Update is called once per frame
    private void Update()
    {
        float xAngle = NormalizeAngle(transform.rotation.eulerAngles.x); //x angle
        float zAngle = NormalizeAngle(transform.rotation.eulerAngles.z); //z angle

        waterStream.transform.up = Vector3.up; //Make the waterstream always falling perpedicular to the floor: strangely Vector3 must be up I DONT UNDERSTAND TOCHECK

        bool isCrossThreshold = Mathf.Abs(xAngle) > pourAngleThreshold || Mathf.Abs(zAngle) > pourAngleThreshold; //the boolean of detection of cross threshold

        if (isCrossThreshold)
        {

            if (!waterStream.activeSelf)
            {
                waterStream.SetActive(true);
            }

            waterStream.transform.position = topPosition.position; //have for origin the point the highest among the different origin points
        }
        else
        {
            waterStream.SetActive(false);
        }

        //Change the color of the waterstream with the current color of the erlenmyer
        color = targetRenderer.material.GetColor("_SideColor");
        vfx_water_stream.SetVector4("Color", color);

    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}