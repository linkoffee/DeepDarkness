using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;

    [SerializeField] private Collider2D attackCollider;

    [SerializeField] public int attackDamage = 1;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 1.1f;

    public event EventHandler OnEnemyAttack;
    public event EventHandler OnEnemyTakeDamage;
    public event EventHandler OnEnemyDeath;

    public bool IsAlive { get; private set; } = true;
    public bool IsAttack { get; private set; }
    public Transform PlayerTransform { get; private set; }

    public int CurrentHealth => _currentHealth;
    private int _currentHealth;

    private const float AttackAnimationDuration = 0.3f;

    private Collider2D _collider;

    private bool _canAttack = true;
    private bool _isPlayerInRange;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        DisableAttackCollider();
    }

    private void Start()
    {
        _currentHealth = maxHealth;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            PlayerTransform = player.transform;
    }

    private void Update()
    {
        if (!IsAlive)
            return;

        UpdatePlayerInRangeStatus();

        if (CanAttack())
            Attack();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void EnableAttackCollider() => attackCollider.enabled = true;
    public void DisableAttackCollider() => attackCollider.enabled = false;

    public void TakeDamage(int damage)
    {
        if (!IsAlive)
            return;

        _currentHealth -= damage;
        OnEnemyTakeDamage?.Invoke(this, EventArgs.Empty);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (!IsAlive)
            return;

        IsAlive = false;
        IsAttack = false;

        _collider.enabled = false;
        DisableAttackCollider();

        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
    }

    private void Attack()
    {
        if (!IsAlive || IsAttack || !_canAttack)
            return;

        StartCoroutine(PerformAttack());
    }

    private void UpdatePlayerInRangeStatus()
    {
        _isPlayerInRange = Vector2.Distance(transform.position, PlayerTransform.position) <= attackRange;
    }

    private bool CanAttack()
    {
        return _isPlayerInRange &&
               _canAttack &&
               Player.Instance.IsAlive && 
               !Player.Instance.IsWalking;
    }

    private IEnumerator PerformAttack()
    {
        IsAttack = true;
        _canAttack = false;

        OnEnemyAttack?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(AttackAnimationDuration);

        IsAttack = false;

        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }
}
