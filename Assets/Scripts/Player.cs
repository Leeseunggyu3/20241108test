using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rbody;
    Animator animator;

    public float speed = 5f;          //ÀÌµ¿¼Óµµ
    public float jumpForce = 5f;      //Á¡ÇÁ Èû
    public float MoveX, MoveY;
    private bool isGrounded = true;   //¶¥¹Ù´Ú°¨Áö

    public static string GameState = "Playing";

    public string IdleAnime = "Knight_Idle";
    public string WalkAnime = "Knight_walk";
    public string JumpAnime = "Knight_jump";
    public string GoalAnime = "Knight_clear";
    public string DeadAnime = "Knight_dead";

    string nowAnime = "";
    string oldAnime = "";

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameState = "Playing";
        nowAnime = IdleAnime;
        oldAnime = IdleAnime;
        animator.Play(IdleAnime, 0);
    }

    void Update()
    {
        if (GameState != "Playing")
        {
            return;
        }

        MoveX = Input.GetAxis("Horizontal");
        MoveY = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        }

        if (MoveX != 0.0f || MoveY != 0.0f)
        {
            if (MoveX > 0.0f)
            {
                transform.localScale = new Vector3(1, 1);
                PlayAnimation(WalkAnime);
            }
            else if (MoveX < 0.0f)
            {
                transform.localScale = new Vector3(-1, 1);
                PlayAnimation(WalkAnime);
            }
        }
        else
        {
            PlayAnimation(IdleAnime);
        }

        Vector2 movement = new Vector2(MoveX, MoveY) * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    void Jump()
    {
        rbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        PlayAnimation(JumpAnime);
        isGrounded = false;
    }

    void PlayAnimation(string animationName)
    {
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

        GetComponent<CapsuleCollider2D>().enabled = false;          //Ãæµ¹¹«½Ã
        rbody.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);     //À§·Î »ìÂ¦ Æ¦
    }

    public void GameStop()
    {
        rbody.velocity = Vector2.zero;
    }
}