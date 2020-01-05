using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Arena : Node2D{
    protected static Arena instance;
    public static Actor LeftActor, RightActor;
    public static Ball Ball;
    public static Effector Effector;
    public static bool isActive, isPaused;
    public new static Rect rect;
    public static float delimLine{
        get{return LeftActor.field.position.x;}
    }
    void Awake(){
        instance = this;
        DOTween.Init(true, true);
    }
    void Start(){
        rect = GetComponent<RectTransform>().rect;
        Time.timeScale = 1;
    }
    public void StartGame(){
        BroadcastMessage("GameStart");
    }
    public void StartRound(){
        UnityEngine.Random.InitState(DateTime.Now.Second);
        BroadcastMessage("RoundStart");
    }
    public void PreEndRound(){
        BroadcastMessage("PreRoundEnd");
    }
    public void EndRound(){
        BroadcastMessage("RoundEnd");
    }
    public void EndGame(){
        BroadcastMessage("GameEnd");
    }
    public void Restart(){
        Interrupt();
        Time.timeScale = 1;
        StartGame();
    }
    public void Interrupt(){
        BroadcastMessage("GameInterrupt");
    }
    public void Pause(){
        BroadcastMessage("GamePause",SendMessageOptions.DontRequireReceiver);
        Time.timeScale = 0;
        isPaused = true;
    }
    public void Resume(){
        BroadcastMessage("GameResume",SendMessageOptions.DontRequireReceiver);
        Time.timeScale = 1;
        isPaused = false;
    }
    public void Sleep(){
        BroadcastMessage("GameSleep");
    }
    public void WakeUp(){
        BroadcastMessage("GameWakeUp");
    }
    public void Reset(){
        BroadcastMessage("GameReset");
    }
    public void Spawn(){
        BroadcastMessage("GameSpawn");
    } 
    public static Arena GetInstance(){
        return instance;
    }
    void OnDestroy(){
        Time.timeScale = 1;
    }
}




