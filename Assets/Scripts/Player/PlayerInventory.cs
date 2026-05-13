using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Sprite _defaultImage;
    private Image _itemImage;

    public static PlayerInventory Instance { get; private set; }

    public bool HasKey => _hasKey;

    private bool _hasKey;

    private void Awake()
    {
        _itemImage = GetComponent<Image>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Key.OnKeyPickedUp += OnKeyPickedUp;
        Door.OnAnyDoorOpened += OnAnyDoorOpened;
        PickupEvent.OnItemPickedUp += OnItemPickedUp;
    }

    private void OnDisable()
    {
        Key.OnKeyPickedUp -= OnKeyPickedUp;
        Door.OnAnyDoorOpened -= OnAnyDoorOpened;
        PickupEvent.OnItemPickedUp -= OnItemPickedUp;
    }

    private void OnKeyPickedUp() => _hasKey = true;
    private void OnAnyDoorOpened()
    {
        _hasKey = false;
        ResetInventoryView();
    }

    private void OnItemPickedUp(Sprite itemIcon)
    {
        if (_itemImage != null)
            _itemImage.sprite = itemIcon;
    }

    private void ResetInventoryView()
    {
        if (_itemImage != null)
            _itemImage.sprite = _defaultImage;
    }
}
