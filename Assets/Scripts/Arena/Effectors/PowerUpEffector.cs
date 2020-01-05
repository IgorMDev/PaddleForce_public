using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpEffector : Effector{
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Brick"){
            GetComponent<Animator>().Play("activate");
        }
    }
    public override void ApplyEffector(){
        if(actor != null){
            actor.energy.AddEnergy();
            base.ApplyEffector();
        }
    }
}