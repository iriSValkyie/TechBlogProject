using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Posts.UIToolkitとPreviewRenderUtilityを使ってオブジェクト配置ツールを作った.Scripts
{
    public class PlacementObjectViewToolbar : VisualElement
    {
        public Action<GameObject> OnSelectObject { get; set; }

        public Action<string> OnSearchObject { get; set; }

        public PlacementObjectViewToolbar()
        {
            this.style.marginTop = 5;
            this.style.flexDirection = FlexDirection.Row;
            this.style.justifyContent = Justify.SpaceBetween;
            ObjectField objectField = new ObjectField();
            this.Add(objectField);
            objectField.objectType = typeof(GameObject);
            objectField.RegisterValueChangedCallback(OnObjectRegisterdValue);
            objectField.UnregisterValueChangedCallback(OnObjectUnRegisterdValue);

            ToolbarSearchField searchField = new ToolbarSearchField();
            this.Add(searchField);
            searchField.RegisterValueChangedCallback(OnSearchFieldRegisterdValue);
            searchField.UnregisterValueChangedCallback(OnSearchFieldUnRegisterdValue);
        }

        private void OnObjectRegisterdValue(ChangeEvent<Object> value)
        {
            OnSelectObject?.Invoke(value.newValue.GameObject());
        }

        private void OnObjectUnRegisterdValue(ChangeEvent<Object> value)
        {
            OnSelectObject?.Invoke(null);
        }

        private void OnSearchFieldRegisterdValue(ChangeEvent<string> value)
        {
            OnSearchObject?.Invoke(value.newValue);
        }

        private void OnSearchFieldUnRegisterdValue(ChangeEvent<string> value)
        {
            OnSearchObject?.Invoke("");
        }
    }
}
