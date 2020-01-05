using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombEffector : Effector{
    public AudioClip hitAudio;
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Brick"){
            stayOnDelimLine = false;
            GetComponent<AudioSource>().PlayOneShot(hitAudio);
            GetComponent<Animator>().SetTrigger("Activate");
        }
    }
    public override void ApplyEffector(){
        if(actor != null){
            rb.velocity = Vector2.zero;
            actor.MoveFieldTo(position.x);
            base.ApplyEffector();
        }
    }
}