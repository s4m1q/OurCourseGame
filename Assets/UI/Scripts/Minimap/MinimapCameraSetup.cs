using UnityEngine;

public class MinimapCameraSetup : MonoBehaviour
{
    public Camera minimapCamera;

    void Start()
    {
        // Включаем только нужные слои, используя побитовую маску
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int groundLayer = LayerMask.NameToLayer("Default");

        minimapCamera.cullingMask = (1 << playerLayer) | (1 << enemyLayer) | (1 << groundLayer);
    }
}
