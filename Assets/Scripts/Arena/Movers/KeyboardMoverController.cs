using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeyboardMoverController : Mover2DController{
    Vector2 sp1;
    public string hAxis = "Horizontal",vAxis = "Vertical",rAxis = "Rotation";
    new void Awake(){
        base.Awake();
        actions = UpdateState;
    }
    public void UpdateState(){
        sp1.Set(Input.GetAxis(hAxis),Input.GetAxis(vAxis));
        mover.MoveBySpeed(sp1);
        mover.RotateBySpeed(Input.GetAxis(rAxis));
    }
    public void SetAxes(string h, string v, string r){
        hAxis = h;
        vAxis = v;
        rAxis = r;
    }
}