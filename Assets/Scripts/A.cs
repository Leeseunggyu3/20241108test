using UnityEngine;
using System.Collections;

public class A : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1f;
    public int maxHealth = 100;
    private int currentHealth;

    public Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isAttacking = false;

    public GameObject healthBar1; // 첫 번째 HP 바 오브젝트 참조
    public GameObject healthBar2; // 두 번째 HP 바 오브젝트 참조

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
        else
        {
            MoveTowardsPlayer();
        }

        // HP 바 위치 업데이트
        UpdateHealthBarPosition();
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        animator.SetBool("isMoving", true);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");

        // 두 개의 HP 바 제거
        if (healthBar1 != null)
        {
            Destroy(healthBar1);
        }

        if (healthBar2 != null)
        {
            Destroy(healthBar2);
        }

        // 보스 오브젝트 제거
        Destroy(gameObject, 1.5f); // 죽는 애니메이션 후 보스 오브젝트 제거
    }

    // HP 바 위치 업데이트
    void UpdateHealthBarPosition()
    {
        if (healthBar1 != null)
        {
            // HP 바를 보스의 위치에 맞춰 업데이트
            healthBar1.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        }

        if (healthBar2 != null)
        {
            // 두 번째 HP 바도 동일하게 위치 업데이트
            healthBar2.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        }
    }
}
