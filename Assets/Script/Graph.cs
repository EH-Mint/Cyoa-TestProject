using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
[ExecuteInEditMode]
public class Graph : MonoBehaviour
{
    public LineRenderer lineRenderer; // LineRenderer 컴포넌트를 참조
    [SerializeField]private float[] values = { 2, 3, 4, 5, 1 }; // 각 변수의 값
    [SerializeField]private float Scaler;
    [SerializeField]private float radius = 100; // 그래프의 반지름
    [SerializeField]private float Angle;
    [SerializeField]private List<TMP_Text> Graphtext;
    [SerializeField]private List<TMP_Text> GraphTitletext;
    [SerializeField]private TMP_InputField nameinput;
    private int numPoints = 6; // 점의 수 (변수의 수 + 1, 시작점으로 돌아오기 때문)
    [SerializeField]private AnimationCurve curve;
    private void OnEnable() {
        DrawRadarChart();
    }
    [Button("그래프생성")]
    public void DrawRadarChart()
    {
        var dm=Resources.Load<DM>("DataManager");
        nameinput.text=dm.NameSave;
        values[0]=CalculateValue(dm.Power,2000);
        values[1]=CalculateValue(dm.Talk,2000);
        values[2]=CalculateValue(dm.HP,2000);
        values[3]=CalculateValue(dm.Mental,2000);
        values[4]=CalculateValue(dm.MP,2000);
        lineRenderer.positionCount = numPoints; // 선의 점 수 설정
        float angleIncrement = 360f / (numPoints - 1); // 각도 증분 계산

        for (int i = 0; i < numPoints; i++)
        {
            float currentAngle = (i * angleIncrement)+Angle; // 현재 각도
            Vector3 pointPosition = Vector3.zero; // 점의 위치 초기화

            if (i < values.Length)
            {
                // 값을 사용하여 점 위치 계산
                pointPosition = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * values[i] * radius;
            }
            else
            {
                // 마지막 점은 첫 번째 점과 같아야 하므로, 첫 번째 점의 위치를 다시 계산
                float firstAngle = Angle;
                pointPosition = new Vector3(Mathf.Cos(firstAngle * Mathf.Deg2Rad), Mathf.Sin(firstAngle * Mathf.Deg2Rad), 0) * values[0] * radius;
            }

            lineRenderer.SetPosition(i, pointPosition); // LineRenderer에 점의 위치 설정
        }
        Graphtext[0].text=settext(PointType.전투력);
        Graphtext[1].text=settext(PointType.사교성);
        Graphtext[2].text=settext(PointType.내구성);
        Graphtext[3].text=settext(PointType.정신력);
        Graphtext[4].text=settext(PointType.마력);
        GraphTitletext[0].text="전투력"+((Mathf.FloorToInt(values[0])==0)?1:Mathf.FloorToInt(values[0]));
        GraphTitletext[1].text="사교성"+((Mathf.FloorToInt(values[1])==0)?1:Mathf.FloorToInt(values[1]));
        GraphTitletext[2].text="내구성"+((Mathf.FloorToInt(values[2])==0)?1:Mathf.FloorToInt(values[2]));
        GraphTitletext[3].text="정신력"+((Mathf.FloorToInt(values[3])==0)?1:Mathf.FloorToInt(values[3]));
        GraphTitletext[4].text="마력"+((Mathf.FloorToInt(values[4])==0)?1:Mathf.FloorToInt(values[4]));
    }
    private string settext(PointType type){
        switch(type){
            case PointType.전투력:
                switch(Mathf.FloorToInt(values[0])){
                    case 0:
                    case 1:
                    return "일반인 수준의 전투력입니다.<br>마수와 전투는 피하는걸 권장합니다.";
                    case 2:
                    return "총기로 무장한 수준의 전투력입니다.<br>저급 마수 정도는 어렵지 않게 처리할 수 있습니다.";
                    case 3:
                    return "건물 하나쯤은 가볍게 날려버릴 수준의 전투력입니다.<br>당신에게 대부분의 마수는 위협이 되지 않습니다.";
                    case 4:
                    return "군대 수준의 전투력입니다.<br>고위 마수와 대등하게 전투가 가능합니다.";
                    case 5:
                    return "국가와 동등한 수준의 전투력입니다.<br>당신을 막을 수 있는것은 없습니다.";
                }
            break;
            case PointType.내구성:
            switch(Mathf.FloorToInt(values[2])){
                    case 0:
                    case 1:
                    return "일반인 수준의 내구성입니다.";
                    case 2:
                    return "갑옷을 입은 수준의 내구성입니다.<br>날붙이 정도는 버틸 수 있습니다.";
                    case 3:
                    return "평범한 마법소녀의 내구성입니다.<br>소총탄 정도는 버틸 수 있습니다.";
                    case 4:
                    return "상당한 수준의 내구성입니다<br>미사일 조차 막아낼 수 있습니다.";
                    case 5:
                    return "불멸에 가깝습니다.<br>전략핵을 제외하면 제거할 수단이 존재하지 않습니다";
                }
            break;
            case PointType.마력:
                switch(Mathf.FloorToInt(values[4])){
                    case 0:
                    case 1:
                    return "일반인 수준의 마력입니다.";
                    case 2:
                    return "대부분의 마법사나 재능있는 일반인이 지니고있는 수준의 마력입니다.";
                    case 3:
                    return "평범한 마법소녀 수준의 마력입니다.";
                    case 4:
                    return "필멸자의 한계에 도달한 마력입니다.";
                    case 5:
                    return "통상적인 마법소녀의 영역을 벗어난 마력입니다.<br>정상적인 상태가 아니므로 과도하게 사용할 경우 신체에 손상이 올 수 있습니다.";
                }
            break;
            case PointType.사교성:
                switch(Mathf.FloorToInt(values[1])){
                    case 0:
                    case 1:
                    return "누군가 먼져 말을 걸어주지 않으면 대화조차 성립하지 않을 수준의 사교성입니다.";
                    case 2:
                    return "아싸라고 불릴 수준의 사교성으로써 사람을 대하는게 어렵습니다.";
                    case 3:
                    return "평범한 수준의 사교성입니다.";
                    case 4:
                    return "어딜 가더라도 사람들과 쉽게 친해질 수 있으며 사람들은 당신을 좋은 사람이라 생각할것입니다.";
                    case 5:
                    return "처음 보는 사람과 30분만에 절친이 될 수 있을 수준의 사교성입니다.";
                }
            break;
            case PointType.정신력:
                switch(Mathf.FloorToInt(values[3])){
                    case 0:
                    case 1:
                    return "당신의 정신상태는 언제 무너질지 모를 정도로 위태롭습니다.";
                    case 2:
                    return "당신의 정신력은 상당히 약합니다.<br>정신계열 공격에 취약합니다.";
                    case 3:
                    return "평범한 일반인 수준의 정신력입니다.";
                    case 4:
                    return "강인한 정신력입니다.<br>정신계열 공격에 저항할 수 있습니다.";
                    case 5:
                    return "불굴의 정신력입니다.<br>압도적인 재앙을 만나더라도 꺾이지 않을 수 있습니다.";
                }
            break;
        }
        return null;
    }
    private float CalculateValue(float x,float y)
    {
        curve.keys[1].time=y;
        return curve.Evaluate(x);
    }
}
