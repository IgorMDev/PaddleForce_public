using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Actor : Node2D{
    public Field field;
    public GoalTrigger goalTrigger;
    public Brick brick;
    public Actor enemy;
    public ActorEnergy energy;
    public UnityAction<Actor> wasted, spawned, waste;
    public UnityAction<string, Vector2> onCombo;
    public bool isFocused, isSpawned, canUnlockAchievements;
    [HideInInspector]
    public short goalsScored, roundsWon, goalCombo, goalsMissed, roundsWonInRow, maxGoalCombo, roundsNoMiss;
    [HideInInspector]
    public float minRatioInfo;
    public float areaRatio{
        get{return field.areaRatio;}
    }
    public Color color{
        get{return field.renderer.color;}
        set{
            field.color = value;
            goalTrigger.renderer.color = value;
        }
    }
    new Rect rect{
        get{return Arena.rect;}
    }
    void Awake(){
        brick.collidedWithBall = OnCollidedWithBall;
        brick.collidedWithEffector = OnCollidedWithEffector;
        brick.wasted.AddListener(OnWasted);
        brick.spawned.AddListener(OnSpawned);
        goalTrigger.onEnter.AddListener(OnGoal);
        field.growedUp.AddListener(OnFieldGrowedUp);
        field.growedDown.AddListener(OnFieldGrowedDown);
    }
    void Start(){
        GetComponent<RectTransform>().sizeDelta = Arena.rect.size;
    }
    public void Instantiate(ActorData d){
        if(d.colorName != ""){
            SetColor(d.colorName);
        }
        if(d.brickName != ""){
            SetBrick(d.brickName);
        }
        if(d.controls != ControlType.None){
            SetControls(d.controls);
        }
    }
    void SetControls(ControlType t){
        foreach(var el in GetComponents<Mover2DController>()){
            Destroy(el);
        }
        switch(t){
            case ControlType.Manual:
                AddComponent<KeyboardMoverController>();
                #if UNITY_ANDROID
                AddComponent<TouchMoverController>();
                #endif
                break;
            case ControlType.Bot:
                AddComponent<BotActor>();
                break;
        }
    }
    public void SetBrick(string bname){
        brick = brick.Instantiate(bname);
    }
    public void SetColor(string cname){
        color = ColorPalette.GetColorByName(cname);
    }
    void GameStart(){
        ResetScores();
        energy.Reset();
    }
    void GameSleep(){
        Sleep();
    }
    void GameWakeUp(){
        WakeUp();
    }
    public void Sleep(){
        foreach(var el in GetComponents<Mover2DController>()){
            el.enabled = false;
        }
    }
    public void WakeUp(){
        foreach(var el in GetComponents<Mover2DController>()){
            el.enabled = true;
        }
    }
    public void Waste(){
        isSpawned = false;
        energy.PopEnergy();
        roundsWonInRow = 0;
        waste.Invoke(this);
    }
    public void Wasted(){
        brick.Waste();
    }
    public void Win(){
        Sleep();
        roundsWon++;
        roundsWonInRow++;
    }
    public void Won(){
        brick.Win();
    }
    void GameSpawn(){
        if(isSpawned){
            field.Reset();
            brick.Reset();
            
        }else{
            field.Reset();
            brick.Spawn();
            isSpawned = true;
        }
        energy.ShowEnergy();
    }
    void GamePause(){
        energy.ShowUp();
    }
    void GameResume(){
        energy.ShowDown();
    }
    void RoundStart(){
        roundsNoMiss++;
        goalCombo = 0;
        UnFocus();
    }
    void PreRoundEnd(){
        UnFocus();
        Sleep();
    }
    void GameEnd(){
        if(canUnlockAchievements){
            Prefs.arenaData.playedMatches++;
            if(roundsWon > enemy.roundsWon){
                Prefs.arenaData.wonMatches++;
            }
            AchievementUnlocker.CheckActorGameAchievements(this);
        }
    }
    void GameInterrupt(){
        isSpawned = false;
        UnFocus();
        Sleep();
    }
    public void OnCollidedWithBall(Ball b){
        b.SetTrailColor(color);
        Focus();
    }
    public void OnCollidedWithEffector(Effector eff){
        eff.SetActor(this);
    }
    public void Focus(){
        if(!isFocused){
            enemy.UnFocus();
            goalTrigger.Show();
            isFocused = true;
        }
    }
    public void UnFocus(){
        if(isFocused){
            goalTrigger.Hide();
            isFocused = false;
        }
    }
    public void OnGoal(Ball b){
        if(areaRatio < 1-float.Epsilon){
            field.GrowByRatio(b.growPoints);
            goalsScored++;
            goalCombo++;
            if(goalCombo > maxGoalCombo){
                maxGoalCombo = goalCombo;
            }
            if(goalCombo > 1){
                onCombo?.Invoke(goalCombo+"X", b.position);
            }
            OnGrowUp();
        }
        enemy.OnMiss(b);
        
    }
    public void OnMiss(Ball b){
        if(areaRatio > float.Epsilon){
            field.GrowByRatio(-b.growPoints);
            goalCombo = 0;
            roundsNoMiss = 0;
            goalsMissed++;
            OnGrowDown();
        }
        
    }
    public void SetAreaRatio(float ratio){
        if(areaRatio > 0 && areaRatio < 1){
            float or = areaRatio;
            field.GrowToRatio(ratio);
            enemy.field.GrowToRatio(1 - ratio);
            if(or < areaRatio){
                OnGrowUp();
            }else if(or > areaRatio){
                OnGrowDown();
            }
        }
    }
    public void MoveFieldTo(float px){
        if(areaRatio > 0 && areaRatio < 1){
            float ox = field.areaPx;
            field.GrowTo(transform.InverseTransformPoint(px,0,0).x);
            enemy.field.GrowTo(enemy.transform.InverseTransformPoint(px,0,0).x);
            if(ox < field.areaPx){
                OnGrowUp();
            }else if(ox > field.areaPx){
                OnGrowDown();
            }
        }
    }
    void OnGrowDown(){
        if(minRatioInfo > areaRatio){
            minRatioInfo = areaRatio;
        }
        if(areaRatio <= Prefs.maxPrecise){
            Waste();
        }else if(field.areaPx < brick.sprite.bounds.extents.x){
            SetAreaRatio(0);
        }
    }
    void OnGrowUp(){
        if(areaRatio >= 1-Prefs.maxPrecise){
            Win();
        }
    }
    public void OnFieldGrowedDown(){
        if(areaRatio <= Prefs.maxPrecise){
            Wasted();
        }
    }
    public void OnFieldGrowedUp(){
        if(areaRatio >= 1-Prefs.maxPrecise){
            Won();
        }
    }
    void ResetScores(){
        roundsWon = goalsScored = goalCombo = maxGoalCombo = goalsMissed = roundsWonInRow = 0;
        minRatioInfo = 1;
    }
    void OnWasted(){
        wasted?.Invoke(this);
    }
    void OnSpawned(){
        spawned?.Invoke(this);
    }
}


[System.Serializable]
public class GoalEvent : UnityEvent<Ball>{}