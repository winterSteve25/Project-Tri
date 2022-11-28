using System;
using System.Collections.Generic;
using Items;
using TMPro;
using UI.Utilities;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.TextContents
{
    public class TextContent
    {
        private const string PrefabsLocation = "UI/Prefabs/Text Content/PREFAB_";
        private readonly List<Action<Transform>> _layoutTopToDown;

        private TextMeshProUGUI _headerText;
        private TextMeshProUGUI _contentText;
        private LocalizeStringEvent _localizedHeaderText;
        private LocalizeStringEvent _localizedContentText;
        private UIItem _itemWithName;
        private Transform _itemStackList;
        private GameObject _iconAndText;
        private GameObject _iconAndLocalizedText;
        
        public TextContent(List<Action<Transform>> layout)
        {
            _layoutTopToDown = layout;
        }

        /// <summary>
        /// Creates and returns an empty tooltip object
        /// </summary>
        /// <returns></returns>
        public static TextContent Empty()
        {
            return new TextContent(new List<Action<Transform>>());
        }

        /// <summary>
        /// Adds a GameObject to the layout
        /// </summary>
        /// <param name="layer">Prefab to add</param>
        /// <returns></returns>
        public TextContent AddLayer(GameObject layer)
        {
            _layoutTopToDown.Add(transform => Object.Instantiate(layer, transform));
            return this;
        }

        /// <summary>
        /// Adds a layer of TMP text GameObject
        /// </summary>
        /// <param name="text">The text displayed, TMP special characters are supported</param>
        /// <param name="headerStyle">Whether the text should be displayed with the header template</param>
        /// <param name="fontSize">Font size</param>
        /// <returns></returns>
        public TextContent AddText(string text, bool headerStyle = false, float fontSize = -1)
        {
            TextMeshProUGUI t;

            if (headerStyle)
            {
                _headerText ??= Resources.Load<TextMeshProUGUI>($"{PrefabsLocation}Header");
                t = _headerText;

                if (fontSize <= 0)
                {
                    fontSize = 20;
                }
            }
            else
            {
                _contentText ??= Resources.Load<TextMeshProUGUI>($"{PrefabsLocation}Content");
                t = _contentText;

                if (fontSize <= 0)
                {
                    fontSize = 16;
                }
            }

            _layoutTopToDown.Add(transform =>
            {
                var tmp = Object.Instantiate(t, transform);
                tmp.text = text;
                tmp.fontSize = fontSize;
                transform.GetComponent<TextWrapper>().TextBoxes.Add(tmp);
            });
            return this;
        }

        public TextContent AddIconAndText(Sprite icon, string text, float fontSize = -1)
        {
            _iconAndText ??= Resources.Load<GameObject>($"{PrefabsLocation}Icon and Text");
            
            _layoutTopToDown.Add(transform =>
            {
                var go = Object.Instantiate(_iconAndText, transform);
                var tmp = go.GetComponentInChildren<TMP_Text>();
                tmp.text = text;
                if (fontSize > 0)
                {
                    tmp.fontSize = fontSize;
                }
                go.GetComponentInChildren<Image>().sprite = icon;
            });
            
            return this;
        }

        /// <summary>
        /// Adds a layer of translated TMP text GameObject
        /// </summary>
        /// <param name="text">The localized string object</param>
        /// <param name="headerStyle">Whether the text should be displayed with the header template</param>
        /// <param name="fontSize">Font size</param>
        /// <returns></returns>
        public TextContent AddText(LocalizedString text, bool headerStyle = false, float fontSize = -1)
        {
            LocalizeStringEvent t;

            if (headerStyle)
            {
                _localizedHeaderText ??= Resources.Load<LocalizeStringEvent>($"{PrefabsLocation}Localized Header");
                t = _localizedHeaderText;

                if (fontSize <= 0)
                {
                    fontSize = 20;
                }
            }
            else
            {
                _localizedContentText ??= Resources.Load<LocalizeStringEvent>($"{PrefabsLocation}Localized Content");
                t = _localizedContentText;

                if (fontSize <= 0)
                {
                    fontSize = 16;
                }
            }

            _layoutTopToDown.Add(transform =>
            {
                var localizedT = Object.Instantiate(t, transform);
                localizedT.StringReference = text;
                localizedT.RefreshString();
                var tmp = localizedT.GetComponent<TMP_Text>();
                tmp.fontSize = fontSize;
                transform.GetComponent<TextWrapper>().TextBoxes.Add(tmp);
            });
            return this;
        }

        public TextContent AddIconAndText(Sprite icon, LocalizedString text, float fontSize = -1)
        {
            _iconAndLocalizedText ??= Resources.Load<GameObject>($"{PrefabsLocation}Icon and Localized Text");
            
            _layoutTopToDown.Add(transform =>
            {
                var go = Object.Instantiate(_iconAndLocalizedText, transform);
                
                var strEvent = go.GetComponentInChildren<LocalizeStringEvent>();
                strEvent.StringReference = text;
                strEvent.RefreshString();

                var tmp = go.GetComponentInChildren<TMP_Text>();
                if (fontSize > 0)
                {
                    tmp.fontSize = fontSize;
                }
                
                go.GetComponentInChildren<Image>().sprite = icon;
            });
            
            return this;
        }

        /// <summary>
        /// Adds an item to the tooltip with an item icon and the item name with count
        /// </summary>
        /// <param name="itemStack">ItemStack to display, if count is 0 or below no count will be displayed</param>
        /// <returns></returns>
        public TextContent AddItem(ItemStack itemStack)
        {
            _itemWithName ??= Resources.Load<UIItem>($"{PrefabsLocation}Item With Name");
            _layoutTopToDown.Add(transform => Object.Instantiate(_itemWithName, transform).Item = itemStack);
            return this;
        }

        /// <summary>
        /// Adds items in a list with a grid layout.
        /// </summary>
        /// <param name="itemStacks">Items to display</param>
        /// <returns></returns>
        public TextContent AddItems(params ItemStack[] itemStacks)
        {
            _itemWithName ??= Resources.Load<UIItem>($"{PrefabsLocation}Item With Name");
            _itemStackList ??= Resources.Load<Transform>($"{PrefabsLocation}ItemStacks List");
            _layoutTopToDown.Add(transform =>
            {
                var list = Object.Instantiate(_itemStackList, transform);
                foreach (var itemStack in itemStacks)
                {
                    Object.Instantiate(_itemWithName, list).Item = itemStack;
                }
            });
            return this;
        }

        public TextContent Add(Action<Transform> instantiation)
        {
            _layoutTopToDown.Add(instantiation);
            return this;
        }

        public void Build(Transform tooltipObject)
        {
            foreach (var instantiation in _layoutTopToDown)
            {
                instantiation(tooltipObject);
            }
        }
    }
}