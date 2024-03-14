using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
public enum PointType{
    소망,
    운명,
    전투력,
    사교성,
    내구성,
    마력,
    정신력,
    
}
public enum ChoiceGroup{
    세계관,
    커스텀,
    마법,
    필살기,
    HUD,
    마법지식,
    신체개조,
    거래,
    제약해제,
    대화선택지,
    신분,
    거주지,
    소원,
    마스코트,

}
public struct Point{
    public PointType Type;//포인트종류
    public int Cost;//비용
}
public class Choice : SerializedScriptableObject
{
    [HideLabel]
    [HorizontalGroup("Split1")]
    [PreviewField(100, ObjectFieldAlignment.Left)]
    [VerticalGroup("Split1/Left")]
    [HorizontalGroup("Split1/Left/Split", 100, LabelWidth = 60)]
    public Texture Img;
    [VerticalGroup("Split1/Left/Split/Right")]
    [LabelText("선택지이름")]
    [ReadOnly]
    public string Name;
    [VerticalGroup("Split1/Left/Split/Right")]
    [LabelText("비용")]
    public List<Point> Cost;
    [VerticalGroup("Split1/Left/Split/Right")]
    [LabelText("그룹")]
    public ChoiceGroup C_Group;
    [VerticalGroup("Split1/Left/Split/Right")]
    [LabelText("포인트표시")]
    public bool ViewPoint;
    [VerticalGroup("Split1/Left/Split/Right")]
    [LabelText("조건비용")]
    public Dictionary<Choice,List<Point>> CCost;
    
    [HorizontalGroup("Split2")]
    [VerticalGroup("Split2/Left")]
    [HorizontalGroup("Split2/Left/Split", 100, LabelWidth = 60)]
    [HideLabel]
    [PreviewField(100, ObjectFieldAlignment.Left)]
    public Sprite Front;
    [VerticalGroup("Split2/Left/Split/Right")]
    [LabelText("비활성화시숨김")]
    public bool DisActive;
    
    [VerticalGroup("Split2/Left/Split/Right")]
    [LabelText("표시이름")]
    public string ViewName;
    [VerticalGroup("Split2/Left/Split/Right")]
    [LabelText("변경이름")]
    public string ChangeName;
    [VerticalGroup("Split2/Left/Split/Right")]
    [LabelText("폰트")]
    public Material FontMaterial;
    
    [Multiline(10)]
    [HorizontalGroup("Split3")]
    [VerticalGroup("Split3/Left")]
    [LabelText("설명")]
    public string Info;
    [Multiline(10)]
    [VerticalGroup("Split3/Left")]
    [LabelText("변경설명")]
    public string ChangeInfo;
    [VerticalGroup("Split3/Left")]
    [LabelText("낙장불입")]
    public bool NoneSelect;
    [ReadOnly]public bool Active;//선택되었는지
    public KeySystem K_System=new KeySystem();
    public void ActiveChoice(){
        Active=true;
        if(K_System.ConnectKey!=null){
            foreach(var x in K_System.ConnectKey){
                if(x is Row){
                    (x as Row).K_System.KeyReload();
                }
                else if(x is NPCTalk){
                    (x as NPCTalk).K_System.KeyReload();
                }
                else{
                    (x as Choice).K_System.KeyReload();
                }
            }
        }
        Choice a;
        if(K_System.BenKey!=null){
            foreach(var x in K_System.BenKey){
                if(x is Row){
                    (x as Row).K_System.AddBenKey(this);
                }
                else if(x is NPCTalk){
                    (x as NPCTalk).K_System.AddBenKey(this);
                }
                else{
                    a=x as Choice;
                    a.K_System.AddBenKey(this);
                    if(a.Active){
                        a.DisActiveChoice();
                    }
                }
            }
        }
    }
    public void DisActiveChoice(){
        Choice a;
        Active=false;
        if(K_System.ConnectKey!=null){
            foreach(var x in K_System.ConnectKey){
                if(x is Row){
                    (x as Row).K_System.KeyReload();
                }
                else if(x is NPCTalk){
                    (x as NPCTalk).K_System.KeyReload();
                }
                else{
                    a=x as Choice;
                    a.K_System.KeyReload();
                    if(a.Active&&!a.NoneSelect){
                        a.CalcCost(true);
                        a.DisActiveChoice();
                    }
                }
            }
        }
        if(K_System.BenKey!=null){
            foreach(var x in K_System.BenKey){
                if(x is Row){
                    (x as Row).K_System.RemoveBenKey(this);
                }
                else if(x is NPCTalk){
                    (x as NPCTalk).K_System.RemoveBenKey(this);
                }
                else{
                    (x as Choice).K_System.RemoveBenKey(this);
                }
            }
        }
        
    }
    public void CalcCost(bool ck){
        if(Cost!=null){
            foreach(var x in Cost){
                CostSet(x,ck);
            }  
        }
        if(CCost!=null){
            foreach(var x in CCost){
                if(x.Key.Active){
                    foreach(var y in x.Value){
                        CostSet(y,ck);
                    }
                }
            }
        }
    }
    private void CostSet(Point pt,bool ck){
        switch(pt.Type){
            case PointType.운명:
            Resources.Load<DM>("DataManager").Destiny-=ck?(pt.Cost*-1):pt.Cost;
            break;
            case PointType.소망:
            Resources.Load<DM>("DataManager").Pray-=ck?(pt.Cost*-1):pt.Cost;
            break;
            case PointType.전투력:
            Resources.Load<DM>("DataManager").Power-=ck?(pt.Cost*-1):pt.Cost;
            break;
            case PointType.마력:
            Resources.Load<DM>("DataManager").MP-=ck?(pt.Cost*-1):pt.Cost;
            break;
            case PointType.내구성:
            Resources.Load<DM>("DataManager").HP-=ck?(pt.Cost*-1):pt.Cost;
            break;
            case PointType.정신력:
            Resources.Load<DM>("DataManager").Mental-=ck?(pt.Cost*-1):pt.Cost;
            break;
            case PointType.사교성:
            Resources.Load<DM>("DataManager").Talk-=ck?(pt.Cost*-1):pt.Cost;
            break;
        }
    }
}
