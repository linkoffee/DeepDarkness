using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemySO enemyData;
    [SerializeField] private Collider2D attackCollider;

    [SerializeField] private float attackRange = 1.1f;

    [SerializeField] private string takeDamageSfx;
    [SerializeField] private string dieSfx;

    private const float AttackCooldown = 2f;

    public EnemySO EnemyData => enemyData;

    public event Action OnEnemyAttack;
    public event Action OnEnemyTakeDamage;
    public event Action OnEnemyDeath;

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
        _currentHealth = enemyData.health;

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

        OnEnemyTakeDamage?.Invoke();
        SfxManager.Instance.PlaySound2D(takeDamageSfx);

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

        OnEnemyDeath?.Invoke();
        SfxManager.Instance.PlaySound2D(dieSfx);
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

        OnEnemyAttack?.Invoke();

        yield return new WaitForSeconds(AttackAnimationDuration);

        IsAttack = false;

        yield return new WaitForSeconds(AttackCooldown);
        _canAttack = true;
    }
}
