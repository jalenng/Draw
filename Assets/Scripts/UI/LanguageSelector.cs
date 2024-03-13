using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] GameObject template;
    [SerializeField] GameObject footer;

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        UpdateOptions();
    }

    // Instantiate the options in the dialog
    public void UpdateOptions()
    {
        // Delete old buttons
        foreach (Transform button in footer.transform)
        {
            if (button.gameObject != template)
                Destroy(button.gameObject);
        }

        // Create new buttons
        List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (Locale l in locales)
        {
            GameObject newButton = Instantiate(template, footer.transform) as GameObject;
            newButton.SetActive(true);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = l.LocaleName;
            newButton.GetComponentInChildren<Button>().onClick.AddListener(() => SetLocale(l));
        }
    }

    private void SetLocale(Locale l)
    {
        string localeName = l.LocaleName;
        Debug.Log($"[LanguageSelector] Setting locale to {localeName}");
        LocalizationSettings.SelectedLocale = l;
    }
}
