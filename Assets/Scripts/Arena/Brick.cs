using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class Brick : PhysicsNode2D{
    public UnityAction<Ball> collidedWithBall;
    public UnityAction<Effector> collidedWithEffector;
    public float brickImpulse, ballImpulse, strikeBound;
    [HideInInspector]
    public UnityEvent spawned, wasted;
    protected override void Awake(){
        base.Awake();
    }
    public Brick Instantiate(string brickName){
        if(name == brickName){
            return this;
        }
        Brick b = Resources.Load<Brick>(Prefs.arenaData.location + "/Bricks/" + brickName);
        Brick nb = null;
        if(b != null){
            nb = Instantiate<Brick>(b, b.defaultPosition, Quaternion.identity, transform.parent);
            nb.name = b.name;
            nb.collidedWithBall = collidedWithBall;
            nb.collidedWithEffector = collidedWithEffector;
            nb.wasted = wasted;
            nb.spawned = spawned;
            Destroy(gameObject);
        }
        if(nb == null){
            nb = this;
        }
        return nb;
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Ball"){
            if(!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
            collidedWithBall.Invoke(col.gameObject.GetComponent<Ball>());
        }else if(col.gameObject.tag == "Effector"){
            collidedWithEffector.Invoke(col.gameObject.GetComponent<Effector>());
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Effector"){
            collidedWithEffector.Invoke(col.gameObject.GetComponent<Effector>());
        }
    }
    void OnCollisionExit2D(Collision2D col){
        if(col.gameObject.tag == "Ball" && ballImpulse != 0){
            Vector2 fdir = col.gameObject.GetComponent<Rigidbody2D>().velocity;
            col.rigidbody.AddRelativeForce(fdir * ballImpulse, ForceMode2D.Impulse);
        }
    }
    void OnSpawned(){
        spawned.Invoke();
    }
    void OnWasted(){
        wasted.Invoke();
    }
    void GameSleep(){
        GetComponent<Collider2D>().enabled = false;
    }
    void GameWakeUp(){
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Animator>().Play("brickWakeUp");
    }
    public void Spawn(){
        SetRotation(0);
        localPosition = defaultPosition;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Animator>().Play("brickSpawn");
    }
    public void Waste(){
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().Play("brickWaste");
    }
    public void Win(){
        Reset();
    }
    public void Reset(){
        transform.DOLocalMove(defaultPosition, 1);
        rb.DORotate(0, 1);
    }
    public Vector2 defaultPosition{
        get{return new Vector2(200, 0);}
    }
    public void SetDefaultState(){
        GetComponent<Animator>().Play("idle");
    }
}
