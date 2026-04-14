using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;

    [Header("Sprites")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private float _fadeDuration = 0.5f;
    private float _hidingDelay = 5f;

    private Player _player;
    private CanvasGroup _canvasGroup;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        _player.OnHealthChanged += UpdateHealthDisplay;
    }

    private void OnDisable()
    {
        _player.OnHealthChanged -= UpdateHealthDisplay;
    }

    private void Start()
    {
        CreateHearts(_player.maxHealth);
        UpdateHealthDisplay(_player.CurrentHealth);
    }

    private void Show()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        StartCoroutine(FadeTo(1f, _fadeDuration));
    }

    private void Hide()
    {
        StartCoroutine(FadeTo(0f, _fadeDuration));
    }

    private void UpdateHealthDisplay(int currentHealth)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Image heartImage = transform.GetChild(i).GetComponent<Image>();

            if (heartImage != null)
                heartImage.sprite = (i < currentHealth) ? fullHeartSprite : emptyHeartSprite;
        }

        Show();

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(HideAfterDelay(_hidingDelay));
    }

    private void CreateHearts(int count)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            heart.name = $"HealthHeart{i + 1}";
        }
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = _canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;
    }
}
