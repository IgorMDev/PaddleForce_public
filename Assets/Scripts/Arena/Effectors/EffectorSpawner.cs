using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EffectorSpawner : MonoBehaviour{
    public Effector[] effectors;
    public UnityAction<Effector> instantiated;
    public float probability, minInterval;
    Effector currentEffector;
    IEnumerator spawnRoutine;
    Type lastEffector = typeof(Effector);
    bool isRunning;
    void RoundStart(){
        if(Arena.LeftActor.energy.isFull && Arena.RightActor.energy.isFull){
            DisableEffectorSpawn(typeof(PowerUpEffector));
        }else{
            EnableEffectorSpawn(typeof(PowerUpEffector));
        }
        spawnRoutine = null;
        RunSpawner();
    }
    void PreRoundEnd(){
        StopSpawner();
    }
    void GamePause(){
        StopSpawner();
    }
    void GameResume(){
        RunSpawner();
    }
    void GameInterrupt(){
        StopSpawner();
    }
    void InstantiateRandomEffector(){
        if(UnityEngine.Random.value < probability){
            if(!Arena.LeftActor.energy.isFull || !Arena.RightActor.energy.isFull){
                EnableEffectorSpawn(typeof(PowerUpEffector));
            }else{
                DisableEffectorSpawn(typeof(PowerUpEffector));
            }
            Effector eff;
            do{
                eff = effectors[UnityEngine.Random.Range(0, effectors.Length)];

            }while(!eff.enabled || lastEffector == eff.GetType());
            float ypos = UnityEngine.Random.Range(-280, 280);
            lastEffector = eff.GetType();
            currentEffector = Instantiate<Effector>(eff, new Vector2(Arena.delimLine, ypos), Quaternion.identity, transform);
            currentEffector.destroyed = OnEffectorDestroyed;
            Arena.Effector = currentEffector;
        }
    }
    public void DisableEffectorSpawn(System.Type t){
        foreach(var el in effectors){
            if(el.GetType() == t){
                el.enabled = false;
            } 
        }
    }
    public void EnableEffectorSpawn(System.Type t){
        foreach(var el in effectors){
            if(el.GetType() == t){
                el.enabled = true;
            } 
        }
    }
    void OnEffectorDestroyed(){
        currentEffector = null;
    }
    void RunSpawner(){
        if(!isRunning){
            if(spawnRoutine == null){
                StartCoroutine(spawnRoutine = Spawner());
            }else{
                StartCoroutine(spawnRoutine);
            }
            isRunning = true;
        }
    }
    void StopSpawner(){
        if(isRunning){
            if(spawnRoutine != null) StopCoroutine(spawnRoutine);
            isRunning = false;
        }
    }
    void DisableAllEffectors(){
        if(currentEffector != null) currentEffector.Destroy();
    }
    IEnumerator Spawner(){
        while(true){
            yield return new WaitForSecondsRealtime(minInterval);
            if(currentEffector == null && Arena.isActive){
                Debug.Log("eff instantiated");
                InstantiateRandomEffector();
            }
        }
    }
}