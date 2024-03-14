using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class ChoiceSystem : MonoBehaviour
{
    private int Page;
    private int MaxPage;
    private int select;
    public int Select{
        get{return select;}
        set{
            select=value;
            ButtonActiove();
        }
    }
    public Row row;
    public List<Choice> RowData;
    [SerializeField]private List<ChoiceObject> CObj;
    [SerializeField]private List<ChoiceObject> CObj2;
    [SerializeField]private HorizontalLayoutGroup Choice1;
    [SerializeField]private GridLayoutGroup Choice2;
    [SerializeField]private GameObject ObjGroup;
    [SerializeField]private GameObject ObjGroup2;
    [SerializeField]private TMPro.TMP_Text PageText;
    [SerializeField]private TMPro.TMP_Text MaxChoiceText;
    [SerializeField]private TMPro.TMP_Text MinChoiceText;
    [SerializeField]private TMPro.TMP_Text RowText;//열텍스트
    [SerializeField]private Button Back;//이전 페이지로
    [SerializeField]private Button Front;//다음 페이지로
    [SerializeField]private Button NexButton;//다음 선택지로
    [SerializeField]private Button BackButton;//이전 선택지로
    [SerializeField]private GameObject Select2;
    public int MaxChoice;
    public int maxchoice;
    private int MinChoice;
    private int minchoice;
    private void OnEnable() {
        Select2.SetActive(true);
    }
    private void OnDisable(){
        Select2.SetActive(false);
    }
    public void Reset(){
        Page=0;
        Select=0;
        BackButton.gameObject.SetActive(!row.First);
        RowText.text=row.RawInfo;
        MinChoice=row.MinSelect;
        minchoice=MinChoice;

        MaxChoice=row.MaxSelect;
        maxchoice=MaxChoice;

        ButtonActiove();

        Load();
    }
    public void Load(){
        var data=Resources.Load<DM>("DataManager");
        int TargetChoice=0;
        RowData= row.RowData.Where(X => X.K_System.Enable).ToList();
        if(RowData.Count<4)Choice1.childAlignment=TextAnchor.UpperCenter;
        else Choice1.childAlignment=TextAnchor.UpperLeft;
        if(RowData.Count<8)Choice2.childAlignment=TextAnchor.MiddleCenter;
        else Choice2.childAlignment=TextAnchor.UpperLeft;
        if(row.RowType){
            ObjGroup2.gameObject.SetActive(true);
            ObjGroup.gameObject.SetActive(false);
            for(int i=0;i<8;i++){
                TargetChoice=Page*8+i;
                if(RowData.Count>TargetChoice){
                    CObj2[i].gameObject.SetActive(true);
                    CObj2[i].SetChoice(RowData[TargetChoice],row.Findchoice(RowData[TargetChoice]));
                } 
                else{
                    CObj2[i].gameObject.SetActive(false);
                }
            }
        }
        else{
            ObjGroup2.gameObject.SetActive(false);
            ObjGroup.gameObject.SetActive(true);
            for(int i=0;i<4;i++){
                TargetChoice=Page*4+i;
                if(RowData.Count>TargetChoice){
                    CObj[i].gameObject.SetActive(true);
                    CObj[i].SetChoice(RowData[TargetChoice],row.Findchoice(RowData[TargetChoice]));
                } 
                else{
                    CObj[i].gameObject.SetActive(false);
                }
            }
        }
        Select=0;
        foreach(var x in row.RowData){
            if(x.Active)Select++;
        }
        MaxPage=Mathf.CeilToInt(RowData.Count/(row.RowType?8f:4f))-1;
        
        PageText.text=(Page+1)+"/"+(MaxPage+1);
        Back.gameObject.SetActive(Page!=0);
        Front.gameObject.SetActive(MaxPage>Page);

    }
    public void SetPage(bool Ck){
        if(Ck){
            if(Page<MaxPage){
                Page++;
            }
        }
        else{
            if(Page>0){
                Page--;
            }
        }
        Load();
    }
    private void ButtonActiove(){
        minchoice=MinChoice-Select;
        maxchoice=MaxChoice-Select;
        if(minchoice>0){
            MinChoiceText.text=minchoice.ToString();
            row.Complet=false;
            NexButton.interactable=false;
        }
        else{
            MinChoiceText.text="0";
            row.Complet=true;
            NexButton.interactable=true;
        }
        if(MaxChoice>0){
            MaxChoiceText.text=maxchoice>0?maxchoice.ToString():"0";
        }
        else{
            MaxChoiceText.text="∞";
        }
    }
    public void SetChoice(bool ck){
        if(MaxChoice!=0){
            maxchoice+=ck?1:-1;
        }
        if(MinChoice!=0){
            minchoice+=ck?1:-1;
        }
        ButtonActiove();
    }
    public void LastReset(){
        row.RowData[row.Last].DisActiveChoice();
        row.RowData[row.Last].CalcCost(true);
    }
}
