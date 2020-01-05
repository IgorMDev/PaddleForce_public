using UnityEngine;

public class ActorMover : BrickMover{
    public Actor actor;
    BrickMover brickMover{
        get{return actor.brick.GetComponent<BrickMover>();}
    }
    public override Rigidbody2D rb{get{return actor.brick.GetComponent<Rigidbody2D>();}}
    public bool bounded = true;
    protected override void Awake(){
        base.Awake();
        if(actor == null){
            actor = GetComponent<Actor>();
        }
        speed = brickMover.speed;
        angSpeed = brickMover.angSpeed;
    }
    public override Vector2 position{
        get{return brickMover.position;}
        set{
            if(bounded){
                value = CheckBounds(value);
            }
            brickMover.position = value;
        }
    }
    public override float rotation{
        get{return brickMover.rotation;}
        set{brickMover.rotation = value;}
    }
    public override Vector2 forward{
        get{return brickMover.forward;}
        set{brickMover.forward = value;}
    }
    public override float forwardRotation{
        get{return brickMover.forwardRotation;}
        set{brickMover.forwardRotation = value;}
    }
    Vector2 CheckBounds(Vector2 pos){
        if(pos.x < actor.field.min.x){
            pos.x = Mathf.MoveTowards(brickMover.position.x, actor.field.min.x, dt*speed);
        }
        else if(pos.x > actor.field.max.x){
            pos.x = Mathf.MoveTowards(brickMover.position.x, actor.field.max.x, dt*speed);
        }
        return pos;
    }
    void Update(){
        position = position;
    }
}

