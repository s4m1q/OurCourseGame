using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Ссылка на объект, за которым должна следовать камера
    public Transform target;

    // Смещение камеры относительно цели
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        // Если цель существует, перемещаем камеру
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}