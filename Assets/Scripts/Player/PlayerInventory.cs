using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    public bool HasKey => _hasKey;

    private bool _hasKey;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Key.OnKeyPickedUp += OnKeyPickedUp;
    }

    private void OnDisable()
    {
        Key.OnKeyPickedUp -= OnKeyPickedUp;
    }

    private void OnKeyPickedUp() => _hasKey = true;
}
