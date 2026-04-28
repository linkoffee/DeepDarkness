using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorVisual : MonoBehaviour
{
    [SerializeField] private Door door;

    private static readonly int IsOpen = Animator.StringToHash(IsOpenParam);
    private const string IsOpenParam = "IsOpen";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        door.OnDoorOpened += OnDoorOpened;
    }

    private void OnDestroy()
    {
        door.OnDoorOpened -= OnDoorOpened;
    }

    private void OnDoorOpened() => _animator.SetBool(IsOpen, true);
}
