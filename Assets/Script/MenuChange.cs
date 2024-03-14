using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuChange : MonoBehaviour
{
    [SerializeField]private List<GameObject> Menu;
    [SerializeField]private List<TMP_Text> MenuText;
    [SerializeField]private TMP_Text Title;
    private void OnEnable() {
        Change(0);
    }
    public void Change(int num){
        if(Menu.Count>num){
            for(int i=0;i<Menu.Count;i++){
                if(i==num){
                    Menu[i].SetActive(true);
                    MenuText[i].fontStyle=FontStyles.Bold;
                }
                else{
                    Menu[i].SetActive(false);
                    MenuText[i].fontStyle=FontStyles.Normal;
                }
            }
            Title.text=MenuText[num].text;
        }
    }
}
