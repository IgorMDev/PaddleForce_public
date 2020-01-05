using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BotActor : Mover2DController{
    public Actor actor;
    public Brick self{get{return actor.brick;}}
    public Brick enemy{get{return actor.enemy.brick;}}
    public Ball ball{get{return Arena.Ball;}}
    public Effector effector{get{return Arena.Effector;}}
    public GoalTrigger goal{get{return actor.goalTrigger;}}
    public float advance = 0, distToStrike, boundDist;
    public bool isEnabled = true;
    public CircleCollider2D col;
    bool focused{get{return GetComponent<Actor>().isFocused;}}
    Vector2 dirToGoal, dirToBall, fdirToBall, dirToEffector, dirToEnemy, curScale;
    bool isBallStriking, isBallForward, isBallApproachToMe,isBallApproachToGoal, isBallInStrikeZone,
    isEffStriking, isEffForward, isEffInStrikeZone;
    float strikeAng;
    UnityAction move, rotate;
    new void Awake(){
        base.Awake();
        actor = GetComponent<Actor>();
        if(isEnabled){
            actions = UpdateState;
        }
    }
    void Start(){
        dirToGoal = new Vector2((goal.position - self.position).x, 0);
        curScale = self.scale;
        SetDistToStrike();
    }
    void UpdateState(){
        dirToEnemy = enemy.position - self.position;
        SetBallData();
        SetEffectorData();
        FollowBall();
        if((focused && dirToEffector.magnitude < dirToBall.magnitude) 
            || dirToEffector.magnitude <= distToStrike
            || dirToEffector.magnitude < dirToBall.magnitude/2){
            FollowEffector();
        }
        move.Invoke();
        rotate.Invoke();
    }
    void SetBallData(){
        if(ball != null){
            dirToBall = ball.position - self.position;
            fdirToBall = (actor.isFocused && SimilarityNormalized(dirToGoal, dirToBall) < 0.707f)  ? dirToBall - mover.forward*self.strikeBound : dirToBall;
            isBallForward = SimilarDirection(dirToBall, dirToGoal);
            //isBallApproachToMe = SimilarDirection(ball.velocity, -dirToBall);
            //isBallApproachToGoal = SimilarDirection(ball.velocity, dirToGoal);
            isBallInStrikeZone = dirToBall.magnitude <= distToStrike;
        }
    }
    void SetEffectorData(){
        if(effector != null && !effector.isApplied){
            dirToEffector = effector.position - self.position;
            isEffForward = SimilarDirection(dirToEffector, dirToGoal);
            isEffInStrikeZone = dirToEffector.magnitude <= distToStrike;
        }
    }
    void FollowBall(){
        if(ball != null){
            if(actor.field.Contains(ball.position, 64) || !actor.GetComponent<ActorMover>().bounded){
                move = () => mover.MoveBy(fdirToBall + ball.velocity*Time.fixedDeltaTime*advance);
            }else{
                Vector2 bd = (ball.position + ball.velocity*Time.fixedDeltaTime*advance)-new Vector2(Arena.delimLine, ball.position.y);
                bd.x = Arena.delimLine - (bd.x-Arena.delimLine);
                float r = (2*actor.field.localPosition.x/Arena.rect.width);
                bd.x *= (r <= 1 ? r/2 : r/2+0.5f);
                bd.y = ball.position.y/2f;
                move = () => mover.MoveTo(bd, 0.7f);
            }
            if(isBallInStrikeZone){
                StrikeBall();
            }else{
                rotate = () => mover.LookAt(Vector2.Lerp(dirToBall, new Vector2(dirToBall.x*100, 0), 0.5f));
                isBallStriking = false;
            }
        }
    }
    void FollowEffector(){
        if(effector != null && !effector.isApplied && effector.GetComponent<Collider2D>().enabled){
            if((actor.field.Contains(effector.position, 100))){
                move = () => mover.MoveBy(dirToEffector);
            }
            if(isEffInStrikeZone){
                if(effector.GetComponent<Collider2D>().isTrigger){
                    rotate = () => mover.RotateTo(Vector2.SignedAngle(mover.forward, dirToEffector));
                }else{
                    StrikeEffector();
                }
            }else{
                rotate = () => mover.LookAt(dirToEffector);
                isEffStriking = false;
            }
        }
    }
    void StrikeBall(){
        if(!isBallStriking){
            strikeAng = Vector2.SignedAngle(mover.forward, -ball.velocity);
            if(Mathf.Abs(strikeAng) < 10){
                float r = Random.value;
                if(r < 0.4){
                    strikeAng = -1;
                }else if(r > 0.6){
                    strikeAng = 1;
                }else{
                    //move = () => mover.MoveBy(fdirToBall);
                }
            }
            if(!isBallForward && actor.isFocused && SimilarityNormalized(ball.velocity, dirToGoal) > 0.5f){
                strikeAng = -strikeAng;
            }
            rotate = () => mover.RotateBy(strikeAng);
            isBallStriking = true;
            StartCoroutine(wait(1));
            IEnumerator wait(float t){
                yield return new WaitForSecondsRealtime(t);
                isBallStriking = false;
            }
        }
    }
    void StrikeEffector(){
        if(!isEffStriking){
            strikeAng = Vector2.SignedAngle(mover.forward, dirToEffector);
            isEffStriking = true;
            if(Mathf.Abs(strikeAng) < 10){
                float r = Random.value;
                if(r < 0.4){
                    strikeAng = -1;
                }else if(r > 0.6){
                    strikeAng = 1;
                }else{
                    //move = () => mover.MoveBy(dirToEffector);
                    isEffStriking = false;
                }
            }
            if(!isEffForward){
                strikeAng = -strikeAng;
            }
            rotate = () => mover.RotateBy(strikeAng);
        }
    }
    void LateUpdate(){
        if(curScale != self.scale){
            SetDistToStrike();
            //col.transform.localScale = Vector2.one / self.scale;
            curScale = self.scale;
        }
        // if(actor.field.localPosition.x < boundDist*2){
        //     col.radius = actor.field.localPosition.x/2;
        // }else{
        //     col.radius = boundDist*2;
        // }
    }
    void MoveAround(Vector2 p, float dist){
        move = () => mover.MoveTo(p+Vector2.Perpendicular(p)*Mathf.Sign((p - mover.position).y)*dist);
        rotate = () => mover.LookAt(p);
    }
    void SetDistToStrike(){
        boundDist = self.strikeBound * self.scale.LargestComponent();
        distToStrike = boundDist + ball.GetComponent<CircleCollider2D>().radius;
    }
    /* void CreateBounds(){
        if(self.transform.childCount > 0){
            col = self.transform.GetChild(0).GetComponent<CircleCollider2D>();
        }else{
            GameObject go = new GameObject();
            go.layer = 12;
            go.transform.parent = self.transform;
            col = go.AddComponent<CircleCollider2D>();
            col.transform.localPosition = Vector2.zero;
        }
    } */
    bool SimilarDirection(Vector2 v1, Vector2 v2){
        if(v1.magnitude == 0 || v2.magnitude == 0){
            return false;
        }
        return Vector2.Dot(v1.normalized, v2.normalized*2) > 0;
    }
    float SimilarityNormalized(Vector2 v1, Vector2 v2){
        try{
            return Vector2.Dot(v1.normalized, v2.normalized);
        }catch{
            return 0;
        }
    }
    void OnDestroy(){
        StopAllCoroutines();
    }
}