using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Posts.UIToolkitとPreviewRenderUtilityを使ってオブジェクト配置ツールを作った.Scripts
{
    public class PlacementDataGenerator:IDisposable
    {
        private const float OBJECT_DISTANCE = 3f;
        private const float OBJECT_PREVIEW_SIZE_FACTOR = 0.6f;

        private PreviewRenderUtility m_PreviewRenderUtility = new();

        public PlacementData[] GenerateDatas(GameObject objects)
        {
            //Debug.Log("Create PreviewRender...");
            GameObject parentObject = m_PreviewRenderUtility.InstantiatePrefabInScene(objects);
            m_PreviewRenderUtility.AddSingleGO(parentObject);
            m_PreviewRenderUtility.camera.clearFlags = CameraClearFlags.Depth;
            m_PreviewRenderUtility.camera.orthographic = true;
            
            List<GameObject> childs = new List<GameObject>();
            List<PlacementData> placementDatas = new List<PlacementData>();
            //Debug.Log("Generate Datas...");
            parentObject.transform.Cast<Transform>().ToList().ForEach(child => child.gameObject.SetActive(false));
            for (int i = 0; i < parentObject.transform.childCount; i++)
            {
                GameObject child = parentObject.transform.GetChild(i).gameObject;
                Texture2D image = CaptureObjectImage(child);
                placementDatas.Add(new PlacementData(image,objects.transform.GetChild(i).gameObject)); 
            }
            //Debug.Log("Finish...");
            return placementDatas.ToArray();
        }
        private Texture2D CaptureObjectImage(GameObject child)
        {
            child.SetActive(true);
            Bounds bounds = GetRenderersBounds(child);
            //NOTE:child.transform.rightをしているのは今回使用しているアセットの大半がright方向を向いていたためです
            //本来はforwardにしています
            m_PreviewRenderUtility.camera.transform.position = bounds.center + child.transform.right * OBJECT_DISTANCE;
            m_PreviewRenderUtility.camera.transform.LookAt(bounds.center);
            m_PreviewRenderUtility.camera.orthographicSize = bounds.size.magnitude / (OBJECT_DISTANCE * OBJECT_PREVIEW_SIZE_FACTOR);
            m_PreviewRenderUtility.BeginStaticPreview(new Rect(0, 0, 400, 400));
            m_PreviewRenderUtility.Render();
            child.SetActive(false);
            return m_PreviewRenderUtility.EndStaticPreview();
        }
        private Bounds GetRenderersBounds(GameObject child)
        {
            Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
            bool first = true;
            Bounds totalBounds = new Bounds();
            foreach (var renderer in renderers)
            {
                if (first)
                {
                    totalBounds = renderer.bounds;
                    first = false;
                    continue;
                }
                totalBounds.Encapsulate(renderer.bounds);
            }
            return totalBounds;
        }
        public void Dispose()
        {
            m_PreviewRenderUtility.Cleanup();
        }
    }
}