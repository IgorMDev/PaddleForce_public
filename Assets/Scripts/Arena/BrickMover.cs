using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMover : RigidbodyMover2D{
    public Vector2[] forwards;
    public Vector2[] forwardsSight;
    public int curForward;
    new void Start(){
        base.Start();
    }
    public override Vector2 forward{
        get{return base.forward;}
        set{
            SetForwardByAngle(Vector2.Angle(value, Vector2.right));
            base.forward = value;
        }
    }
    public override float forwardRotation{
        get{return base.forwardRotation;}
        set{
            SetForwardByAngle(value);
            base.forwardRotation = value;
        }
    } 
    void SetForwardByAngle(float ang){
        if(ang < forwardsSight[curForward][0]){
            forwardAxis = PreviousForward();
        }else if(ang > forwardsSight[curForward][1]){
            forwardAxis = NextForward();
        }
    }
    Vector2 NextForward(){
        if(++curForward >= forwards.Length){
            curForward = 0;
        }
        return forwards[curForward];
    }
    Vector2 PreviousForward(){
        if(--curForward < 0){
            curForward = forwards.Length - 1;
        }
        return forwards[curForward];
    }
}

