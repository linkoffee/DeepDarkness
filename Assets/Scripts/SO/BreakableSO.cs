using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "SO/Breakable Data")]
public class BreakableSO : ScriptableObject
{
    [Header("Base")]
    public new string name;
    [TextArea] public string description;

    [Header("Stats")]
    public int strength;
    [Range(2, 50)] public int minDroppedLootCount;
    [Range(2, 50)] public int maxDroppedLootCount;
}
