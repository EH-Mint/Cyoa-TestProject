using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
public class KeySystem
{
    [LabelText("활성화 요구 선택지")]
    public List<Choice> Key;
    [LabelText("비활성화 할 선택지")]
    public HashSet<ScriptableObject> BenKey;
    [ReadOnly]public HashSet<ScriptableObject> ConnectKey=new HashSet<ScriptableObject>();//활성화 시 변경할 키들
    
    [ReadOnly]public HashSet<ScriptableObject> BenSave=new HashSet<ScriptableObject>();
    [ReadOnly]public bool Enable;//활성화되었는지
     public void AddBenKey(ScriptableObject Key){
        Enable=false;
        BenSave.Add(Key);  
    }
    public void RemoveBenKey(ScriptableObject Key){
        BenSave.Remove(Key);
        KeyReload();
    }
    public void KeyReload(){
        Enable=KeyCheck();
    }
    private bool KeyCheck(){
        if(BenSave.Count>0)return false;
        bool check=true;
        if(Key!=null){
            foreach(var x in Key){
                if(!x.Active)check=false;
            }
        }
        return check;
    }
}
