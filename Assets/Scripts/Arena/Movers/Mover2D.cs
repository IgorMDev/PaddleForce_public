using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mover2D : MonoBehaviour{
    public static float timeScale = 1;
    public float speed, angSpeed;
    public Vector2 forwardAxis = Vector2.right;
    protected Vector2 forwardVector;
    protected float forwardAngle;
    protected virtual void Awake(){
        
    }
    protected virtual void Start(){
        forward = forwardAxis;
    }
    public virtual Vector2 position{
        get{return (Vector2)transform.position;}
        set{
            transform.position = value;
        }
    }
    public virtual float rotation{
        get{return transform.eulerAngles.z;}
        set{
            transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,value);
        }
    }
    public virtual Vector2 forward{
        get{return forwardVector;}
        set{
            forwardVector = value;
            forwardAngle = Vector2.SignedAngle(forwardVector, forwardAxis);
        }
    }
    public virtual float forwardRotation{
        get{return forwardAngle;}
        set{
            forwardVector = new Vector2(Mathf.Cos(Mathf.Deg2Rad*value), Mathf.Sin(Mathf.Deg2Rad*value));
            forwardAngle = Vector2.SignedAngle(forwardVector, forwardAxis);;
        }
    }
    protected virtual float dt{
        get{return Time.deltaTime*timeScale;}
    }
    public virtual void MoveTo(Vector2 p, float s = 1){
        position = Vector2.MoveTowards(position, p, speed*dt*s);
    }
    public virtual void MoveRawTo(Vector2 p, float s = 1){
        position = Vector2.MoveTowards(position, p, speed*s);
    }
    public virtual void MoveBy(Vector2 dp, float s = 1){
        if(dp.magnitude > Mathf.Epsilon){
            MoveTo(position+dp,s);
        }
    }
    public virtual void MoveRawBy(Vector2 dp, float s = 1){
        if(dp.magnitude > Mathf.Epsilon){
            MoveRawTo(position+dp,s);
        }
    }
    public virtual void MoveBySpeed(Vector2 dp){
        if(dp.magnitude > Mathf.Epsilon){
            position += dp*speed*dt;
        }
    }
    public virtual void RotateTo(float a, float s = 1){
        rotation = Mathf.MoveTowards(rotation, a, angSpeed*dt*s);
        forwardRotation = a;
    }
    public virtual void RotateForwardTo(float a, float s = 1){
        rotation = Mathf.MoveTowards(rotation, forwardRotation = a, angSpeed*dt*s);
    }
    public virtual void RotateRawTo(float a, float s = 1){
        rotation = Mathf.MoveTowards(rotation, a, angSpeed*s);
        forwardRotation = a;
    }
    public virtual void RotateBy(float da, float s = 1){
        if(da != 0){
            RotateTo(rotation+da,s);
        }
    }
    public virtual void RotateRawBy(float da, float s = 1){
        if(da != 0){
            RotateRawTo(rotation+da,s);
        }
    }    
    public virtual void RotateBySpeed(float u = 1){
        if(u != 0){
            rotation += angSpeed*u*dt;
            forwardRotation = rotation;
        }
    }
    public virtual void LookAt(Vector2 p){
        forward = (p-position).normalized;
        rotation = Mathf.MoveTowardsAngle(rotation, forwardRotation, angSpeed*dt);
    }

}
 public static class Vector2Extension {
     public static Vector2 Rotate(this Vector2 v, float degrees) {
         float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
         float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         float tx = v.x, ty = v.y;
         v.x = (cos * tx) - (sin * ty);
         v.y = (sin * tx) + (cos * ty);
         return v;
     }
     public static float LargestComponent(this Vector2 v){
         return Mathf.Abs(v.y) > Mathf.Abs(v.x) ? v.y : v.x;
     }
     public static bool SimilarDirection(this Vector2 v1, Vector2 v2){
        if(v1.magnitude == 0 || v2.magnitude == 0){
            return false;
        }
        return Vector2.Dot(v1.normalized, v2.normalized*2) > 0;
    }
 }
