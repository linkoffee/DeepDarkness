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

    [SerializeField] private string takeDamageSfx;
    [SerializeField] private string dieSfx;

    [SerializeField] private string[] blockMovementTags = { "Wall", "Enemy" };

    public event Action OnPlayerAttack;
    public event Action OnPlayerBlock;
    public event Action OnPlayerTakeDamage;
    public event Action OnPlayerDeath;
    public event Action OnPlayerRanIntoObstacle;

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

    private Coroutine _currentMoveCoroutine;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsBlockMovementTag(collision.gameObject.tag))
        {
            StopMovement();
            OnPlayerRanIntoObstacle.Invoke();
        }
    }

    private bool IsBlockMovementTag(string tag)
    {
        foreach (string blockingTag in blockMovementTags)
        {
            if (tag == blockingTag)
                return true;
        }
        return false;
    }

    private void StopMovement()
    {
        if (_currentMoveCoroutine != null)
        {
            StopCoroutine(_currentMoveCoroutine);
            _currentMoveCoroutine = null;
        }

        _isWalking = false;
        _isBusy = false;
    }

    public void MoveUp(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartMoveCoroutine(Vector3.up * stepCount);
    }

    public void MoveDown(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartMoveCoroutine(Vector3.down * stepCount);
    }

    public void MoveLeft(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartMoveCoroutine(Vector3.left * stepCount);
    }

    public void MoveRight(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartMoveCoroutine(Vector3.right * stepCount);
    }

    private void StartMoveCoroutine(Vector3 direction)
    {
        if (_currentMoveCoroutine != null)
        {
            StopCoroutine(_currentMoveCoroutine);
        }
        _currentMoveCoroutine = StartCoroutine(Move(direction));
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

        OnPlayerTakeDamage?.Invoke();
        SfxManager.Instance.PlaySound2D(takeDamageSfx);

        if (CurrentHealth <= 0)
            Die();
    }

    public void Die()
    {
        _isAlive = false;

        OnPlayerDeath?.Invoke();
        SfxManager.Instance.PlaySound2D(dieSfx);

        StopAllCoroutines();
        _isBusy = false;
        _isWalking = false;
    }

    public void EnableAttackCollider() => attackCollider.enabled = true;
    public void DisableAttackCollider() => attackCollider.enabled = false;

    private void EnableTakeDamageCollider() => takeDamageCollider.enabled = true;
    private void DisableTakeDamageCollider() => takeDamageCollider.enabled = false;

    private IEnumerator Move(Vector3 direction)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction;

        _isBusy = true;
        _isWalking = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            if (!_isWalking)
                break;

            Vector3 newPosition = Vector3.MoveTowards(
                transform.position, targetPosition, movingSpeed * Time.deltaTime
            );

            if (CheckCollisionAtPosition(newPosition))
            {
                transform.position = startPosition;
                _isWalking = false;
                _isBusy = false;
                _currentMoveCoroutine = null;
                yield break;
            }

            transform.position = newPosition;
            yield return null;
        }

        _isWalking = false;
        _isBusy = false;
        _currentMoveCoroutine = null;
    }

    private bool CheckCollisionAtPosition(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == gameObject)
                continue;

            if (IsBlockMovementTag(collider.gameObject.tag))
                return true;
        }

        return false;
    }

    private IEnumerator PerformAttack()
    {
        _isBusy = true;
        _isAttack = true;

        OnPlayerAttack?.Invoke();

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
        OnPlayerBlock?.Invoke();

        yield return new WaitForSeconds(BlockAnimationDuration);

        _isBlock = false;
        EnableTakeDamageCollider();

        yield return new WaitForSeconds(blockCooldown);
        _isBusy = false;
    }
}