using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class ProfilManager : SerializedMonoBehaviour
{
   [SerializeField]private List<ProfilCard> card;
   [SerializeField]private Dictionary<Row,ProfilCell>cell;
   private void OnEnable() {
        foreach(var x in card){
            x.Setting();
        }
        foreach(var x in cell){
            for(int i=0;i<x.Value.transform.childCount;i++){
                x.Value.transform.GetChild(i).gameObject.SetActive(false);
            }
            
        }
        foreach(var x in cell){
            foreach(var y in x.Key.RowData){
                if(y.Active)x.Value.GetObj().Setting(y);
            }
        }
   }
}
