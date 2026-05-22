using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class BossLoot : MonoBehaviour
{
    [SerializeField] private GameObject lootPrefab;

    private Enemy _bossEnemy;

    private void Awake()
    {
        _bossEnemy = GetComponent<Enemy>();
    }

    private void OnEnable() => _bossEnemy.OnEnemyDeath += SpawnLootItem;
    private void OnDisable() => _bossEnemy.OnEnemyDeath -= SpawnLootItem;

    private void SpawnLootItem()
    {
        if (lootPrefab == null ) return;

        Instantiate(lootPrefab, transform.position, Quaternion.identity);
    }
}
