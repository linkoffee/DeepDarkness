using TMPro;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using DD.Utils;
using System.Text.RegularExpressions;

public class VarTagProcessor : MonoBehaviour, ITextPreprocessor
{
    [SerializeField] private List<Enemy> enemies;

    private Dictionary<string, string> vars = new Dictionary<string, string>();

    private void Awake()
    {
        var tmpText = GetComponent<TMP_Text>();

        if (tmpText != null)
            tmpText.textPreprocessor = this;

        LoadVars();
    }

    private void OnEnable()
    {
        LeanLocalization.OnLocalizationChanged += OnLanguageChanged;
    }

    private void OnDisable()
    {
        LeanLocalization.OnLocalizationChanged -= OnLanguageChanged;
    }

    private void OnLanguageChanged()
    {
        LoadVars();

        var tmpText = GetComponent<TMP_Text>();

        if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
            tmpText.ForceMeshUpdate();
    }

    public void LoadVars()
    {
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            string enemyName = enemy.EnemyData.name.ToLower();

            vars[$"{enemyName}_name"] = GetLocalizedEnemyData(enemy.EnemyData, "name");
            vars[$"{enemyName}_desc"] = GetLocalizedEnemyData(enemy.EnemyData, "desc");
            vars[$"{enemyName}_health"] = enemy.EnemyData?.health.ToString();
            vars[$"{enemyName}_damage"] = enemy.EnemyData?.attackDamage.ToString();
        }
    }

    public string PreprocessText(string text)
    {
        string processedText = Regex.Replace(text, @"<var>(\w+)</var>", match =>
        {
            string varName = match.Groups[1].Value;
            if (vars.TryGetValue(varName, out string value))
                return value;
            return match.Value;
        });

        processedText = Regex.Replace(processedText, @"{(\w+)}", match =>
        {
            string varName = match.Groups[1].Value;
            return vars.GetValueOrDefault(varName, match.Value);
        });

        return processedText;
    }

    private string GetLocalizedEnemyData(EnemySO data, string fieldName)
    {
        string translationName = $"{data.name.ToLower()}Enemy{fieldName.FirstCharToUpper()}";
        string localizedText = LeanLocalization.GetTranslationText(translationName);

        if (!string.IsNullOrEmpty(localizedText))
            return localizedText;

        string fallbackText = fieldName switch
        {
            "name" => data.name,
            "desc" => data.description,
            _ => ""
        };

        return fallbackText;
    }
}
