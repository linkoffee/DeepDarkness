using System;
using UnityEngine;

public interface IPickable
{
    Sprite GetIcon();
    void OnPickup();
}

public static class PickupEvent
{
    public static event Action<Sprite> OnItemPickedUp;

    public static void NotifyPickup(Sprite icon)
    {
        OnItemPickedUp?.Invoke(icon);
    }
}
