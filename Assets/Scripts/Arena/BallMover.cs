using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : RigidbodyMover2D{
    public float minSpeed;
    protected override void Awake(){
        base.Awake();
    }
    void FixedUpdate(){
        if(velocity.magnitude > speed){
            velocity = Vector2.ClampMagnitude(velocity, speed);
        }else if(velocity.magnitude < minSpeed){
            velocity = velocity.normalized * minSpeed;
        }
    }
}