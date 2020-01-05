using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ToggleBtn : MonoBehaviour{
    public bool selfOn = true;
    public UnityEvent On;
    public bool selfOff = true;
    public UnityEvent Off;
    public bool isOn = true;
    protected virtual void Awake(){
        GetComponent<Button>().onClick.AddListener(Clicked);
        
    }
    protected virtual void Start(){
        if(isOn){
            SetOn();
        }
    }
    protected virtual void Clicked(){
        if(!isOn && selfOn) SetOn();
        else if(isOn && selfOff) SetOff();
    }
    public virtual void SetOn(){
        isOn = true;
        On.Invoke();
    }
    public virtual void SetOff(){
        isOn = false;
        Off.Invoke();
    }
}