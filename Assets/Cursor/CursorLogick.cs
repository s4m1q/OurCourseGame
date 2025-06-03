using UnityEngine;

public class CursorLogick : MonoBehaviour
{
    public Transform target; // Целевой объект (перетащите в инспекторе)

    private SpriteRenderer sprite;

    bool flag = false;
    bool oneTime = true;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    void Update()
    {
        if (flag && oneTime)
        {
            sprite.enabled = true;
            oneTime = false;
        }
        if (target != null)
        {
            // Рассчитываем направление к цели
            Vector2 direction = target.position - transform.position;

            // Вычисляем угол поворота в градусах
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Применяем поворот (с коррекцией для спрайта)
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
    }

    public void ShowCursor()
    {
        flag = true;
    }
}
