using System;
using UnityEngine;
using UnityEngine.UI;

namespace UserSettings.GUIElements
{
    public class UserSettingsCategories : MonoBehaviour
    {
        [Serializable]
        private struct Category
        {
            public GameObject Group;
            public GameObject Highlight;
            public Button Activator;
        }

        [SerializeField] private Category[] _categories;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        private int _prev = -1;

        private void Awake()
        {
            for (int i = 0; i < _categories.Length; i++)
            {
                int index = i;
                Button button = _categories[i].Activator;
                if (button != null)
                {
                    button.onClick.AddListener(() => SelectCategory(index));
                }
            }

            if (_categories.Length > 0)
            {
                SelectCategory(0);
            }
        }

        private void Update()
        {
            if (_layoutGroup != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.GetComponent<RectTransform>());
            }
        }

        private void ToggleCategory(int index, bool isVisible)
        {
            if (index < 0 || index >= _categories.Length) return;

            Category cat = _categories[index];
            if (cat.Group != null)
            {
                cat.Group.SetActive(isVisible);
            }
            if (cat.Highlight != null)
            {
                cat.Highlight.SetActive(isVisible);
            }
        }

        private void SelectCategory(int cat)
        {
            if (cat == _prev) return;

            if (_prev >= 0 && _prev < _categories.Length)
            {
                ToggleCategory(_prev, false);
            }

            ToggleCategory(cat, true);

            _prev = cat;
        }
    }
}