using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseMoverController : Mover2DController{
    Vector2 sp, dp;
    float unitLength;
    protected override void Awake(){
        base.Awake();
        actions = LeftBtnPoint;
        unitLength = Mathf.Clamp(35*(Screen.width/Screen.dpi),150, 400);
        
    }
    void LeftBtn(){
        if(Input.GetMouseButtonDown(0)){
            sp = Input.mousePosition;
        }else if(Input.GetMouseButton(0)){
            dp = (Vector2)Input.mousePosition - sp;
            sp = (Vector2)Input.mousePosition;
            
            mover.MoveBy(dp.normalized,1);
        }
    }
    void LeftBtnPoint(){
        if(Input.GetMouseButtonDown(0)){
            sp = Input.mousePosition;
        }else if(Input.GetMouseButton(0)){
            dp = (Vector2)Input.mousePosition - sp;
            mover.MoveBy(dp, Mathf.Clamp01(dp.magnitude/unitLength));
        }
    }
    void WheelBtn(){
        if(Input.GetMouseButtonDown(1)){
            sp = Input.mousePosition;
        }else if(Input.GetMouseButton(1)){
            dp = (Vector2)Input.mousePosition - sp;
            sp = (Vector2)Input.mousePosition;
            //RotateBy(Mathf.Sign(dp.x),1);
        }
    }
}