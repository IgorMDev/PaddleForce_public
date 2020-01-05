using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ToggleSpriteBtn : ToggleBtn{
    public Sprite onSprite,offSprite;
    public override void SetOn(){
        GetComponent<Image>().sprite = onSprite;
        base.SetOn();
    }
    public override void SetOff(){
        GetComponent<Image>().sprite = offSprite;
        base.SetOff();
    }
}