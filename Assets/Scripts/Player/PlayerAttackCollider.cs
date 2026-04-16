using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    private bool _hasDamagedThisAttack = false;

    public void ResetColliderState()
    {
        _hasDamagedThisAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || _hasDamagedThisAttack)
            return;

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null && enemy.IsAlive)
        {
            enemy.TakeDamage(Player.Instance.attackDamage);
            _hasDamagedThisAttack = true;
        }
    }
}
