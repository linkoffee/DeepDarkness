using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    [SerializeField] private float _interactionRange = 1f;

    public event Action OnDoorOpened;

    private string _openSound = "DoorOpen";

    private BoxCollider2D _collider;
    private bool _isOpen = false;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_isOpen) return;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer <= _interactionRange && PlayerInventory.Instance.HasKey)
            OpenDoor();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _interactionRange);
    }

    private void OpenDoor()
    {
        _isOpen = true;

        if (_collider != null)
            _collider.enabled = false;

        OnDoorOpened?.Invoke();
        SfxManager.Instance.PlaySound2D(_openSound);
    }
}
