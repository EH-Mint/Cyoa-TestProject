using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Row : SerializedScriptableObject {
    public bool First;//첫번째 선택지인지
    [Multiline(10)]
    public string RawInfo;//선택지 위에 나올 대사
    public int MinSelect;//필수 선택 숫자
    public int MaxSelect;
    public bool RowType;//4개인지 8개인지
    public List<Choice> RowData;//선택지 데이터
    public KeySystem K_System=new KeySystem();
    public int Last;
    [ReadOnly]public bool Complet;//조건이 완료되었는지
    public int Findchoice(Choice C){
        for(int i=0;i<RowData.Count;i++){
            if(RowData[i]==C)return i;
        }
        return 0;
    }
}
