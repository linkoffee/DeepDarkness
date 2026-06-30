using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FinishLevelPoint : MonoBehaviour
{
    public static FinishLevelPoint Instance { get; private set; }

    public bool IsTransitioning => _isTransitioning;

    [SerializeField] private float loadingDuration = 1.0f;

    private static readonly int StartLevel = Animator.StringToHash(StartLevelParam);
    private static readonly int EndLevel = Animator.StringToHash(EndLevelParam);

    private const string StartLevelParam = "StartLevel";
    private const string EndLevelParam = "EndLevel";

    private BoxCollider2D _collider;
    private Animator levelTransitionAnimator;
    private GameObject levelTransition;

    private bool _isTransitioning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = true;
    }

    private void Start()
    {
        levelTransition = GameObject.FindGameObjectWithTag("UI.LevelTransition");
        GetTransitionAnimator();

        if (levelTransition != null)
            levelTransition.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isTransitioning)
        { 
            StartCoroutine(LoadSequence()); 
        }
    }

    private void GetTransitionAnimator()
    {
        if (levelTransition != null)
            levelTransitionAnimator = levelTransition.GetComponent<Animator>();
    }

    private IEnumerator LoadSequence()
    {
        if (_isTransitioning)
            yield break;

        _isTransitioning = true;
        _collider.enabled = false;

        CoinCounter.SaveData();
        LevelProgressManager.Instance.UnlockNextLevel();

        if (levelTransitionAnimator != null)
        {
            levelTransition.SetActive(true);
            levelTransitionAnimator.SetTrigger(EndLevel);
        }

        yield return new WaitForSeconds(loadingDuration);

        LevelLoader.Instance.LoadNextLevel();

        yield return new WaitForSeconds(loadingDuration);

        if (levelTransitionAnimator != null)
        {
            levelTransitionAnimator.SetTrigger(StartLevel);
            yield return new WaitForSeconds(2);
            levelTransition.SetActive(false);
        }

        _isTransitioning = false;
    }
}
