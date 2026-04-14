using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCount;

    public int TotalCoins => _totalCoins;

    private const float PulseTextSizeMultiplier = 1.5f;
    private const float PulseTextAnimationDuration = 0.2f;

    private float _fadeDuration = 0.5f;
    private float _displayDuration = 4f;

    private static int _totalCoins = 0;

    private CanvasGroup _canvasGroup;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        UpdateCoinDisplay();
    }

    private void OnEnable()
    {
        Coin.OnAnyCoinCollected += OnCoinCollected;
    }

    private void OnDisable()
    {
        Coin.OnAnyCoinCollected -= OnCoinCollected;
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

    private void OnCoinCollected(int coinValue)
    {
        _totalCoins += coinValue;
        UpdateCoinDisplay();
        Show();
        PlayCoinPulseAnimation();

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(HideAfterDelay(_displayDuration));
    }

    private void UpdateCoinDisplay()
    {
        coinCount.text = _totalCoins.ToString();
    }

    private void PlayCoinPulseAnimation()
    {
        StartCoroutine(PulseText());
    }

    private IEnumerator PulseText()
    {
        Vector3 originalScale = coinCount.rectTransform.localScale;
        Vector3 targetScale = originalScale * PulseTextSizeMultiplier;

        float time = 0;

        while (time < PulseTextAnimationDuration)
        {
            coinCount.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, time / PulseTextAnimationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;

        while (time < PulseTextAnimationDuration)
        {
            coinCount.rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, time / PulseTextAnimationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        coinCount.rectTransform.localScale = originalScale;
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
