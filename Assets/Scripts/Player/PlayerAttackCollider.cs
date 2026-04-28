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
        if (_hasDamagedThisAttack) return;

        IDamageable damageableObj = collision.GetComponent<IDamageable>();

        if (damageableObj != null)
        {
            damageableObj.TakeDamage(Player.Instance.attackDamage);
            _hasDamagedThisAttack = true;
        }
    }
}
