using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class AchievementItem : MonoBehaviour{

    public UnityEvent opened, selected, unselected;
    public AchievementType type;
    public bool isUnlocked, isSelected;
    public Image image;
    [HideInInspector]
    public AchievementData saveData;
    void Awake(){
        this.name = image.sprite.name;
        saveData = Prefs.arenaData.achievements.Find((AchievementData data)=>data.type == type);
        if(saveData == null){
            saveData = new AchievementData{type = type, unlocked = isUnlocked};
        }else{
            isUnlocked = saveData.unlocked;
        }
        if(isUnlocked){
            Open();
        }
        if(isSelected){
            Select();
        }
    }
    
    public void Open(){
        isUnlocked = true;
        OnOpened();
    }
    public void Select(){
        OnSelected();
    }
    public void OnSelectBtn(){
        
        OnSelected();
    }
    public void Unselect(){
        isSelected = false;
        unselected.Invoke();
    }
    void OnOpened(){
        
        opened.Invoke();
    }
    void Save(){
        if(saveData != null){
            saveData.type = type;
            saveData.unlocked = isUnlocked;
        }
    }
    void OnSelected(){
        
        isSelected = true;
        selected.Invoke();
    }
    
}