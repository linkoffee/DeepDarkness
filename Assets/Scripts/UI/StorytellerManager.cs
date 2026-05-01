using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StorytellerManager : MonoBehaviour
{
    public static StorytellerManager Instance { get; private set; }

    public event Action OnClosed;

    [SerializeField] private GameObject storytellerBox;
    [SerializeField] private TextMeshProUGUI bodyText;
    [Space]
    [SerializeField] private float textSpeed = 0.05f;

    private static readonly int ShowTrigger = Animator.StringToHash(ShowParam);
    private static readonly int HideTrigger = Animator.StringToHash(HideParam);

    private const string ShowParam = "Show";
    private const string HideParam = "Hide";

    private const string ShowAnimationName = "BoxJumpOut";
    private const string HideAnimationName = "BoxJumpIn";

    private Animator _animator;
    private Coroutine _typingCoroutine;

    private string[] _pages;
    private int _currentPageIndex;

    private bool _isShowing = false;
    private bool _isTyping = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Instance = this;
    }

    public void Show(string text)
    {
        _pages = text.Split(new[] { "<page>" }, System.StringSplitOptions.None);

        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i] = _pages[i].Trim();
        }

        _currentPageIndex = 0;

        if (!_isShowing)
            StartCoroutine(ShowCurrentPage());
        else
            StartCoroutine(TypeCurrentPage());
    }

    public void OnClicked()
    {
        if (_isTyping)
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }

            bodyText.text = _pages[_currentPageIndex];
            _isTyping = false;
        }
        else
        {
            if (_currentPageIndex < _pages.Length - 1)
            {
                _currentPageIndex++;
                StartCoroutine(ShowCurrentPage());
            }
            else
            {
                StartCoroutine(Hide());
            }
        }
    }

    public void SetTextSpeed(float speed)
    {
        textSpeed = Mathf.Max(0.01f, speed);
    }

    public void SkipAll()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }

        StartCoroutine(Hide());
    }

    private IEnumerator ShowCurrentPage()
    {
        _animator.SetTrigger(ShowTrigger);
        yield return new WaitForSeconds(GetAnimationLength(ShowAnimationName));

        _typingCoroutine = StartCoroutine(TypeCurrentPage());
    }

    private IEnumerator TypeCurrentPage()
    {
        _isShowing = true;
        _isTyping = true;

        string currentPageText = _pages[_currentPageIndex];
        bodyText.text = "";

        foreach (char letter in currentPageText)
        {
            bodyText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        _isTyping = false;
        _typingCoroutine = null;
    }

    private IEnumerator Hide()
    {
        _animator.SetTrigger(HideTrigger);
        yield return new WaitForSeconds(GetAnimationLength(HideAnimationName));

        _isShowing = false;
        _currentPageIndex = 0;

        OnClosed?.Invoke();
    }

    private float GetAnimationLength(string animName)
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        AnimationClip clip = clips.FirstOrDefault(c => c.name.Contains(animName));

        return clip != null ? clip.length : 0.3f;
    }
}
