using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private bool _hasDamagedThisAttack = false; 

    public void ResetColliderState()
    {
        _hasDamagedThisAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !(collision is BoxCollider2D) || _hasDamagedThisAttack)
            return;

        Player.Instance.TakeDamage(enemy.EnemyData.attackDamage);

        _hasDamagedThisAttack = true;
    }
}
