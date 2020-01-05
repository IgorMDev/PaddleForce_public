using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleUpEffector : Effector{
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Brick"){
            GetComponent<Animator>().Play("activate");
        }
    }
    public override void ApplyEffector(){
        if(actor != null){
            actor.brick.GetComponent<Animator>().Play("onScaleUp");
            base.ApplyEffector();
        }
    }
    public override void Disable(){
        actor?.brick.GetComponent<Animator>().Play("offScaleUp");
        base.Disable();
    }
}