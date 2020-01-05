using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class EscapeBtn : MonoBehaviour{
    public UnityEvent onActive, onDeactive, onEscape;
    public bool useBtn, useEnDis = false;
    bool activated = false;
    void Awake(){
        if(useBtn){
            onEscape.AddListener(GetComponent<Button>().onClick.Invoke);
        }
    }
    void OnEnable(){
        if(useEnDis) Activate();
    }
    void OnDisable(){
        if(useEnDis) Deactivate();
    }
    public void Activate(){
        if(!activated) EscapeManager.instance?.eventStack.Add(onEscape);
        activated = true;
        onActive?.Invoke();
    }
    public void Deactivate(){
        if(activated) EscapeManager.instance?.eventStack.Remove(onEscape);
        activated = false;
        onDeactive?.Invoke();
    }
    public void Escape(){
        onEscape?.Invoke();
    }
}