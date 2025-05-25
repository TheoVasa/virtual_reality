using UnityEngine;

public class ScreenMaterialChanger : MonoBehaviour
{
    public MeshRenderer screenRenderer;
    public Material activeMaterial;
    public Material blackMaterial;

    public void SetActiveMaterial()
    {
        if (screenRenderer && activeMaterial)
            screenRenderer.material = activeMaterial;
    }

    public void SetBlackMaterial()
    {
        if (screenRenderer && blackMaterial)
            screenRenderer.material = blackMaterial;
    }
}
