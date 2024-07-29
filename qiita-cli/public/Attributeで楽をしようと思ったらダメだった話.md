---
title: Attributeで楽をしようと思ったらダメだった話
tags:
  - Unity
private: false
updated_at: '2024-07-30T00:00:29+09:00'
id: 0e1a81652c1661590340
organization_url_name: null
slide: false
ignorePublish: false
---
# はじめに
初めまして、IriSと申します  
田舎でUnityエンジニアをしているものです   
今日から重い腰を上げてアウトプットをしようと決意しました  
これから週1投稿を目指して頑張っていこうと思います  
よろしくお願いしますー！

# 題材となるコード
m_Typeで定義されているEnumをインスペクターから変えると、インスペクターに表示される変数が変わるようにしたいと思い  
エディタ拡張を考えたのですが、汎用性を考えてAttributeで実装しようとしました

```Unity
public enum Fruit
{
    APPLE,
    GRAPE,
    ORANGE,
}

public class FruitObject : MonoBehaviour
{
    [SerializeField] private Fruit m_Type;

    [SerializeField] private int m_AppleNum = 3;
} 
```


# CustomAttributeを作る

CustomAttributeでShowEnumというものを作ります  
表示非表示するためのEnumの名前と表示するEnumの値をintにキャストして代入していて
DrawerのOnGUI内で判定して描画します

```Unity
public class ShowEnumAttribute : PropertyAttribute
{
    public string EnumName; 
    public int Value;


    public ShowEnumAttribute(string enumName, int value)
    {
        EnumName = enumName;
        Value = value;
    }
}

[CustomPropertyDrawer(typeof(ShowEnumAttribute))]
    public class ShowEnumAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ShowEnumAttribute;
            var enumProperty = property.serializedObject.FindProperty(attr.EnumName);
            if (enumProperty == null || attr.Value != enumProperty.intValue) return;

            var height = GetPropertyHeight(property,label);
            position.height = height;
            
            EditorGUI.PropertyField(position, property);

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
```
# 完成?
事前に実装しておいたFruitObjectのm_AppleNumにAttributeを当ててみます

<img src="https://qiita-image-store.s3.ap-northeast-1.amazonaws.com/0/915230/bf2c2794-b992-f200-ac01-74d9e49772f2.gif">
<!--ここに画像-->

普通に動きます


次はGrapeを選択した際にintのListを表示したいので作成をします


```Unity
public class FruitObject : MonoBehaviour
{
      [SerializeField] private Fruit m_Type;

        [ShowEnum("m_Type",(int)Fruit.APPLE),SerializeField] private int m_AppleNum = 3;

        [ShowEnum("m_Type",(int)Fruit.GRAPE),SerializeField] private List<int> m_GrapeNum = new List<int>(); //追記
} 
```
<img src="https://qiita-image-store.s3.ap-northeast-1.amazonaws.com/0/915230/d11d9ed3-57ca-c117-4f0a-c19a0b7f2a71.png">
<!--ここに画像-->

なぜか動きません

# 原因

コードを見ていきます

```Unity
public class FruitObject : MonoBehaviour
{
        [ShowEnum("m_Type",(int)Fruit.GRAPE),SerializeField] private List<int> m_GrapeNum = new List<int>();
} 
```
AttributeにはListのGUIを表示させるためSerializeFieldも付与しています
しかし、そうしてしまうとシリアライズされ、SerializeFieldの描画が優先されてしまい
Enumが変わってもずっと表示されてしまいます


# 普通にエディタ拡張したほうがいい

```Unity
    [CustomEditor(typeof(FruitObject))]
    public class FruitObjectEditor: Editor
    {
        private SerializedProperty m_Type;
        private SerializedProperty m_AppleNum;
        private SerializedProperty m_GrapeNum;
        public void OnEnable()
        {
            
            m_Type = serializedObject.FindProperty("m_Type");
            m_AppleNum = serializedObject.FindProperty("m_AppleNum");
            m_GrapeNum = serializedObject.FindProperty("m_GrapeNum");
        }

        public override void OnInspectorGUI()
        {

            EditorGUILayout.PropertyField(m_Type);

            if (m_Type.intValue == (int)Fruit.APPLE)
            {
                EditorGUILayout.PropertyField(m_AppleNum);
            }
            
            if (m_Type.intValue == (int)Fruit.GRAPE)
            {
                EditorGUILayout.PropertyField(m_GrapeNum);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

```

面倒なことするくらいなら普通にエディター拡張を書いてしまったほうが早いですね

<img src="https://qiita-image-store.s3.ap-northeast-1.amazonaws.com/0/915230/728c38b4-8c5b-8a75-1f1e-db01e36d4237.gif">
