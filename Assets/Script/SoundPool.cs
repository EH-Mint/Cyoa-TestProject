using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundPool : ObjectPool<SoundObject>
{
    private void Start() {
        var dt=Resources.Load<DM>("DataManager");
        dt.Reset();
        dt.SetPage();

    }
    [SerializeField]private AudioSource BGM;
    [SerializeField]private Dictionary<string,AudioClip> BgmList;
    private string NowBGM;
    public void SFXPlay(AudioClip clip){
        GetObj().Play(clip);
    }
    public void PlayBGM(string name){
        if (NowBGM==name) return;

        if (BgmList.ContainsKey(name)){
            BGM.clip = BgmList[name];
            BGM.Play();
            NowBGM = name;
        }
    }
}
