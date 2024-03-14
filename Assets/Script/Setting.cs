using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField]private AudioMixer Master;
    [SerializeField]private Slider SFXSlider;
    [SerializeField]private TMP_Text SFXText;
    [SerializeField]private Slider BGMSlider;
    [SerializeField]private TMP_Text BGMText;
    
    private void OnEnable() {
        float Volume;
        Master.GetFloat("SFX",out Volume);
        SFXSlider.value=Volume;
        SFXText.text=SetText(Volume);
        Master.GetFloat("BGM",out Volume);
        BGMSlider.value=Volume;
        BGMText.text=SetText(Volume);
    }
    public void SetSFX(){
        var Volume=SFXSlider.value;
        if(Volume==-80f)Master.SetFloat("SFX",-80);
        else Master.SetFloat("SFX",Volume);
        SFXText.text=SetText(Volume);
    }
    public void SetBGM(){
        var Volume=BGMSlider.value;
        if(Volume==-80f)Master.SetFloat("BGM",-80);
        else Master.SetFloat("BGM",Volume);
        BGMText.text=SetText(Volume);
    }
    private string SetText(float num){
        return Mathf.RoundToInt(2.5f * num + 100)+"%";
    }
}
