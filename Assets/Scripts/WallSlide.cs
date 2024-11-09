using UnityEngine;

public class WallSlide : MonoBehaviour
{
    public float slidingSpeed = 0.5f; // 벽에서 미끄러져 내려오는 속도
    public LayerMask wallLayer;       // 벽의 레이어

    private Rigidbody2D rbody;
    private bool isWallSliding = false;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // 벽에 붙어 있는지 감지하고 미끄러짐 효과를 적용하는 함수
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

    // 미끄러지기 시작
    private void StartWallSlide()
    {
        isWallSliding = true;
        rbody.velocity = new Vector2(rbody.velocity.x, -slidingSpeed);
    }

    // 미끄러지기 중지
    private void StopWallSlide()
    {
        if (isWallSliding)
        {
            isWallSliding = false;
        }
    }

    // 바닥에 닿았는지 감지하는 함수 (예시로 간단히 구현, 필요시 수정)
    private bool IsGrounded()
    {
        return rbody.velocity.y == 0;
    }
}
