using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestVisual : MonoBehaviour
{
    [SerializeField] private BreakableObject chest;

    private static readonly int IsOpen = Animator.StringToHash(IsOpenParam);
    private static readonly int IsTakingDamage = Animator.StringToHash(IsTakingDamageParam);

    private const string IsOpenParam = "IsOpen";
    private const string IsTakingDamageParam = "IsTakingDamage";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        chest.OnObjectTakeDamage += OnTakeDamage;
        chest.OnObjectBreak += OnBreak;
    }

    private void OnDisable()
    {
        chest.OnObjectTakeDamage -= OnTakeDamage;
        chest.OnObjectBreak -= OnBreak;
    }

    private void OnTakeDamage() => _animator.SetTrigger(IsTakingDamage);
    private void OnBreak() => _animator.SetBool(IsOpen, true);
}
