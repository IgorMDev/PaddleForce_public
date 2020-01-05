using UnityEngine;
using UnityEngine.Events;

public class Mover2DController : MonoBehaviour{
    public Mover2D mover;
    public bool movePhysics;
    protected UnityAction actions;
    protected virtual void Awake(){
        if(mover == null){
            mover = GetComponent<Mover2D>();
            if(mover is RigidbodyMover2D) movePhysics = true;
        }
    }
    private void FixedUpdate(){
        if(movePhysics) actions?.Invoke();;
    }
    void Update(){
        if(!movePhysics) actions?.Invoke();
    }
}