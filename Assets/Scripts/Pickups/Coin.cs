using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private string _collectSound = "CoinCollect";
    private int _coinValue = 1;

    public static event Action<int> OnAnyCoinCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            CollectCoin();
    }

    private void CollectCoin()
    {
        OnAnyCoinCollected?.Invoke(_coinValue);
        SfxManager.Instance.PlaySound2D(_collectSound);

        Destroy(gameObject);
    }
}
