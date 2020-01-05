using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArenaConfigurator : MonoBehaviour{
    
    public ActorConfigurator actor1, actor2;
    public Image ball;
    public AchievementMenu ballsMenu;
    public ArenaData saveData;
    void Awake(){
        saveData = Prefs.arenaData;
        actor1.saveData = saveData.actor1Data;
        actor2.saveData = saveData.actor2Data;
        AchievementUnlocker.CheckPlayedMatches();
        AchievementUnlocker.CheckWonMatches();
    }
    void Start(){
        RestoreState();
        ball.SetNativeSize();
    }
    void RestoreState(){
        if(saveData != null){ 
            if(saveData.ballName != ""){
                Image b = ballsMenu.SelectItemImage(saveData.ballName);
                if(b != null){
                    ball.sprite = b.sprite;
                }
            }
            if(actor1.colorName == ""){
                actor1.colorName = ColorPalette.GetRandomColorName();
            }else{
                actor1.colorName = actor1.colorName;
            }
            if(actor2.colorName == ""){
                actor2.colorName = ColorPalette.GetRandomColorNameExcept(actor1.colorName);
            }else{
                actor2.colorName = actor2.colorName;
            }
        }
    }
    public void SetNextColor(ActorConfigurator ac){
        if(ac == actor1){
            actor1.colorName = ColorPalette.GetNextColorNameExcept(actor1.colorName, actor2.colorName);
        }else if(ac == actor2){
            actor2.colorName = ColorPalette.GetNextColorNameExcept(actor2.colorName, actor1.colorName);
        }
    }
    void SaveState(){
        saveData.ballName = ball.sprite.name;
    }
    void OnDestroy(){
        SaveState();
    }
}