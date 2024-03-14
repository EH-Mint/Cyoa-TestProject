using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPool<T> :SerializedMonoBehaviour{
    protected Stack<T> ObjectStack=new Stack<T>();
    public GameObject Prefap;
    private void Start() {
        for(int i=0;i<transform.childCount;i++){
            ObjectStack.Push(transform.GetChild(i).GetComponent<T>());
        }
    }
    public virtual T GetObj(){
        if(ObjectStack.Count>0){
            
            return ObjectStack.Pop();
        }
        else{
            var copy=Instantiate(Prefap);
            copy.transform.SetParent(transform);
            copy.transform.localScale=Vector3.one;
            return copy.GetComponent<T>();
        }
    }
    public virtual void ReturnObj(T obj){
        if(!ObjectStack.Contains(obj))ObjectStack.Push(obj);
    }
}