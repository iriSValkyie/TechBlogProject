using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Posts.UIToolkitとPreviewRenderUtilityを使ってオブジェクト配置ツールを作った.Scripts
{
    public class PlacementObjectItem :Button
    {
        private GameObject m_Prefab; 
        
        public PlacementObjectItem(PlacementData data)
        {
            this.style.width = this.style.height = 100;
            m_Prefab = data.Prefab;
            this.style.backgroundImage = data.PreviewImage;
            Label label = new Label();
            label.text = data.Name;
            this.Add(label);
            this.clicked += OnClicked;
            this.name = data.Name;
        }

        private void OnClicked()
        {
            Debug.Log($"OnClicked {m_Prefab.name}");
            var obj = GameObject.Instantiate(m_Prefab);
            obj.name = obj.name.Replace("(Clone)","");
            obj.transform.position = SceneView.lastActiveSceneView.camera.transform.forward * 3;
        }
    }
    
}