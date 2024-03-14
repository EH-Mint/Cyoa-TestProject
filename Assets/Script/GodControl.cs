using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;
public class GodControl : MonoBehaviour
{
    [SerializeField]private apPortrait Goddess;
    private void RandomPlay() {
        if(Random.Range(0,100)>90){
            if(Random.Range(0,10)>5){
                Goddess.Play("Fly");
                Goddess.PlayQueued("Idle");
            }
            else{
                Goddess.Play("Fly2");
                Goddess.PlayQueued("Idle");
            }
        }
    }

}
