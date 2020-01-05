using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Effector : PhysicsNode2D{
    public bool stayOnDelimLine = true;
    public float duration = 1;
    public AudioClip spawnAudio, activateAudio;
    protected Actor actor;
    public UnityAction destroyed;
    public bool isApplied{get;set;}
    IEnumerator dur;
    public virtual void SetActor(Actor act){
        actor = act;
    }
    public Actor GetActor(){
        return actor;
    }
    public virtual void Spawn(){
        GetComponent<AudioSource>()?.PlayOneShot(spawnAudio);
        isApplied = false;
    }
    public virtual void ApplyEffector(){
        GetComponent<AudioSource>()?.PlayOneShot(activateAudio);
        StartCoroutine(dur = SetDuration());
        isApplied = true;
    }
    public virtual void Disable(){
        StopAllCoroutines();
        Destroy();
    }
    protected virtual IEnumerator SetDuration(){
        yield return new WaitForSecondsRealtime(duration);
        Disable();
    }
    void GamePause(){
        if(dur != null){
            StopCoroutine(dur);
        }
    }
    void GameResume(){
        if(dur != null){
            StartCoroutine(dur);
        }
    }
    void PreRoundEnd(){
        Disable();
    }
    void GameInterrupt(){
        Disable();
    }
    void Update(){
        if(stayOnDelimLine){
            SetPositionX(Arena.delimLine);
        }
    }
    public void Destroy(){
        destroyed?.Invoke();
        Destroy(gameObject);
    }
}