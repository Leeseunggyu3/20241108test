using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rbody;
    Animator animator;

    public float speed = 5f;            // 이동속도
    public float jumpForce = 5f;        // 점프 힘
    public float MoveX;
    private int jumpCnt = 0;            // 점프 횟수
    private bool isGrounded = true;     // 땅바닥 감지

    public static string GameState = "Playing";

    public string IdleAnime = "idle";
    public string WalkAnime = "run";
    public string JumpAnime = "jump";
    public string GoalAnime = "idle";
    public string DeadAnime = "hurt";

    public float isRight = 1; //1이면 우측방향, -1이면 좌측방향


    public Transform wallChk;
    public float wallchkDistance;
    public LayerMask w_Layer;
    public float SlidingSpeed;





    bool isWall;







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








       isWall = Physics2D.Raycast(wallChk.position, Vector2.right*isRight, wallchkDistance, w_Layer);
        animator.SetBool("", isWall);
            





        if (GameState != "Playing")
        {
            return;
        }

        MoveX = Input.GetAxis("Horizontal");


        if(MoveX > 0)
        {
            isRight = 1;
        }
        else if (MoveX < 0)
        {
            isRight = -1;
        }


        // 점프 입력 처리
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || jumpCnt < 2))
        {
            Jump();
        }

        // 애니메이션 업데이트
        if (isGrounded)
        {
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
        else
        {
            PlayAnimation(JumpAnime);
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(MoveX, 0) * speed * Time.fixedDeltaTime;
        transform.Translate(movement);





        if (isWall)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y*SlidingSpeed);

        }

    }

    void Jump()
    {
        rbody.velocity = Vector2.zero; // 점프 시 속도 초기화
        rbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        PlayAnimation(JumpAnime);


        if (MoveX != 0.0f)
        {
            transform.localScale = new Vector3(MoveX > 0 ? 1 : -1, 1);
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
    }


}
