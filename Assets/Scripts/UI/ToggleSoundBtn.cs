using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleSoundBtn : ToggleBtn{
    protected override void Start(){
        if(Prefs.options.soundOn){
            SetOn();
        }else{
            SetOff();
        }
    }
    public override void SetOn(){
        AudioListener.volume = 1;
        Prefs.options.soundOn = true;
        base.SetOn();
    }
    public override void SetOff(){
        AudioListener.volume = 0;
        Prefs.options.soundOn = false;
        base.SetOff();
    }
}