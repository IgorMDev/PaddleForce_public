using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : PhysicsNode2D{
    [Range(0,1)]
    public float growPoints;
    public UnityAction<Ball> spawned, instantiated;
    public Vector2 defaultPosition{
        get{return Vector2.zero;}
    }
    protected override void Awake(){
        base.Awake();
    }
    public Ball Instantiate(string ballName){
        if(name == ballName){
            return this;
        }
        Ball b = Resources.Load<Ball>(Prefs.arenaData.location + "/Balls/" + ballName);
        Ball nb = null;
        if(b != null){
            nb = Instantiate<Ball>(b, b.defaultPosition, Quaternion.identity, transform.parent);
            nb.name = b.name;
            nb.spawned = spawned;
            nb.instantiated = instantiated;
            Destroy(gameObject);
        }
        if(nb == null){
            nb = this;
        }
        return nb;
    }
    void GameSleep(){
        Sleep();
    }
    void GameWakeUp(){
        WakeUp();
    }
    void GameInterrupt(){
        Sleep();
    }
    void LateUpdate(){
        Arena.isActive = velocity.magnitude > 0;
    }
    public void Sleep(){
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<TrailRenderer>().emitting = false;
    }
    public void WakeUp(){
        GetComponent<Collider2D>().enabled = true;
        GetComponent<TrailRenderer>().emitting = true;
    }
    void GameSpawn(){
        velocity = Vector2.zero;
        position = defaultPosition;
        GetComponent<Animator>().Play("spawn");
        spawned?.Invoke(this);
    }
    void PreRoundEnd(){
        velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<TrailRenderer>().emitting = false;
        GetComponent<Animator>().Play("destroy");
    }
    public void SetTrailColor(Color c){
        GetComponent<TrailRenderer>().startColor = c;
    }
}
