using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rbody;
    Animator animator;

    public float speed = 5f;            // �̵��ӵ�
    public float jumpForce = 5f;        // ���� ��
    public float MoveX;
    private int jumpCnt = 0;            // ���� Ƚ��
    private bool isGrounded = true;     // ���ٴ� ����

    public static string GameState = "Playing";

    public string IdleAnime = "idle";
    public string WalkAnime = "run";
    public string JumpAnime = "jump";
    public string GoalAnime = "idle";
    public string DeadAnime = "hurt";

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

        // ���� �Է� ó��
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || jumpCnt < 2))
        {
            Jump();
        }

        // �ִϸ��̼� ������Ʈ
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
    }

    void Jump()
    {
        rbody.velocity = Vector2.zero; // ���� �� �ӵ� �ʱ�ȭ
        rbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        PlayAnimation(JumpAnime);


        if (MoveX != 0.0f)
        {
            transform.localScale = new Vector3(MoveX > 0 ? 1 : -1, 1);
        }
            // ���� �� ���� ������Ʈ
            jumpCnt++;                    // ���� Ƚ�� ����
        isGrounded = false;           // ���߿� ���� ���� ���� ���� ���� ����
    }

    void PlayAnimation(string animationName)
    {
        if (animator == null) return;  // Animator�� ������ �������� ����

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
            jumpCnt = 0; // ���� ������ ���� Ƚ�� �ʱ�ȭ
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(ReduceGravityTemporarily());
            isGrounded = true;
            jumpCnt = 0; // ���� ������ ���� Ƚ�� �ʱ�ȭ
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rbody.gravityScale = 1.0f;
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

        GetComponent<CapsuleCollider2D>().enabled = false;          // �浹 ����
        rbody.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);     // ���� ��¦ Ʀ
    }

    public void GameStop()
    {
        rbody.velocity = Vector2.zero;
    }

    IEnumerator ReduceGravityTemporarily()
    {
        float originalGravity = rbody.gravityScale; // ���� �׷���Ƽ ������ �� ����
        rbody.gravityScale = 0.1f; // �׷���Ƽ�� ������ ����
        rbody.velocity = Vector2.zero;

        // ���ϴ� �ð���ŭ �Ͻ������� �׷���Ƽ�� ����Ӵϴ�
        yield return new WaitForSeconds(100f);

        // �ð��� ������ ���� ������ ����
        rbody.gravityScale = originalGravity;
    }
}
