using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class RemotContorl : SerializedMonoBehaviour
{
    public Dictionary<Button,Row> RemoteButton;
    public Dictionary<Row,Button> RemoteButto2;
    [SerializeField]private GameObject selectmenu;
    [Button("a")]
    public void Set(){
        RemoteButto2=new Dictionary<Row, Button>();
        foreach(var x in RemoteButton){
            RemoteButto2.Add(x.Value,x.Key);
        }
    }
    private void OnEnable() {
        RemoteSet();
    }
    public void Click(Button bt){
        var data=Resources.Load<DM>("DataManager");
        for(int i=0;i<data.RowList.Count;i++){
            if(data.RowList[i]==RemoteButton[bt]){
                data.page=i-1;
                break;
            }
        }
        data.SetPage();
        transform.parent.gameObject.SetActive(false);
        selectmenu.SetActive(true);
    }
    private void RemoteSet(){
        var data=Resources.Load<DM>("DataManager");
        Row r;
        NPCTalk t;
        bool ck=true;
        for(int i=0;i<data.RowList.Count;i++){
            if(data.RowList[i]is Row){
                r=data.RowList[i] as Row;
                if(RemoteButto2.ContainsKey(r)){
                    if(ck&&r.K_System.Enable){
                        RemoteButto2[r].gameObject.SetActive(true);
                        RemoteButto2[r].transform.GetChild(0).GetComponent<TMP_Text>().text=i==data.page?"<b>"+r.name:r.name;
                        if(!r.Complet){
                            ck=false;
                        }
                    }
                    else{
                        RemoteButto2[r].gameObject.SetActive(false);
                    }
                }
            }
            else if(ck){
                t=data.RowList[i] as NPCTalk;
                if(t.Active){
                    ck=false;
                }
            }
        }
    }
}
