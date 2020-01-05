using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleColorBtn : ToggleBtn{
    public Color onColor, offColor;
    public override void SetOn(){
        GetComponent<Image>().color = onColor;
        base.SetOn();
    }
    public override void SetOff(){
        GetComponent<Image>().color = offColor;
        base.SetOff();
    }
}