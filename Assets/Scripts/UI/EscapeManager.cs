using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscapeManager : MonoBehaviour{
    public static EscapeManager instance;
    public List<UnityEvent> eventStack = new List<UnityEvent>();
    
    void Awake(){
        if(instance == null){
            instance = this;
        }else if (instance != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(eventStack.Count > 0){
                Debug.Log("escape btn pressed");
                eventStack[eventStack.Count - 1].Invoke();
            }
        }
    }
    
}