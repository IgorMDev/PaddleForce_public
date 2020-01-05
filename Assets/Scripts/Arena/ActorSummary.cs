using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActorSummary : MonoBehaviour{
    public Actor actor;
    public Text roundsWon, goalsScored;
    public void SetSummary(){
        roundsWon.text = actor.roundsWon.ToString();
        goalsScored.text = actor.goalsScored.ToString();
    }
}