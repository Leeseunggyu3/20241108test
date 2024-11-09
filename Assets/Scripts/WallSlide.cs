using UnityEngine;

public class WallSlide : MonoBehaviour
{
    public float slidingSpeed = 0.5f; // ������ �̲����� �������� �ӵ�
    public LayerMask wallLayer;       // ���� ���̾�

    private Rigidbody2D rbody;
    private bool isWallSliding = false;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // ���� �پ� �ִ��� �����ϰ� �̲����� ȿ���� �����ϴ� �Լ�
    public void CheckAndSlide(bool isWall)
    {
        if (isWall && !IsGrounded())
        {
            StartWallSlide();
        }
        else
        {
            StopWallSlide();
        }
    }

    // �̲������� ����
    private void StartWallSlide()
    {
        isWallSliding = true;
        rbody.velocity = new Vector2(rbody.velocity.x, -slidingSpeed);
    }

    // �̲������� ����
    private void StopWallSlide()
    {
        if (isWallSliding)
        {
            isWallSliding = false;
        }
    }

    // �ٴڿ� ��Ҵ��� �����ϴ� �Լ� (���÷� ������ ����, �ʿ�� ����)
    private bool IsGrounded()
    {
        return rbody.velocity.y == 0;
    }
}
