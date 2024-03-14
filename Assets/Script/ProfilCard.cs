using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilCard : MonoBehaviour
{
    [SerializeField]private RawImage Img;
    [SerializeField]private TMP_Text Title;
    [SerializeField]private List<Choice> co;
    [SerializeField]private bool returncheck;
    public void Setting() {
        int Num=-1;
        for(int i=0;i<co.Count;i++){
            if(co[i].Active){
                Num=i;
            }
        }
        if(Num!=-1){
            gameObject.SetActive(true);
            Img.texture=co[Num].Img;
            Title.text=co[Num].ChangeName==""? (co[Num].ViewName==""? co[Num].Name:co[Num].ViewName):co[Num].ChangeName;
        }
        else{
            gameObject.SetActive(false);
        }
    }
    public void Setting(Choice Co) {
        co=new List<Choice>();
        co.Add(Co);
        Setting();
    }
    private void OnDisable() {
        if(returncheck){
            transform.parent.GetComponent<ProfilCell>().ReturnObj(this);
        }
    }
}