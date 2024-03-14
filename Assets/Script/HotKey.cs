using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Sirenix.OdinInspector;

public class HotKey : SerializedMonoBehaviour
{
    public delegate void KeyEvent();
    public Dictionary<List<KeyCode>,KeyEvent> keyevent;
    [SerializeField]private Save SaveSystem;
    private void Start() {
        keyevent=new Dictionary<List<KeyCode>, KeyEvent>(){
                {new List<KeyCode>(){KeyCode.LeftControl,KeyCode.S},new KeyEvent(SaveSystem.SaveData)}
        };
    }
    private void Update() {
        if(Input.anyKeyDown){
            foreach(var X in keyevent){
                foreach(var y in X.Key){
                    if(Input.GetKeyDown(y)){
                        X.Value();
                    }
                }
                
            }
        }
    }
    private void SaveEvent(){
        
    }
}
