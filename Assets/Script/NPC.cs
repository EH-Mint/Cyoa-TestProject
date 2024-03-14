using UnityEngine;
using Sirenix.OdinInspector;
public enum NpcList{
    여신,
    마신,
    책
}
[CreateAssetMenu(fileName ="NPC",menuName ="Data/NPC")]
public class NPC : SerializedScriptableObject
{
    [HideLabel]
    [PreviewField(100, ObjectFieldAlignment.Left)]
    [HorizontalGroup("Split")]
    [VerticalGroup("Split/Left")]
    [HorizontalGroup("Split/Left/Split", 100, LabelWidth = 60)]
    public Texture Img;
    [VerticalGroup("Split/Left/Split/Right")]
    [LabelText("이름")]
    public string Name;
    [VerticalGroup("Split/Left/Split/Right")]
    [LabelText("호감도")]
    public int Love;

    [VerticalGroup("Split/Left/Split/Right")]
    [LabelText("배경")]
    public Texture BackGround;
    [VerticalGroup("Split/Left/Split/Right")]
    [LabelText("배경음")]
    public AudioClip 배경음;
}
