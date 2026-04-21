using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "SO/Enemy Data")]
public class EnemySO : ScriptableObject
{
    [Header("Base")]
    public new string name = "Enemy";
    public string description = "Some dungeon dweller...";

    [Header("Stats")]
    public int health = 2;
    public int attackDamage = 1;
}
