using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverBox : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 1.5f;

    private string _gameOverSound = "GameOver";

    private CanvasGroup _canvasGroup;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeTo(1f, _fadeDuration));

        MusicManager.Instance.StopAll();
        SfxManager.Instance.PlaySound2D(_gameOverSound);
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
