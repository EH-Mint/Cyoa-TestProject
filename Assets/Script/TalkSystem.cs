using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public struct Choicement{
    public GameObject Choice;//선택지 오브젝트
    public TMPro.TMP_Text Txt;//선택지 텍스트
}
public class TalkSystem : SerializedMonoBehaviour
{
    [SerializeField]private TMPro.TMP_Text Name;//이름 텍스트
    [SerializeField]private TMPro.TMP_Text Say;//대화 텍스트
    [SerializeField]private GameObject Godness;//여신 전용 켄버스
    [SerializeField]private GameObject[] NPC;//NPC켄버스
    [SerializeField]private GameObject ChoiceSystem;//선택지 켄버스
    [SerializeField]private GameObject Talk;//대화창 켄버스
    [SerializeField]private RawImage NPCImg;//NPC이미지
    [SerializeField]private RawImage NPCBackGound;//NPC배경
    [SerializeField]private List<Choicement> Choice;//선택지 오브젝트
    [SerializeField]private Button NextBt;//대화 버튼
    [SerializeField]private GameObject DarkVfx;//마신이펙트
    public NPCTalk TalkData;//대화 데이터
    private bool TalkOn;//대화가 움직이는 중인지
    private bool Mod;//스킵할지 여부
    private int Page;//남은 대화 숫자
    public void Load(){
        var dt=Resources.Load<DM>("DataManager");
        dt.save.SkipButton.SetActive(TalkData.Skip);
        if(!TalkData.ChangeBgm.Equals(string.Empty)){
            dt.save.SoundSystem.PlayBGM(TalkData.ChangeBgm);
        }
        DarkVfx.gameObject.SetActive(TalkData.Npc==NpcList.마신);
        if(TalkData.Npc==NpcList.여신){
            foreach(var x in NPC){
                x.SetActive(false);
            }
            Godness.SetActive(true);
        }
        else{
            Godness.SetActive(false);
            foreach(var x in NPC){
                x.SetActive(true);
            }
            NPCImg.texture=dt.NPCList[TalkData.Npc].Img;
            NPCBackGound.texture=dt.NPCList[TalkData.Npc].BackGround;
        }
        Page=0;
        LoadPage();
    }
    private async UniTask SetTalk(string talk,float speed){
        TalkOn=true;
        Mod=false;
        Say.text="";
        NPCTalk npcsave=TalkData;
        for(int i=0;i<talk.Length;i++){
            if(npcsave!=TalkData)break;
            if(talk[i]=='<'){
                Mod=true;
            }
            else if(talk[i]=='>')Mod=false;
            Say.text+=talk[i];
            if(!Mod){
                if(speed>0&&TalkOn){
                    await UniTask.Delay(TimeSpan.FromSeconds(speed));
                }
            }
        }
       if(npcsave==TalkData) TalkOn=false;
    }
    public void Skip(){
        if(TalkOn)TalkOn=false;
        else if(TalkData){
            NextBt.interactable=false;
            SkipPage().Forget();
        }
    }
    private async UniTask SkipPage(){
        if(TalkData.TalkList[Page].EndFX!=FXList.없음){
            var dm=Resources.Load<DM>("DataManager");
            dm.save.SoundSystem.SFXPlay(TalkData.TalkList[Page].FXSound);
            await UniTask.Delay(TimeSpan.FromSeconds(dm.save.fxsystem.Play(TalkData.TalkList[Page].EndFX)));
        }
        Page++;
        LoadPage();
        NextBt.interactable=true;
    }
    private void LoadPage(){
        if(TalkData.TalkList!=null&&TalkData.TalkList.Count>Page){
            ChoiceSystem.SetActive(false);
            Talk.SetActive(true);
            Name.text=TalkData.TalkList[Page].Name;
            SetTalk(TalkData.TalkList[Page].Say,TalkData.TalkList[Page].Speed).Forget();
           
        }
        else{
            if(TalkData.Choice!=null&&TalkData.Choice.Count>0){
                ChoiceSystem.SetActive(true);
                Talk.SetActive(false);
                for(int i=0;i<Choice.Count;i++){
                    if(i<TalkData.Choice.Count){
                        Choice[i].Choice.SetActive(true);
                        if(TalkData.Choice[i].choice is Choice)Choice[i].Txt.text=(TalkData.Choice[i].choice as Choice).Info;
                        else Choice[i].Txt.text=TalkData.Choice[i].choice as string;
                    }
                    else{
                        Choice[i].Choice.SetActive(false);
                    }
                }
            }
            else{
                if(!TalkData.NextTalk)Resources.Load<DM>("DataManager").SetPage();
                else{
                    TalkData= TalkData.NextTalk;
                    Load();
                }
            }
        }
    }
    public void PressChoice(int num){
        var dm= Resources.Load<DM>("DataManager");
        if(TalkData.Choice[num].NextTalk){
            if(TalkData.Choice[num].choice is Choice)(TalkData.Choice[num].choice as Choice).ActiveChoice();
            TalkData= TalkData.Choice[num].NextTalk;
            Load();
        }
        else{
            if(!TalkData.NextTalk)dm.SetPage();
            else{
                TalkData= TalkData.NextTalk;
                Load();
            }
        }
    }
}
