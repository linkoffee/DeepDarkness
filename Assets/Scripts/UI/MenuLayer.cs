using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuLayer : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup _canvasGroup;
    private Coroutine _fadeCoroutine;

    private bool _isVisible = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void Update()
    {
        HandleInputAction();
    }

    public void Show()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(ShowNPause());
    }

    public void Hide()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(HideNResume());
    }

    private void HandleInputAction()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isVisible)
                Hide();
            else
                Show();
        }
    }

    private IEnumerator ShowNPause()
    {
        yield return StartCoroutine(FadeTo(1f, fadeDuration));

        Time.timeScale = 0f;
        _isVisible = true;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator HideNResume()
    {
        Time.timeScale = 1f;

        yield return StartCoroutine(FadeTo(0f, fadeDuration));

        _isVisible = false;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    // Duplicate from CoinCounter
    // TODO: Move duplicate methods to Utils
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
