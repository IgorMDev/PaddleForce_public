using UnityEngine;
using UnityEngine.Events;

public class AppPaused : MonoBehaviour{
    bool isPaused = false, started = false;
    public UnityEvent paused;
    void Start(){
        started = true;
    }
    void OnApplicationFocus(bool hasFocus){
        isPaused = !hasFocus;
        if(started && isPaused){
            paused.Invoke();
        }
    }
    void OnApplicationPause(bool pauseStatus){
        isPaused = pauseStatus;
        if(started && isPaused){
            paused.Invoke();
        }
    }
}