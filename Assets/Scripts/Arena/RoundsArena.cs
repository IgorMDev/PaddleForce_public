using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RoundsArena : MonoBehaviour{
    public Actor player1, player2;
    public Ball ball;
    public Text mainText, upperText, comboText;
    public RectTransform leftTouchArea, rightTouchArea;
    public AudioClip roundOnAudio, gameEndAudio;
    public UnityEvent GameStarted, GameEnded, RoundStarted, RoundEnded;
    Arena arena;
    int roundsPlayed;
    bool isGameEnd, missAdOneRound;
    ArenaData saveData;
    VideoAd ad;
    void Awake(){
        arena = Arena.GetInstance();
        saveData = Prefs.arenaData;
        player1.Instantiate(saveData.actor1Data);
        player2.Instantiate(saveData.actor2Data);
        
        Arena.LeftActor = player1;
        Arena.RightActor = player2;
        InstantiateBall();
        player1.waste = OnActorWaste;
        player2.waste = OnActorWaste;
        player1.wasted = OnActorWasted;
        player2.wasted = OnActorWasted;
        player1.onCombo = ComboOn;
        player2.onCombo = ComboOn;
        ad = GetComponent<VideoAd>();
        ad.ended.AddListener(OnAdEnded);
    }
    void Start(){
        if(saveData.actor1Data.controls == ControlType.Manual && saveData.actor2Data.controls == ControlType.Manual){
            player1.GetComponent<TouchMoverController>()?.SetLeftHalf();
            player2.GetComponent<TouchMoverController>()?.SetRightHalf();
            player2.GetComponent<KeyboardMoverController>()?.SetAxes("Horizontal2","Vertical2","Rotation2");
        }
        if(saveData.actor1Data.controls == ControlType.Manual && saveData.actor2Data.controls == ControlType.Bot){
            player1.canUnlockAchievements = true;
        }else if(saveData.actor2Data.controls == ControlType.Manual && saveData.actor1Data.controls == ControlType.Bot){
            player2.canUnlockAchievements = true;
        }
        arena.StartGame();
    }
    public void GameStart(){
        isGameEnd = false;
        if(Prefs.options.musicOn){
            GameController.instance?.MusicOff();
        }
        ResetScore();
        arena.Sleep();
        GameOn();
        GameStarted.Invoke();
        arena.StartRound();
    }
    public void RoundStart(){
        arena.Spawn();
        RoundOn();
        RoundStarted.Invoke();
    }
    void GamePause(){
        if(Prefs.options.musicOn){
            GameController.instance?.MusicOn();
        }
    }
    void GameResume(){
        if(Prefs.options.musicOn){
            GameController.instance?.MusicOff();
        }
    }
    public void PreRoundEnd(){
        roundsPlayed++;
        arena.Sleep();
    }
    public void RoundEnd(){
        RoundEnded.Invoke();
        if(!missAdOneRound && ad.canShow){
            ad.Show();
        }else{
            if(isGameEnd){
                arena.EndGame();
            }else{
                arena.StartRound();
                missAdOneRound = false;
            }
        }
    }
    public void GameEnd(){
        GetComponent<AudioSource>().PlayOneShot(gameEndAudio);
        if(Prefs.options.musicOn){
            GameController.instance?.MusicOn();
        }
        GameEnded.Invoke();
    }
    public void OnActorWaste(Actor loser){
        arena.PreEndRound();
    }
    public void OnActorWasted(Actor loser){
        if(loser.energy.isEmpty){
            isGameEnd = true;
        }
        arena.EndRound();
    }
    void OnAdEnded(){
        if(Arena.isPaused){
            GameObject.Find("ResumeBtn").GetComponent<Button>().onClick.Invoke();
        }
        if(isGameEnd){
            arena.EndGame();
        }else{
            arena.StartRound();
            missAdOneRound = true;
        }
    }
    void InstantiateBall(){
        ball = ball.Instantiate(saveData.ballName);
        Arena.Ball = ball;
        ball.instantiated?.Invoke(ball);
    }
    void ResetScore(){
        roundsPlayed = 0;
    }
    void GameOn(){
        //mainText.text = "Game on";
        mainText.GetComponent<Animation>().Play();
    }
    void RoundOn(){
        
        upperText.text = "";
        GetComponent<AudioSource>().PlayOneShot(roundOnAudio);
        upperText.GetComponent<Animation>().Play();
    }
    void ComboOn(string info, Vector2 pos){
        comboText.text = info;
        pos.x = pos.x - 24*Mathf.Sign(pos.x);
        comboText.transform.position = pos;
        comboText.GetComponent<Animation>().Play();
    }
    
}


