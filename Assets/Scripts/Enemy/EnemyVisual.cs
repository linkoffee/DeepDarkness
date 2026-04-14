using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyAttackCollider attackCollider;

    private static readonly int IsDying = Animator.StringToHash(IsDyingParam);
    private static readonly int IsAttack = Animator.StringToHash(IsAttackParam);
    private static readonly int IsTakingDamage = Animator.StringToHash(IsTakingDamageParam);

    private const string IsDyingParam = "IsDying";
    private const string IsAttackParam = "IsAttack";
    private const string IsTakingDamageParam = "IsTakingDamage";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        enemy.OnEnemyAttack += OnEnemyAttack;
        enemy.OnEnemyTakeDamage += OnEnemyTakeDamage;
        enemy.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemy.OnEnemyAttack -= OnEnemyAttack;
        enemy.OnEnemyTakeDamage -= OnEnemyTakeDamage;
        enemy.OnEnemyDeath -= OnEnemyDeath;
    }

    private void LateUpdate()
    {
        if (enemy.IsAlive)
            UpdateSpriteFlip();
    }

    public void ResetAttackColliderState()
    {
        attackCollider.ResetColliderState();
    }

    public void EnableAttackCollider()
    {
        enemy.EnableAttackCollider();
    }

    public void DisableAttackCollider()
    {
        enemy.DisableAttackCollider();
    }

    private void OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(IsAttack);
    }

    private void OnEnemyTakeDamage(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(IsTakingDamage);
    }

    private void OnEnemyDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IsDying, true);
    }

    private void UpdateSpriteFlip()
    {
        if (enemy.PlayerTransform != null)
        {
            float directionToPlayer = enemy.PlayerTransform.position.x - transform.position.x;

            if (directionToPlayer > 0)
                _spriteRenderer.flipX = false;
            else if (directionToPlayer < 0)
                _spriteRenderer.flipX = true;
        }
    }
}
