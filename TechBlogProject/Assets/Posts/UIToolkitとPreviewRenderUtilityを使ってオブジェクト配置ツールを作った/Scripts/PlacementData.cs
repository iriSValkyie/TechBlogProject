using UnityEngine;

namespace Posts.UIToolkitとPreviewRenderUtilityを使ってオブジェクト配置ツールを作った.Scripts
{
    public class PlacementData
    {
        public string Name { get;private set; }
        public Texture2D PreviewImage { get;private set; }
        public GameObject Prefab { get;private set; }

        public PlacementData(Texture2D texture,GameObject prefab)
        {
            Name = prefab.name;
            PreviewImage = texture;
            Prefab = prefab;
        }
    }
}