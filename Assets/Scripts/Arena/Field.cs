using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Field : Node2D{
    public float growTime = 0.5f;
    float ratio = 0.5f, px;
    public float areaRatio{
        get{return ratio;}
        set{
            ratio = Mathf.Clamp(value, 0, 1);
            px = Arena.rect.width*ratio;
        }
    }
    public float areaPx{
        get{return px;}
        set{
            px = Mathf.Clamp(value, 0, Arena.rect.width);
            ratio = px/Arena.rect.width;
        }
    }
    public Color color{
        get{return renderer.color;}
        set{
            if(ps != null){
                var psmain = ps.main;
                psmain.startColor = value;
            }
            renderer.color = value;
        }
    }
    public UnityEvent growedUp, growedDown;
    public Vector2 min{get{return renderer.bounds.min;}}
    public Vector2 max{get{return renderer.bounds.max;}}
    public ParticleSystem ps;
    void Start(){
        renderer.size = Arena.rect.size;
    }
    public void GrowToRatio(float r){
        float or = areaRatio;
        areaRatio = r;
        if(r > or){
            MovePos(areaPx, OnGrowedUp);
        }else if(r < or){
            MovePos(areaPx, OnGrowedDown);
        }
    }
    public void GrowTo(float px){
        float opx = areaPx;
        areaPx = px;
        if(px > opx){
            MovePos(areaPx, OnGrowedUp);
        }else if(px < opx){
            MovePos(areaPx, OnGrowedDown);
        }
    }
    public void GrowByRatio(float dr){
        areaRatio += dr;
        if(dr > 0){
            MovePos(areaPx, OnGrowedUp);
        }else if(dr < 0){
            MovePos(areaPx, OnGrowedDown);
        }
    }
    public void MovePos(float px, TweenCallback cb){
        transform.DOLocalMoveX(px, growTime).SetEase(Ease.InSine).OnComplete(cb);
    }
    void OnGrowedUp(){
        ps.Play();
        growedUp.Invoke();
    }
    void OnGrowedDown(){
        growedDown.Invoke();
    }
    void CheckPos(){
        if(localPosition.x != areaPx){
            SetLocalPositionX(areaPx);
        }
    }
    public void Reset(){
        areaRatio = 0.5f;
        StopAllCoroutines();
        SetLocalPositionX(areaPx);
    }
    public bool Contains(Vector2 p, float m = 0){
        return p.x > min.x - m && p.x < max.x + m;
    }
}
