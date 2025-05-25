using UnityEngine;
using NOT_Lonely;


public class CableFollow : MonoBehaviour
{
    public ACC_Trail cableTrail;
    public bool isActive = false;


    void Update()
    {
        if (isActive && cableTrail != null)
        {
            cableTrail.UpdateCableTrail();
        }
    }
}