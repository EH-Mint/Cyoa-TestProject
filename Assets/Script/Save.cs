using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using Newtonsoft.Json;
using System;
using AOT; // Ahead Of Time 컴파일을 위한 네임스페이스
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif
public class MonoPInvokeCallbackAttribute : Attribute
{
    public MonoPInvokeCallbackAttribute(Type type) { }
}

public enum SaveView{
    프로필,
    외모,
    마법,
    HUD와신체개조,

}
public class Save : MonoBehaviour
{
    private struct SaveStruct{
        public Dictionary<string,bool>ChoiceActiveCheck;
        public Dictionary<string,bool>TalkCheck;    
    }
    [SerializeField]private Camera cam;
    [SerializeField]private MenuChange page;
    [SerializeField]private GameObject EtcMenu;
    public List<GameObject> DisActiveObject;
    [DllImport("__Internal")]
    private static extern void LoadJSONFile(Action<string> callback);
    [DllImport("__Internal")]
    private static extern void DownloadFile(byte[] fileData, int fileDataSize, string fileName);
    public void CaptureAndSaveImage()
    {
        ObjSet(false);
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 32);
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false); // png 파일에 쓰일 재료 

        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = tex.EncodeToPNG();

        DownloadFile(bytes, bytes.Length, "capturedImage.png");
        ObjSet(true);
        Camera.main.targetTexture=null;
    }
    public void TotalCapture(){
        ObjSet(false);
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 32);
        cam.targetTexture = rt;
        List<Texture2D> screenShots=new List<Texture2D>();
        Texture2D screenShot;
        for(int i=6;i>0;i--){
            page.Change(i);
            cam.Render();
            RenderTexture.active = rt;
            screenShot= new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false); // png 파일에 쓰일 재료 
            screenShot.ReadPixels(new Rect(0,0,rt.width, rt.height),0,0);
            screenShots.Add(screenShot);
        }
        byte[] bytes = CombineImagesVertically(screenShots).EncodeToPNG();

        DownloadFile(bytes, bytes.Length, "capturedImage.png");
        ObjSet(true);
        Camera.main.targetTexture=null;
    }
    Texture2D CombineImagesVertically(List<Texture2D> images)
    {
        // 이미지들의 총 너비와 높이를 계산합니다.
        int totalWidth = images.Max(image => image.width);
        int totalHeight = images.Sum(image => image.height);

        // 새 Texture2D 객체를 생성합니다.
        Texture2D result = new Texture2D(totalWidth, totalHeight);

        int currentHeight = 0;
        foreach (var image in images)
        {
            // 현재 이미지의 픽셀 데이터를 가져옵니다.
            Color[] pixels = image.GetPixels();
            // 픽셀 데이터를 새 이미지에 복사합니다.
            result.SetPixels(0, currentHeight, image.width, image.height, pixels);
            currentHeight += image.height;
        }

        // 픽셀 데이터의 변경 사항을 적용합니다.
        result.Apply();
    return result;
    }
    private void ObjSet(bool state){
        for(int i=0;i<DisActiveObject.Count;i++){
            DisActiveObject[i].SetActive(state);
        }
    }
    public void SaveData(){
        var dm=Resources.Load<DM>("DataManager");
        SaveStruct sdata=new SaveStruct();
        sdata.ChoiceActiveCheck=new Dictionary<string, bool>();
        foreach(var x in dm.ChoiceList){
            sdata.ChoiceActiveCheck.Add(x.name,x.Active);
        }
        sdata.TalkCheck=new Dictionary<string, bool>();
        foreach(var x in from x in dm.RowList where x is NPCTalk select x as NPCTalk){
            sdata.TalkCheck.Add(x.name,x.Active);
        }
        #if UNITY_EDITOR
            File.WriteAllText(Application.dataPath+"/Save.json", JsonConvert.SerializeObject(sdata)); 
        #else
            byte[] bytes=System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(sdata));
            DownloadFile(bytes, bytes.Length, "MagicalGirlSave.json");
        #endif
    }
    // 파일 데이터를 받는 메서드

    // 사용자가 파일 선택을 위한 UI 요소를 클릭했을 때 호출
    public void OnFileSelectButtonClick()
    {
        #if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Load JSON File", "", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string jsonContents = File.ReadAllText(path);
            OnFileRead(jsonContents);
        }
        #else
        LoadJSONFile(OnFileRead);
        #endif
        EtcMenu.SetActive(false);
    }
    [MonoPInvokeCallback(typeof(Action<string>))]
    public static void OnFileRead(string jsonContents)
    {
        var dm=Resources.Load<DM>("DataManager");
        dm.Reset();
        var sdata=JsonConvert.DeserializeObject<SaveStruct>(jsonContents);
        Debug.Log($"Data loaded using Json.NET: {sdata.ChoiceActiveCheck}");
        Dictionary<string, Choice> co=new Dictionary<string, Choice>();
        Dictionary<string, NPCTalk> to=new Dictionary<string, NPCTalk>();
        foreach(var x in dm.ChoiceList){
            co.Add(x.name,x);
        }
        foreach(var x in from x in dm.RowList where x is NPCTalk select x as NPCTalk){
            to.Add(x.name,x);
        }
        foreach(var x in sdata.ChoiceActiveCheck){
            if(co.ContainsKey(x.Key)){
                if(x.Value){
                   co[x.Key].ActiveChoice();
                   co[x.Key].CalcCost(false);
                }
                else{
                    co[x.Key].Active=false;
                }
            }
        }
        foreach(var x in sdata.TalkCheck){
            if(to.ContainsKey(x.Key))to[x.Key].Active=x.Value;
        }
        int num;
        foreach(var x in from x in dm.RowList where x is Row select x as Row){
            if(x.MinSelect!=0){
                num=0;
                foreach(var y in x.RowData){
                    if(y.Active){
                        num++;
                        x.Last=x.Findchoice(y);
                    }
                }
                x.Complet=num>=x.MinSelect;
            }
        }
        dm.SetPage();
    }
}
