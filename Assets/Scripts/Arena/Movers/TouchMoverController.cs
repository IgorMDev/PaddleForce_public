using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Mover2D))]
public class TouchMoverController : Mover2DController{
    TouchData t1,t2;
    int[] localTouchesId = new int[2]{-1,-1};
    Vector2 dp;
    float xMin, xMax;
    public float unitLength;
    new void Awake(){
        base.Awake();
        Input.multiTouchEnabled = true;
        xMin = 0;
        xMax = Screen.width;
        actions = FirstMoveSecondRotate;
    }
    void Start(){
        if(unitLength == 0){
            unitLength = (mover.speed/500) * Mathf.Clamp(Screen.dpi, 50, 1000)/3f;
        }
    }
    private void Update() {
        int lt = 0, i = 0;
        foreach(var t in MultitouchHandler.touches){
            if(lt < localTouchesId.Length){
                localTouchesId[i++] = -1;
                if(t.beginPos.x >= xMin && t.beginPos.x < xMax){
                    localTouchesId[lt++] = t.id;
                }
            }
        }
    }
    public void FirstMoveSecondRotate(){
        if(MultitouchHandler.touchCount >= 1 && localTouchesId[0] > -1){
            t1 = MultitouchHandler.touches[localTouchesId[0]];
            if(t1.touch.phase == TouchPhase.Moved || t1.touch.phase == TouchPhase.Stationary){
                dp = t1.pos - t1.beginPos;
                if(dp.magnitude > t1.touch.radius)
                    mover.MoveBy(dp, Mathf.Clamp(dp.magnitude/unitLength, 0, 1)); 
            }
        }
        if(MultitouchHandler.touchCount >= 2 && localTouchesId[1] > -1){
            t2 = MultitouchHandler.touches[localTouchesId[1]];
            if(t2.touch.phase == TouchPhase.Moved || t2.touch.phase == TouchPhase.Stationary){
                dp = t2.pos - t2.beginPos;
                if(dp.magnitude > t2.touch.radius)
                    mover.RotateBy(LargestComponent(dp), Mathf.Clamp(dp.magnitude/unitLength, 0, 1));
            }
        }
    }
    public void SetLeftHalf(){
        xMax = Screen.width/2;
    }
    public void SetRightHalf(){
        xMin = Screen.width/2;
    }
    float LargestComponent(Vector2 v){
        return Mathf.Abs(v.x) >= Mathf.Abs(v.y) ? v.x : v.y;
    }
}