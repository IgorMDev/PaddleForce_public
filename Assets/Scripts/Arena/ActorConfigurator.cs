using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActorConfigurator : MonoBehaviour{
    public Image brick, field;
    public AchievementMenu bricksMenu;
    public ControlType defaultControlType;
    public ActorConfigurator oppActor;
    public UnityEvent onBot, onManual;
    [HideInInspector]
    public ActorData saveData;
    public Image[] actorColorImages;
    public string colorName{
        get{
            return saveData.colorName;
        }
        set{
            saveData.colorName = value;
            field.color = ColorPalette.GetColorByName(saveData.colorName);
        }
    }
    void Start(){
        RestoreState();
        brick.SetNativeSize();
    }
    void RestoreState(){
        if(saveData != null){ 
            if(saveData.brickName != ""){
                Image b = bricksMenu.SelectItemImage(saveData.brickName);
                if(b != null){
                    brick.sprite = b.sprite;
                }
            }
            if(saveData.controls != ControlType.None){
                if(saveData.controls == ControlType.Bot){
                    OnBotControl();
                }else{
                    OnManualControl();
                }
            }else{
                if(defaultControlType == ControlType.Bot){
                    OnBotControl();
                }else{
                    OnManualControl();
                }
            }
            
        }
    }
    public void Open(){
        bricksMenu.OpenMenuFor(brick);
        foreach(var el in actorColorImages){
            el.color = field.color;
        }
    }
    void SaveState(){
        saveData.brickName = brick.sprite.name;
        
    }
    public void SetManualControl(){
        if(saveData.controls != ControlType.Manual){
            
            OnManualControl();
        }
    }
    public void SetBotControl(){
        if(saveData.controls != ControlType.Bot){
            
            OnBotControl();
        }
    }
    void OnManualControl(){
        saveData.controls = ControlType.Manual;
        onManual.Invoke();
    }
    void OnBotControl(){
        saveData.controls = ControlType.Bot;
        Image b = bricksMenu.GetRandomUnlockedItemImage();
        if(b != null){
            brick.sprite = b.sprite;
        }
        if(oppActor.saveData.controls == ControlType.Bot){
            oppActor.SetManualControl();
        }
        onBot.Invoke();
    }
    void OnDestroy(){
        SaveState();
    }

}