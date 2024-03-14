using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public struct Talk{
    [LabelText("이름")]
    public string Name;
    [LabelText("대화문속도")]
    public float Speed;
    [LabelText("대화내용")]
    [Multiline(10)]
    public string Say;
    public FXList EndFX;
    public AudioClip FXSound;//이펙트사운드
}
public struct TalkChoice{
    [LabelText("대화시 넘어갈 선택지")]
    public NPCTalk NextTalk;
    [LabelText("대화내용")]
    public object choice;

}
public class NPCTalk : SerializedScriptableObject
{
    public NpcList Npc;//NPC종류
    public List<Talk> TalkList;//대화 List
    public NPCTalk NextTalk;//다음에 올 대화
    public List<TalkChoice> Choice; //선택지
    public bool Active;//활성화 되었는지
    public string ChangeBgm;
    public bool Skip;//스킵가능
    public KeySystem K_System=new KeySystem();
}
