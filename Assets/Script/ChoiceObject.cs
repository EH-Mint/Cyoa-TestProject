using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceObject : MonoBehaviour
{
    [SerializeField]private TMPro.TMP_Text Name;//이름
    [SerializeField]private TMPro.TMP_Text Info;//설명
    [SerializeField]private TMPro.TMP_Text Cost;//비용
    [SerializeField]private RawImage Img;//이미지
    [SerializeField]private Image Front;//테두리
    [SerializeField]private ChoiceSystem CSystem;//선택지시스템
    private Choice choice;
    [SerializeField]private Button Bt;
    [SerializeField]private int numsave;
    public void SetChoice(Choice C, int num){
        numsave=num;
        choice=C;
        Name.fontMaterial=C.FontMaterial;
        if(Cost)Cost.fontMaterial=C.FontMaterial;
        if(Info)Info.fontMaterial=C.FontMaterial;
        if(Cost){
            if(choice==null){
                Cost.text="";
            }
            else if(!choice.ViewPoint)
            {
                if(choice.Cost!=null){
                    if(choice.Cost[0].Cost>=0){
                        Cost.text="비용:"+choice.Cost[0].Cost+choice.Cost[0].Type.ToString();
                    }
                    else{
                        Cost.text="획득:"+Mathf.Abs(choice.Cost[0].Cost)+choice.Cost[0].Type.ToString();
                    }
                }
                else{
                    Cost.text="";
                }
            }
        }
        Img.texture=choice.Img;
        Front.sprite=choice.Front;
        if(choice.Active){
            if(Info)Info.text=choice.ChangeInfo==""? choice.Info:choice.ChangeInfo;
            Name.text=choice.ChangeName==""? (choice.ViewName==""? choice.Name:choice.ViewName):choice.ChangeName;
            Front.color=new Color(184f/255f,234f/255f,1);
        }
        else{
            if(Info)Info.text=choice.Info;
            Name.text=choice.ViewName==""? choice.Name:choice.ViewName;
            Front.color=Color.white;
        }
    }
    public void Click(bool Reset){
        if(!choice.Active){
            if(Info)Info.text=choice.ChangeInfo==""? choice.ChangeInfo:choice.Info;
            if(CSystem.MaxChoice!=0&&CSystem.maxchoice<=0&&CSystem.row.Last!=numsave){
                CSystem.LastReset();
            }
            choice.ActiveChoice();
            choice.CalcCost(false);
            CSystem.SetChoice(false);
        }
        else{
            if(choice.NoneSelect)return;
            if(Info)Info.text=choice.Info;
            choice.DisActiveChoice();
            choice.CalcCost(true);
            CSystem.SetChoice(true);
        }
        if(Reset){
            CSystem.Load();
            CSystem.row.Last=numsave;
        }
    }
}
