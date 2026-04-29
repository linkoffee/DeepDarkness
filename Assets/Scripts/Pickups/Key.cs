using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    private string _pickUpSound = "KeyPickUp";

    public static event Action OnKeyPickedUp; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            PickUpKey();
    }

    private void PickUpKey()
    {
        OnKeyPickedUp?.Invoke();
        SfxManager.Instance.PlaySound2D(_pickUpSound);

        Destroy(gameObject);
    }
}
