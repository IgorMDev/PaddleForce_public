using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleMusicBtn : ToggleBtn{
    protected override void Start(){
        if(Prefs.options.musicOn){
            base.SetOn();
        }else{
            base.SetOff();
        }
    }
    public override void SetOn(){
        GameController.instance?.MusicOn();
        Prefs.options.musicOn = true;
        base.SetOn();
    }
    public override void SetOff(){
        GameController.instance?.MusicOff();
        Prefs.options.musicOn = false;
        base.SetOff();
    }
}