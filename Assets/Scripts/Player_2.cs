using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 3.0f;

    public float jump = 9.0f;
    public LayerMask groundLayer;
    bool onGround = false;
    private bool wasOnGround = false;

    private int maxJumpCount = 2;
    private int currentJumpCount = 0;

    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "playing";

    public int score = 0;
    bool isMoving = false;

    public AudioClip itemPickupSound;
    public AudioClip jumpSound;
    private AudioSource audioSource;

    // ���� ������ ���� ����
    public Transform groundCheck;  // ĳ���� �� ��ġ
    public float groundCheckRadius = 0.2f;

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing";

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }

        if (!isMoving)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }

        if (axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        // ���� ��ư�� ���� �� ���� Ƚ�� �˻�
        if (Input.GetButtonDown("Jump") && currentJumpCount < maxJumpCount)
        {
            Jump();
        }
        if (onGround && !wasOnGround)
        {
            currentJumpCount = 0;
            Debug.Log("Landed: Current Jump Count reset to 0");  // ���� �� �ʱ�ȭ Ȯ��
        }

        // ���� ���� ���� ����
        wasOnGround = onGround;

    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }

        // ���� ������ OverlapCircle�� ����
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ���� ���� ��� (���� �� true�� ǥ��)
        Debug.Log("onGround: " + onGround);

        // ���� ���°� ����� ���� ���� Ƚ�� �ʱ�ȭ
        
        if (onGround || axisH != 0)
        {
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }

        if (onGround)
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime;
            }
            else
            {
                nowAnime = moveAnime;
            }
        }
        else
        {
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }
    }

    public void Jump()
    {
        Debug.Log("Jump Called");
        rbody.velocity = new Vector2(rbody.velocity.x, jump); // Y�� �ӵ� ����
        currentJumpCount++;
        Debug.Log("Current Jump Count: " + currentJumpCount); // ���� ī��Ʈ Ȯ��
        PlayJumpSound();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }
    }

    void PlayPickupSound()
    {
        if (itemPickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(itemPickupSound);
        }
    }

    void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop();
    }

    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = "gameover";
        GameStop();

        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    void GameStop()
    {
        rbody.velocity = Vector2.zero;
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        isMoving = axisH != 0;
    }
}
