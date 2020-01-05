using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor;

public class MessageTrigger :MonoBehaviour{
    [Serializable]
    public class Entry{
        public MessageTriggerType eventID = MessageTriggerType.Start;
        public UnityEvent callback = new UnityEvent();
    }
    [SerializeField]
    private List<Entry> m_Delegates;
    public List<Entry> triggers{
        get{
            if (m_Delegates == null)
                m_Delegates = new List<Entry>();
            return m_Delegates;
        }
        set{m_Delegates = value;}
    }
    private void Execute(MessageTriggerType id){
        for (int i = 0, imax = triggers.Count; i < imax; ++i){
            var ent = triggers[i];
            if (ent.eventID == id && ent.callback != null)
                ent.callback.Invoke();
        }
    }
    private void Awake() {
        Execute(MessageTriggerType.Awake);
    }
    private void OnEnable() {
        Execute(MessageTriggerType.OnEnable);
    }
    private void Start() {
        Execute(MessageTriggerType.Start);
    }
    private void OnApplicationFocus(bool focusStatus) {
        Execute(MessageTriggerType.ApplicationFocus);
    }
    private void OnApplicationPause(bool pauseStatus) {
        Execute(MessageTriggerType.ApplicationPause);
    }
    private void OnApplicationQuit() {
        Execute(MessageTriggerType.ApplicationQuit);
    }
    private void OnDisable() {
        Execute(MessageTriggerType.OnDisable);
    }
    private void OnDestroy() {
        Execute(MessageTriggerType.OnDestroy);
    }
}
public enum MessageTriggerType{
    Awake, 
    OnEnable, 
    Start,
    ApplicationPause, ApplicationFocus, 
    ApplicationQuit,
    OnDisable, 
    OnDestroy,

}