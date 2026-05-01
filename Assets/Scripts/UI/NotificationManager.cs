using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;

    private static readonly int ShowTrigger = Animator.StringToHash(ShowParam);
    private static readonly int HideTrigger = Animator.StringToHash(HideParam);

    private const string ShowParam = "Show";
    private const string HideParam = "Hide";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Instance = this;
    }

    public void Show(string text, float showDuration = 3f, string soundName = "DefaultNotification")
    {
        StartCoroutine(NotificationLifespan(text, showDuration, soundName));
    }

    private IEnumerator NotificationLifespan(string text, float duration, string sound)
    {
        SfxManager.Instance.PlaySound2D(sound);

        if (notificationText != null)
            notificationText.text = text;

        _animator.SetTrigger(ShowTrigger);

        yield return new WaitForSeconds(duration);

        _animator.SetTrigger(HideTrigger);

        yield return new WaitForSeconds(duration);
    }
}
