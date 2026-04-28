using System;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private BreakableSO data;
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private bool destroyOnBreak = false;

    public event Action OnObjectTakeDamage;
    public event Action OnObjectBreak;

    private int currentStrength;
    private bool isBroken = false;

    private void Start()
    {
        if (data == null)
        {
            Debug.LogError($"BreakableObject {gameObject.name} missing BreakableSO");
            return;
        }
        currentStrength = data.strength;
    }

    public void TakeDamage(int damage)
    {
        if (isBroken) return;

        currentStrength -= damage;

        OnObjectTakeDamage?.Invoke();

        if (currentStrength <= 0)
            Break();
    }

    private void Break()
    {
        if (isBroken) return;

        isBroken = true;
        DropLoot();

        OnObjectBreak?.Invoke();

        if (destroyOnBreak)
            Destroy(gameObject);
    }

    private void DropLoot()
    {
        if (lootPrefab == null)
        {
            Debug.LogWarning($"BreakableObject {gameObject.name} missing Loot Prefab");
            return;
        }

        int lootCount = UnityEngine.Random.Range(data.minDroppedLootCount, data.maxDroppedLootCount);

        for (int i = 0; i < lootCount; i++)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            Vector3 spawnPosition = transform.position + (Vector3)randomOffset;

            GameObject loot = Instantiate(lootPrefab, spawnPosition, Quaternion.identity);
            DroppedLoot lootItem = loot.AddComponent<DroppedLoot>();

            lootItem.Initialize();
        }
    }
}
