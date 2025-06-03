using UnityEngine;

public class CursorLogick : MonoBehaviour
{
    public Transform target; // ������� ������ (���������� � ����������)

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
            // ������������ ����������� � ����
            Vector2 direction = target.position - transform.position;

            // ��������� ���� �������� � ��������
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // ��������� ������� (� ���������� ��� �������)
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
    }

    public void ShowCursor()
    {
        flag = true;
    }
}
