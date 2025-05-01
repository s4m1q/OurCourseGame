using UnityEngine;
public class MinimapCameraFollow : MonoBehaviour
{
    public Transform target; // Игрок
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(0, 0, 0); // Не поворачиваем камеру
    }
}
