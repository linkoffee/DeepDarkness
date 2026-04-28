using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] public int maxHealth = 3;
    [SerializeField] private float movingSpeed = 1f;

    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private Collider2D takeDamageCollider;

    [SerializeField] public int attackDamage = 1;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float blockCooldown = 2f;

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerBlock;
    public event EventHandler OnPlayerTakeDamage;
    public event EventHandler OnPlayerDeath;

    public event Action<int> OnHealthChanged;

    public static Player Instance { get; private set; }
    public bool IsBusy => _isBusy;
    public bool IsAttack => _isAttack;
    public bool IsBlock => _isBlock;
    public bool IsWalking => _isWalking;
    public bool IsAlive => _isAlive;

    public int CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            int newValue = Mathf.Clamp(value, 0, maxHealth);

            if (_currentHealth != newValue)
            {
                _currentHealth = newValue;
                OnHealthChanged?.Invoke(_currentHealth);
            }
        }
    }
    private int _currentHealth;

    private const float AttackAnimationDuration = 0.3f;
    private const float BlockAnimationDuration = 0.5f;

    private bool _isBusy = false;
    private bool _isAlive;
    private bool _isWalking;
    private bool _isAttack;
    private bool _isBlock;

    private void Awake()
    {
        Instance = this;
        DisableAttackCollider();
        EnableTakeDamageCollider();
    }

    private void Start()
    {
        _isAlive = true;
        CurrentHealth = maxHealth;
    }

    public void MoveUp(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(Move(Vector3.up * stepCount));
    }
    
    public void MoveDown(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(Move(Vector3.down * stepCount));
    }
    
    public void MoveLeft(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(Move(Vector3.left * stepCount));
    }
    
    public void MoveRight(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(Move(Vector3.right * stepCount));
    }

    public void Attack()
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(PerformAttack());
    }

    public void Block()
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(PerformBlock());
    }

    public void TakeDamage(int damage)
    {
        if (!_isAlive || _isBlock)
            return;

        CurrentHealth -= damage;
        OnPlayerTakeDamage?.Invoke(this, EventArgs.Empty);

        if (CurrentHealth <= 0)
            Die();
    }

    public void Die()
    {
        _isAlive = false;
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }
    
    public void EnableAttackCollider() => attackCollider.enabled = true;
    public void DisableAttackCollider() => attackCollider.enabled = false;

    private void EnableTakeDamageCollider() => takeDamageCollider.enabled = true;
    private void DisableTakeDamageCollider() => takeDamageCollider.enabled = false;

    private IEnumerator Move(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;
        
        _isBusy = true;
        _isWalking = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, targetPosition, movingSpeed * Time.deltaTime
            );
            yield return null;
        }

        _isWalking = false;
        _isBusy = false;
    }

    private IEnumerator PerformAttack()
    {
        _isBusy = true;
        _isAttack = true;

        OnPlayerAttack?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(AttackAnimationDuration);

        _isAttack = false;

        yield return new WaitForSeconds(attackCooldown);
        _isBusy = false;
    }

    private IEnumerator PerformBlock()
    {
        _isBusy = true;
        _isBlock = true;

        DisableTakeDamageCollider();
        OnPlayerBlock?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(BlockAnimationDuration);

        _isBlock = false;
        EnableTakeDamageCollider();

        yield return new WaitForSeconds(blockCooldown);
        _isBusy = false;
    }
}
