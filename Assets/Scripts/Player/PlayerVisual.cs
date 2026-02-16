using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerVisual : MonoBehaviour
{
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
    }

    private void Update()
    {
        Vector2 currentPosition = transform.position;

        _moveDirection = (currentPosition - _lastPosition).normalized;

        UpdateMovementAnimation();

        _lastPosition = currentPosition;
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerAttack -= OnPlayerAttack;
        Player.Instance.OnPlayerBlock -= OnPlayerBlock;
    }

    private void OnPlayerAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(IsAttack);
    }

    private void OnPlayerBlock(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(IsBlocking);
    }

    private void UpdateMovementAnimation()
    {
        bool isMoving = Player.Instance.IsWalking;
        _animator.SetBool(IsWalking, isMoving);

        if (isMoving)
        {
            _animator.SetFloat(MoveX, _moveDirection.x);
            _animator.SetFloat(MoveY, _moveDirection.y);
        }
    }
}
