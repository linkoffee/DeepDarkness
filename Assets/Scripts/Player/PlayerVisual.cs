using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerVisual : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash(IsWalkingTrigger);
    private static readonly int IsDying = Animator.StringToHash(IsDyingTrigger);
    private static readonly int IsAttack = Animator.StringToHash(IsAttackTrigger);
    private static readonly int IsBlocking = Animator.StringToHash(IsBlockingTrigger);
    private static readonly int IsTakingDamage = Animator.StringToHash(IsTakingDamageTrigger);

    private const string IsWalkingTrigger = "IsWalking";
    private const string IsDyingTrigger = "IsDying";
    private const string IsAttackTrigger = "IsAttack";
    private const string IsBlockingTrigger = "IsBlocking";
    private const string IsTakingDamageTrigger = "IsTakingDamage";
    
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _lastPosition;
    private bool _isFacingRight = true;


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

        UpdateWalkAnimation();
        UpdateFacingDirection();

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

    private void UpdateWalkAnimation()
    {
        _animator.SetBool(IsWalking, Player.Instance.IsWalking);
    }

    private void UpdateFacingDirection()
    {
        float horizontalMovement = transform.position.x - _lastPosition.x;

        if (Mathf.Abs(horizontalMovement) > 0.01f)
        {
            bool isShouldFacingRight = horizontalMovement > 0;

            _spriteRenderer.flipX = isShouldFacingRight != _isFacingRight;
        }
    }
}
