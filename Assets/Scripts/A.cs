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

    public GameObject healthBar1; // ù ��° HP �� ������Ʈ ����
    public GameObject healthBar2; // �� ��° HP �� ������Ʈ ����

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

        // HP �� ��ġ ������Ʈ
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

        // �� ���� HP �� ����
        if (healthBar1 != null)
        {
            Destroy(healthBar1);
        }

        if (healthBar2 != null)
        {
            Destroy(healthBar2);
        }

        // ���� ������Ʈ ����
        Destroy(gameObject, 1.5f); // �״� �ִϸ��̼� �� ���� ������Ʈ ����
    }

    // HP �� ��ġ ������Ʈ
    void UpdateHealthBarPosition()
    {
        if (healthBar1 != null)
        {
            // HP �ٸ� ������ ��ġ�� ���� ������Ʈ
            healthBar1.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        }

        if (healthBar2 != null)
        {
            // �� ��° HP �ٵ� �����ϰ� ��ġ ������Ʈ
            healthBar2.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        }
    }
}
