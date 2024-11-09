using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Move_test : MonoBehaviour
{
    Rigidbody2D rbody;
    Animator animator;

    public float speed = 5f;            // 이동속도
    public float jumpForce = 5f;        // 점프 힘
    public float MoveX;
    private int jumpCnt = 0;            // 점프 횟수
    private bool isGrounded = true;     // 땅바닥 감지

    public static string GameState = "Playing";

    public string IdleAnime = "Knight_Idle";
    public string WalkAnime = "Knight_walk";
    public string JumpAnime = "Knight_jump";
    public string GoalAnime = "Knight_clear";
    public string DeadAnime = "Knight_dead";

    string nowAnime = "";

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameState = "Playing";
        nowAnime = IdleAnime;
        animator.Play(IdleAnime, 0);
    }

    void Update()
    {
        if (GameState != "Playing")
        {
            return;
        }

        MoveX = Input.GetAxis("Horizontal");

        // 점프 입력 처리
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || jumpCnt < 2))
        {
            Jump();
        }

        // 애니메이션 업데이트
        if (MoveX != 0.0f)
        {
            transform.localScale = new Vector3(MoveX > 0 ? 1 : -1, 1);
            PlayAnimation(WalkAnime);
        }
        else
        {
            PlayAnimation(IdleAnime);
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(MoveX, 0) * speed * Time.fixedDeltaTime;
        transform.Translate(movement);
    }

    void Jump()
    {
        rbody.velocity = Vector2.zero; // 점프 시 속도 초기화
        rbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        PlayAnimation(JumpAnime);

        // 점프 후 상태 업데이트
        jumpCnt++;                    // 점프 횟수 증가
        isGrounded = false;           // 공중에 있을 때는 땅에 닿지 않은 상태
    }

    void PlayAnimation(string animationName)
    {
        if (animator == null) return;  // Animator가 없으면 실행하지 않음

        if (nowAnime != animationName)
        {
            nowAnime = animationName;
            animator.Play(animationName, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCnt = 0; // 땅에 닿으면 점프 횟수 초기화
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Clear"))
        {
            Goal();
        }
        else if (collision.CompareTag("Dead"))
        {
            Dead();
        }
    }

    public void Goal()
    {
        GameStop();
        GameState = "GameClear";
        PlayAnimation(GoalAnime);
    }

    public void Dead()
    {
        GameStop();
        GameState = "GameOver";
        PlayAnimation(DeadAnime);

        GetComponent<CapsuleCollider2D>().enabled = false;          // 충돌 무시
        rbody.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);     // 위로 살짝 튐
    }

    public void GameStop()
    {
        rbody.velocity = Vector2.zero;
    }
}
