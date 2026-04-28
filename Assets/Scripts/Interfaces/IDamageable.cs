using UnityEngine;

/*
 * An interface for all objects capable of receiving damage.
 * These can be living objects, such as enemies and NPCs.
 * They can also be breakable objects.
*/
public interface IDamageable
{
    void TakeDamage(int damage);
}
