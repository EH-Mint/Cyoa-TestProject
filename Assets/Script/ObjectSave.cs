using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSave : MonoBehaviour
{
    public TalkSystem talksystem;//대화시스템
    public ChoiceSystem CyoaSystem;//선택지시스템
    public SoundPool SoundSystem;
    public FX_System fxsystem;//화면효과시스템
    public TMPro.TMP_Text PrayText;//소망텍스트
    public TMPro.TMP_Text DestinyText;//운명텍스트
    public GameObject SkipButton;
    private void Awake() {
        Resources.Load<DM>("DataManager").save=this;
    }
}
