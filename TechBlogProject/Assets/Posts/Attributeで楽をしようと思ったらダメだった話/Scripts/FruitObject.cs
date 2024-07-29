using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechBlogPosts.EditorOrAttribute
{
    public class FruitObject : MonoBehaviour
    {
        [SerializeField] private Fruit m_Type;

        [ShowEnum("m_Type",(int)Fruit.APPLE),SerializeField] private int m_AppleNum = 3;

        [ShowEnum("m_Type",(int)Fruit.GRAPE),SerializeField] private List<int> m_GrapeNum = new List<int>();
    }

    public enum Fruit
    {
        APPLE,
        GRAPE,
        ORANGE,
    }

}
