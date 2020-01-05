using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallbackTrigger : MonoBehaviour{
    public UnityEvent[] zeroArgEvents;
    //public UnityEvent<T>[] oneArgEvents;
    public void DispatchAll(){
        foreach(var ev in zeroArgEvents){
            ev.Invoke();
        }
    }
    public void Dispatch(int index){
        if(index >= 0 && index < zeroArgEvents.Length){
            zeroArgEvents[index].Invoke();
        }
    }
    public void DestroyGO(GameObject go){
        if(go == null){
            go = gameObject;
        }
        Destroy(gameObject);
    }
}