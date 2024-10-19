using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Overlays;
using UnityEngine.UIElements;

namespace Posts.UIToolkitとPreviewRenderUtilityを使ってオブジェクト配置ツールを作った.Scripts
{
    [Overlay(typeof(SceneView), "配置ツール", true)]
    public class PlacementObjectView : Overlay
    {
        private VisualElement m_Root;
        private ScrollView m_ScrollView;
        private List<PlacementObjectItem> m_Items = new List<PlacementObjectItem>();

        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();
            
            root.style.flexDirection = FlexDirection.Column;
            var placementObjectViewToolbar = new PlacementObjectViewToolbar();
            root.style.width = 800;
            m_ScrollView = new ScrollView();
            m_ScrollView.style.marginTop = 10;
            placementObjectViewToolbar.OnSelectObject += OnSelectObject;
            placementObjectViewToolbar.OnSearchObject += OnSearchObject;
            root.Add(placementObjectViewToolbar);
            root.Add(m_ScrollView);
            m_Root = root;
            return m_Root;
        }

        private void OnSearchObject(string keyword)
        {
            if( m_Items.Count == 0) return;
            
            if (keyword == "")
            {
                foreach (var item in m_Items)
                {
                    item.style.display = DisplayStyle.Flex;
                }
                return;
            }

            foreach (var item in m_Items)
            {
                DisplayStyle display = DisplayStyle.Flex;
                if (item.name.IndexOf(keyword,StringComparison.OrdinalIgnoreCase) >= 0) display = DisplayStyle.Flex;
                else display = DisplayStyle.None;
                
                item.style.display = display;
            }
        }
        private void OnSelectObject(GameObject obj)
        {
            //Debug.Log("OnSelectObject");
            m_ScrollView.contentContainer.Clear();
            m_ScrollView.Clear();
            m_Items.Clear();
            if (obj == null)
            {
                //Debug.Log("un registerd");
                m_ScrollView.style.height = Length.Auto();
                return;
            }
            VisualElement itemWrapper = new VisualElement();
            itemWrapper.style.flexDirection = FlexDirection.Row;
            itemWrapper.style.flexWrap = Wrap.Wrap;
            var generator = new PlacementDataGenerator();
            PlacementData[] datas = generator.GenerateDatas(obj);
            generator.Dispose();
            if (datas == null)
            {
                //Debug.LogWarning("placementDatas null");
                return;
            }
           
            foreach (PlacementData data in datas)
            {
                var item = new PlacementObjectItem(data);
                m_Items.Add(item);
                itemWrapper.Add(item);
            }
            m_ScrollView.contentContainer.Add(itemWrapper);
            m_ScrollView.style.height = 350;

        }
    }
}
