using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace UI.Menu.Settings
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LanguageDropdown : MonoBehaviour
    {
        private void Start()
        {
            var dropdown = GetComponent<TMP_Dropdown>();

            foreach (var locale in LocalizationSettings.Instance.GetAvailableLocales().Locales)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
            }

            dropdown.onValueChanged.AddListener(index =>
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            });
            
            dropdown.RefreshShownValue();
        }
    }
}