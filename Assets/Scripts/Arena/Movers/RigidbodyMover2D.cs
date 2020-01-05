using UnityEngine;
public class RigidbodyMover2D : Mover2D{
    public virtual Rigidbody2D rb{get{return GetComponent<Rigidbody2D>();}}
    protected override void Awake(){
        base.Awake();
    }
    public override Vector2 position{
        get{return rb.position;}
        set{rb.MovePosition(value);}
    }
    public override float rotation{
        get{return rb.rotation;}
        set{
            rb.MoveRotation(value%360);
        }
    }
    protected override float dt{
        get{return Time.fixedDeltaTime*Mover2D.timeScale;}
    }
    public virtual Vector2 velocity{
        get{return rb.velocity;}
        set{rb.velocity = value*Mover2D.timeScale;}
    }
}