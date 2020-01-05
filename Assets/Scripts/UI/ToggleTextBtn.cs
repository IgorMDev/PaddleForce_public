using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleTextBtn : ToggleBtn{
    public Text text;
    public string textOn, textOff;
    
    public override void SetOn(){
        text.text = textOn;
        base.SetOn();
    }
    public override void SetOff(){
        text.text = textOff;
        base.SetOff();
    }
}