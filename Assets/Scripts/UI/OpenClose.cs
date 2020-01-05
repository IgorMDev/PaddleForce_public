using UnityEngine;
using UnityEngine.Events;

public class OpenClose : MonoBehaviour {
    public UnityEvent open, close;
    public bool isOpen;
    public void Open(){
        if(!isOpen){
            open.Invoke();
            isOpen = true;
        }
    }
    public void Close(){
        if(isOpen){
            close.Invoke();
            isOpen = false;
        }
    }
    public void Toggle(){
        if(isOpen) Close();
        else Open();
    }
}