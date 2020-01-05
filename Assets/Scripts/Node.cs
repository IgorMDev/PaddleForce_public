using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Node2D : MonoBehaviour{
    
    
    public virtual Vector2 position{
        get{return transform.position;}
        set{transform.position = value;}
    }
    public virtual Vector2 localPosition{
        get{return transform.localPosition;}
        set{transform.localPosition = value;}
    }
    public virtual Vector2 scale{
        get{return transform.localScale;}
        set{transform.localScale = value;}
    }
    public virtual Vector3 rotation3d{
        get{return transform.eulerAngles;}
        set{transform.rotation = Quaternion.Euler(value);}
    }
    public virtual Vector3 localRotation3d{
        get{return transform.localEulerAngles;}
        set{transform.localRotation = Quaternion.Euler(value);}
    }
    public virtual float rotation{
        get{return transform.eulerAngles.z;}
        set{transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,value);}
    }
    public virtual float localRotation{
        get{return transform.localEulerAngles.z;}
        set{transform.localRotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,value);}
    }
    public virtual Node2D parent{
        get{
            return transform.parent ?.GetComponent<Node2D>();
        }
        set{transform.SetParent(value.transform);}
    }
    public virtual Transform parentTransform{
        get{
            return transform.parent;
        }
        set{transform.SetParent(value,false);}
    }

    public virtual Sprite sprite{
        get{return GetComponent<SpriteRenderer>().sprite;}
        set{
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if(sr != null){
                sr.sprite = value;
            }
        }
    }
    public new virtual SpriteRenderer renderer{
        get{return GetComponent<SpriteRenderer>();}
        set{
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if(sr == null){
                sr = AddComponent<SpriteRenderer>() as SpriteRenderer;
            }
            sr = value;
        }
    }
    public virtual Bounds bounds{
        get{return renderer.bounds;}
    }
    public virtual Rect rect{
        get{return GetComponent<RectTransform>().rect;}
    }

    public virtual Rect GetRect(){
        RectTransform rt = GetComponent<RectTransform>();
        Rect r;
        if(rt == null){
            r = new Rect(position-scale/2,scale);
        }else{
            r = rt.rect;
        }
        return r;
    }
    
    public virtual void SetPositionX(float p){
        transform.position = new Vector3(p, transform.position.y, transform.position.z);
    }
    public virtual void SetPositionY(float p){
        transform.position = new Vector3(transform.position.x, p, transform.position.z);
    }
    public virtual void SetPositionZ(float p){
        transform.position = new Vector3(transform.position.x, transform.position.y, p);
    }
    public virtual void SetLocalPositionX(float p){
        transform.localPosition = new Vector3(p, transform.localPosition.y, transform.localPosition.z);
    }
    public virtual void SetLocalPositionY(float p){
        transform.localPosition = new Vector3(transform.localPosition.x, p, transform.localPosition.z);
    }
    public virtual void SetLocaPositionZ(float p){
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, p);
    }
    public virtual void SetScaleX(float s){
        transform.localScale = new Vector3(s, transform.localScale.y, transform.localScale.z);
    }
    public virtual void SetScaleY(float s){
        transform.localScale = new Vector3(transform.localScale.x, s, transform.localScale.z);
    }
    public virtual void SetRotationX(float a){
        transform.Rotate(Vector3.right, a-transform.localEulerAngles.x);
    }
    public virtual void SetRotationY(float a){
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, a, transform.localEulerAngles.z);
    }
    public virtual void SetRotation(float a){
        transform.Rotate(Vector3.forward, a-transform.localEulerAngles.z);
    }
    public virtual float GetRotationX(){
        return transform.localEulerAngles.x;
    }
    public virtual float GetRotationY(){
        return transform.localEulerAngles.y;
    }
    public virtual float GetRotation(){
        return transform.localEulerAngles.z;
    }    
    public virtual void Translate(Vector2 dp){
        transform.Translate(dp);
    }
    public virtual void RotateX(float da){
        transform.Rotate(Vector3.right, da);
    }
    public virtual void RotateY(float da){
        transform.Rotate(Vector3.up, da);
    }
    public virtual void Rotate(float da){
        transform.Rotate(Vector3.forward, da);
    }
    public virtual void LookAt(Vector2 f, Vector2 p){
        transform.LookAt(p, f);
    }
    public void FlipX(){
        SetScaleX(-scale.x);
    }
    public void FlipY(){
        SetScaleY(-scale.y);
    }
    public Component AddComponent<T>(){
        return gameObject.AddComponent(typeof(T));
    }
}
public class PhysicsNode2D : Node2D{
    public Rigidbody2D rb{get;set;}
    protected virtual void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    public override Vector2 position{
        get{return rb.position;}
        set{rb.position = value;}
    }
    public override float rotation{
        get{return rb.rotation;}
        set{rb.rotation = value;}
    }
    public override void SetPositionX(float x){
        rb.position = new Vector2(x, rb.position.y);
    }
    public override void SetRotation(float a){
        rb.rotation = a;
    }
    public virtual Vector2 velocity{
        get{return rb.velocity;}
        set{rb.velocity = value;}
    }
    
    public virtual Bounds colliderBounds{
        get{return GetComponent<Collider2D>().bounds;}
    }
    public override void LookAt(Vector2 f, Vector2 p){
        rb.MoveRotation(Vector2.Angle(p, f));
    }
}





