using UnityEngine;

public class HPBar_Boss : MonoBehaviour
{
    public Transform boss; // ������ Transform
    public Vector3 offset = new Vector3(0, 1.1f, 0); // ���� ���� HP �� ��ġ ����

    void Update()
    {
        if (boss != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(boss.position + offset);
        }
    }
}
