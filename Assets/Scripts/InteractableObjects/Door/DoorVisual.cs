using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorVisual : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash(IsOpenParam);
    private const string IsOpenParam = "IsOpen";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Door.OnDoorOpened += OnDoorOpened;
    }

    private void OnDestroy()
    {
        Door.OnDoorOpened -= OnDoorOpened;
    }

    private void OnDoorOpened() => _animator.SetBool(IsOpen, true);
}
