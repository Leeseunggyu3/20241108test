using UnityEngine;

public class HPBar_Boss : MonoBehaviour
{
    public Transform boss; // 보스의 Transform
    public Vector3 offset = new Vector3(0, 1.1f, 0); // 보스 위에 HP 바 위치 조정

    void Update()
    {
        if (boss != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(boss.position + offset);
        }
    }
}
