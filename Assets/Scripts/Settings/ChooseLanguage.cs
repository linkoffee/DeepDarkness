using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using TMPro;
using UnityEngine;

public class ChooseLanguage : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    public enum Language
    {
        English,
        Russian
    }

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        _dropdown.onValueChanged.AddListener(OnLanguageChanged);
        SetCurrentDropdownValue();
    }

    private void OnDestroy()
    {
        _dropdown.onValueChanged.RemoveListener(OnLanguageChanged);
    }

    public void SetLanguage(int languageIndex)
    {
        if (languageIndex >= 0 && languageIndex < System.Enum.GetValues(typeof(Language)).Length)
        {
            Language selectedLanguage = (Language)languageIndex;
            string languageName = selectedLanguage.ToString();

            LeanLocalization.SetCurrentLanguageAll(languageName);
        }
    }

    private void OnLanguageChanged(int languageIndex) => SetLanguage(languageIndex);

    private void SetCurrentDropdownValue()
    {
        string currentLanguage = LeanLocalization.GetFirstCurrentLanguage();

        if (System.Enum.TryParse(currentLanguage, out Language language))
            _dropdown.value = (int)language;
    }
}
