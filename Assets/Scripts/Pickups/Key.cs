using System;
using UnityEngine;

public class Key : MonoBehaviour, IPickable
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string pickupSound = "KeyPickUp";

    public Sprite GetIcon() => icon;

    public static event Action OnKeyPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerInventory.Instance.HasKey)
        {
            PickupEvent.NotifyPickup(GetIcon());
            OnPickup();
        }
    }

    public void OnPickup()
    {
        OnKeyPickedUp?.Invoke();
        SfxManager.Instance.PlaySound2D(pickupSound);

        Destroy(gameObject);
    }
}
