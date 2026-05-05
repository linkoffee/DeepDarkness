using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectLevelButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        LevelProgressManager.Instance.OnLevelUnlocked += OnLevelUnlocked;
        UpdateButtonState();
    }

    private void OnDestroy()
    {
        LevelProgressManager.Instance.OnLevelUnlocked -= OnLevelUnlocked;
    }

    public void OnClick() => LevelLoader.Instance.LoadLevelByIndex(levelIndex);

    private void UpdateButtonState() => _button.interactable = LevelProgressManager.Instance.IsLevelUnlocked(levelIndex);

    private void OnLevelUnlocked(int unlockedLevelIndex)
    {
        if (unlockedLevelIndex == levelIndex)
            UpdateButtonState();
    }
}
