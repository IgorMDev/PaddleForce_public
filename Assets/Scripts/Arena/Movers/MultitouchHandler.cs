using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultitouchHandler : MonoBehaviour{
    public int maxTouches;
    public static MultitouchHandler instance;
    public static List<TouchData> touches;
    public static int touchCount => touches.Count;
    private void Awake(){
        if(instance == null){
            instance = this;
        }else if (instance != this){
            Destroy(gameObject);
        }
        touches = new List<TouchData>(maxTouches);
    }
    private void Update(){
        CheckTouchesBegin();
        UpdateTouches();
    }
    void CheckTouchesBegin(){
        for(int t = 0; t < maxTouches && t < Input.touchCount; t++){
            if(Input.touches[t].phase == TouchPhase.Began){
                touches.Add(new TouchData{touch = Input.touches[t], beginPos = Input.touches[t].position});
            }
        }
    }
    void UpdateTouches(){
        for(int t = 0; t < touches.Count; ){
            if(Input.GetTouch(t).phase == TouchPhase.Ended){
                touches.RemoveAt(t);
            }else{
                touches[t].touch = Input.GetTouch(t);
                if(touches[t].touch.deltaPosition.magnitude > 10 && 
                !touches[t].touch.deltaPosition.SimilarDirection(touches[t].deltaBegin)){
                    touches[t].beginPos = touches[t].pos;
                }
                t++;
            }
        }
    }
}
public class TouchData{
    public Touch touch;
    public Vector2 beginPos;
    public Vector2 pos => touch.position;
    public int id => touch.fingerId;
    public Vector2 deltaBegin => touch.position - beginPos;
}