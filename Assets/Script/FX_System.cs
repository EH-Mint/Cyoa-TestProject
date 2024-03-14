using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using System;
public enum FXList{
    없음,
    섬광
}
public class FX_System : SerializedMonoBehaviour
{
    [SerializeField]private Dictionary<FXList,Animation> FxAnim; 
    public float Play(FXList type){
        FxAnim[type].gameObject.SetActive(true);
        FxAnim[type].Play("섬광");
        switch(type){
            case FXList.섬광:
            AnimEnd(type,1).Forget();
            return 1f;
            default:
            return 0;
        }
    }
    private async UniTask AnimEnd(FXList type,float time){
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        FxAnim[type].gameObject.SetActive(false);
    }
}
