using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System.IO;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif
public struct ActiveKey{
    public string Key;//활성화 시 필요한 키
    public bool ActiveCheck;//활성화 되어야 하는지
}
[ExecuteInEditMode]
[CreateAssetMenu(fileName ="DataManger",menuName ="Data/DataManger")]
public class DM : SerializedScriptableObject
{
    public ObjectSave save;
    //[SerializeField]private ChoiceSystem CYOA;
    public List<ScriptableObject> RowList=new List<ScriptableObject>();//CYOA진행 데이터 
    public Dictionary<NpcList,NPC> NPCList;//NPC데이터
    public Stack<Choice> ChoiceList;
    #if UNITY_EDITOR
    [SerializeField]private Material fontdefualt;
    [SerializeField]private Material fontdefualt2;
    [SerializeField]private Sprite set;
    [SerializeField]private Sprite set2;
    [Button("마테리얼")]
    public void Setmaterial(){
        foreach(var x in ChoiceList){
            if(x.Front==set||x.Front==set2)x.FontMaterial=fontdefualt;
            else x.FontMaterial=fontdefualt2;
            
        }
    }
    [Button("선택지연결")]
    public void ConnectCYOA(){
        ChoiceList = new Stack<Choice>();
        DirectoryInfo dir = new DirectoryInfo("Assets/Data/Choice/");
        FileInfo[] files = dir.GetFiles("*.asset");

        foreach (FileInfo file in files)
        {
            string relativePath = "Assets" + file.FullName.Substring(Application.dataPath.Length);
            Choice so = AssetDatabase.LoadAssetAtPath<Choice>(relativePath);

            if (so != null)
            {
                ChoiceList.Push(so);
                Debug.Log("Loaded ScriptableObject: " + so.name);
            }
        }

        foreach(var x in ChoiceList){
            x.K_System.ConnectKey=new HashSet<ScriptableObject>();
        }
        foreach(var x in ChoiceList){
            if(x.K_System.Key!=null){
                foreach(var y in x.K_System.Key){
                    y.K_System.ConnectKey.Add(x);             
                }
            }
            if(x.CCost!=null){
                foreach(var y in from y in x.CCost where x.CCost.Values!=null select y){
                    if(y.Key.CCost==null||!y.Key.CCost.ContainsKey(x)){
                        if(y.Key.CCost==null){
                            y.Key.CCost=new Dictionary<Choice, List<Point>>();
                        }
                        else if(y.Key.CCost.ContainsKey(x)&&(y.Key.CCost[x]==null||y.Key.CCost[x]!=y.Value)){
                            y.Key.CCost[x]=y.Value;
                        }
                        else{
                            y.Key.CCost.Add(x,y.Value);
                        }
                        
                    }
                   
                }
            }
        }
        KeySystem k;
        foreach(var x in RowList){
            k=(x is Row)?(x as Row).K_System:(x as NPCTalk).K_System;
            if(k.Key!=null){
                foreach(var y in k.Key){
                    y.K_System.ConnectKey.Add(x);             
                }
            }
        }
    }
    #endif
    public Row SelectRow;
    private int pray;
    public int Pray{
        get{return pray;}
        set{
            pray=value;
            save.PrayText.text="소망:"+pray;
        }
    }
    private int destiny;
    public int Destiny{
        get{return destiny;}
        set{
            destiny=value;
            save.DestinyText.text="운명:"+destiny;
        }
    }


    public int page=0;
    public void BackPage(){
        do{
            page--;
        }while(RowList[page] is NPCTalk||!(RowList[page] as Row).K_System.Enable);
        save.CyoaSystem.row=RowList[page] as Row;
        SelectRow=save.CyoaSystem.row;
        save.CyoaSystem.Reset();
    }
    public void SetPage(){
        do{
            page++;
        }while(!EnableCheck());
        if(RowList[page] is Row){
            save.CyoaSystem.row=RowList[page] as Row;
            SelectRow=save.CyoaSystem.row;
            save.CyoaSystem.Reset();
            save.CyoaSystem.gameObject.SetActive(true);
            save.talksystem.gameObject.SetActive(false);
        }
        else{
            var N=RowList[page] as NPCTalk;
            N.Active=false;
            save.talksystem.TalkData=N;
            save.talksystem.Load();
            save.CyoaSystem.gameObject.SetActive(false);
            save.talksystem.gameObject.SetActive(true);
        }
        
    }
    public void Skip(){
        while(RowList[page] is NPCTalk&&(RowList[page] as NPCTalk).Skip){
            var N=RowList[page] as NPCTalk;
            N.Active=false;
            page++;
        }
        page--;
        SetPage();
    }

    public int Power;//전투력
    public int MP;
    public int Talk;//사교성
    public int Mental;
    public int HP;
    public string NameSave;
    public List<string> Key;//마신계약
    private bool EnableCheck(){
        if(RowList[page] is NPCTalk&&!(RowList[page] as NPCTalk).Active)return false;
        if(RowList[page]is Row)return (RowList[page] as Row).K_System.Enable;
        else return (RowList[page] as NPCTalk).K_System.Enable;
    }
    public void SetName(TMP_InputField tp){
        NameSave=tp.text;
    }
    public void Reset(){
        foreach(var x in ChoiceList){
            x.Active=false;
            x.K_System.Enable=x.K_System.Key==null;
            
            x.K_System.BenSave=new HashSet<ScriptableObject>();
        }
        foreach(var x in from x  in RowList where x is Row select x as Row){
            x.K_System.Enable=x.K_System.Key==null;
            if(x.MinSelect!=0)x.Complet=false;
            else x.Complet=true;
        }
        foreach(var x in from x  in RowList where x is NPCTalk select x as NPCTalk){
            x.Active=true;
            x.K_System.Enable=x.K_System.Key==null;
        }
        page=-1;
        Destiny=0;
        Pray=0;
        Key=new List<string>();
        Power=0;
        MP=600;
        Talk=600;
        Mental=600;
        HP=600;
    }
}