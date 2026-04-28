using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private PlayerAttackCollider attackCollider;

    [Header("Target Settings")]
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private List<string> targetTags = new List<string> { "Enemy", "Pickup", "Breakable" };

    private static readonly int IsWalking = Animator.StringToHash(IsWalkingParam);
    private static readonly int IsDying = Animator.StringToHash(IsDyingParam);
    private static readonly int IsAttack = Animator.StringToHash(IsAttackParam);
    private static readonly int IsBlocking = Animator.StringToHash(IsBlockingParam);
    private static readonly int IsTakingDamage = Animator.StringToHash(IsTakingDamageParam);
    private static readonly int MoveX = Animator.StringToHash(MoveXParam);
    private static readonly int MoveY = Animator.StringToHash(MoveYParam);

    private const string IsWalkingParam = "IsWalking";
    private const string IsDyingParam = "IsDying";
    private const string IsAttackParam = "IsAttack";
    private const string IsBlockingParam = "IsBlocking";
    private const string IsTakingDamageParam = "IsTakingDamage";
    private const string MoveXParam = "MoveX";
    private const string MoveYParam = "MoveY";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _lastPosition;
    private Vector2 _moveDirection;
    private Vector2 _lastMoveDirection = Vector2.right;

    private Transform _currentTarget;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _lastPosition = transform.position;
    }

    private void Start()
    {
        Player.Instance.OnPlayerAttack += OnPlayerAttack;
        Player.Instance.OnPlayerBlock += OnPlayerBlock;
        Player.Instance.OnPlayerTakeDamage += OnPlayerTakeDamage;
        Player.Instance.OnPlayerDeath += OnPlayerDeath;
    }

    private void Update()
    {
        Vector2 currentPosition = transform.position;

        _moveDirection = (currentPosition - _lastPosition).normalized;

        UpdateMovementAnimation();
        FindNearestTarget();

        _lastPosition = currentPosition;
    }

    private void LateUpdate()
    {
        if (Player.Instance.IsAlive)
            UpdateSpriteFlip();
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerAttack -= OnPlayerAttack;
        Player.Instance.OnPlayerBlock -= OnPlayerBlock;
        Player.Instance.OnPlayerTakeDamage -= OnPlayerTakeDamage;
        Player.Instance.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);

        if (_currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _currentTarget.position);
        }
    }

    public void ResetAttackColliderState() => attackCollider.ResetColliderState();
    public void EnableAttackCollider() => Player.Instance.EnableAttackCollider();
    public void DisableAttackCollider() => Player.Instance.DisableAttackCollider();

    private void OnPlayerAttack(object sender, System.EventArgs e) => _animator.SetTrigger(IsAttack);
    private void OnPlayerBlock(object sender, System.EventArgs e) => _animator.SetTrigger(IsBlocking);
    private void OnPlayerTakeDamage(object sender, System.EventArgs e) => _animator.SetTrigger(IsTakingDamage);
    private void OnPlayerDeath(object sender, System.EventArgs e) => _animator.SetBool(IsDying, true);

    private void UpdateMovementAnimation()
    {
        bool isMoving = Player.Instance.IsWalking;
        _animator.SetBool(IsWalking, isMoving);

        if (isMoving)
        {
            _animator.SetFloat(MoveX, _moveDirection.x);
            _animator.SetFloat(MoveY, _moveDirection.y);

            if (Mathf.Abs(_moveDirection.x) > 0.01f || Mathf.Abs(_moveDirection.y) > 0.01f)
                _lastMoveDirection = _moveDirection;
        }
    }

    private void FindNearestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        float nearestDistance = float.MaxValue;
        Transform nearestTarget = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.transform == Player.Instance.transform)
                continue;

            if (targetTags.Contains(collider.tag))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = collider.transform;
                }
            }
        }

        _currentTarget = nearestTarget;
    }

    private void UpdateSpriteFlip()
    {
        if (_currentTarget != null)
        {
            float directionToTarget = _currentTarget.position.x - transform.position.x;
            _spriteRenderer.flipX = directionToTarget < 0;
        }
        else
        {
            _spriteRenderer.flipX = _lastMoveDirection.x < 0;
        }
    }
}
