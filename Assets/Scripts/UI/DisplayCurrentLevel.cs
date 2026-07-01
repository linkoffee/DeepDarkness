using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayCurrentLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLevel;

    private void Start()
    {
        currentLevel.text = LeanLocalization.GetTranslationText("currentLevel") + " " + SceneManager.GetActiveScene().buildIndex.ToString();
    }
}
