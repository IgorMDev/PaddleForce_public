using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class AchievementUnlocker{
    public static void CheckActorGameAchievements(Actor a){
        if(a.minRatioInfo >= 0.2f){
            UnlockAchievement(AchievementType.Keep20TerritoryInGame, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_keep_20));
        }
        if(a.minRatioInfo >= 0.3f){
            UnlockAchievement(AchievementType.Keep30TerritoryInGame, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_keep_30));
        }
        if(a.minRatioInfo >= 0.4f){
            UnlockAchievement(AchievementType.Keep40TerritoryInGame, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_keep_40));
        }
        if(a.roundsWon >= 1){
            UnlockAchievement(AchievementType.WinRound, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_win_round));
        }
        if(a.roundsWonInRow >= 2){
            UnlockAchievement(AchievementType.Win2RoundsInRow, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_2_rounds_in_row));
        }
        if(a.roundsWonInRow >= 3){
            UnlockAchievement(AchievementType.Win3RoundsInRow, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_3_rounds_in_row));
        }
        if(a.roundsNoMiss >= 1){
            UnlockAchievement(AchievementType.DontMissGoalIn1Round, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_keeper));
        }
        if(a.roundsNoMiss >= 2){
            UnlockAchievement(AchievementType.DontMissGoalIn2Rounds, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_defender));
        }
        if(a.roundsNoMiss >= 3){
            UnlockAchievement(AchievementType.DontMissGoalIn3Rounds, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_the_wall));
        }
        if(a.maxGoalCombo >= 3){
            UnlockAchievement(AchievementType.Make3Combo, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_3x_combo));
        }
        if(a.maxGoalCombo >= 5){
            UnlockAchievement(AchievementType.Make5Combo, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_5x_combo));
        }
        if(a.maxGoalCombo >= 7){
            UnlockAchievement(AchievementType.Make7Combo, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_7x_combo));
        }
    }
    public static void CheckConsecutiveDay(){
        if(Prefs.arenaData.consecutiveDay >= 3){
            UnlockAchievement(AchievementType.Play3ConsecutiveDay);
        }
        if(Prefs.arenaData.consecutiveDay >= 5){
            UnlockAchievement(AchievementType.Play5ConsecutiveDay);
        }
        if(Prefs.arenaData.consecutiveDay >= 7){
            UnlockAchievement(AchievementType.Play7ConsecutiveDay);
        }
        if(Prefs.arenaData.consecutiveDay >= 10){
            UnlockAchievement(AchievementType.Play10ConsecutiveDay);
        }
    }
    public static void CheckPlayedMatches(){
        if(Prefs.arenaData.playedMatches >= 10){
            UnlockAchievement(AchievementType.Play10Match, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_amateur));
        }
        if(Prefs.arenaData.playedMatches >= 25){
            UnlockAchievement(AchievementType.Play25Match, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_expert));
        }
        if(Prefs.arenaData.playedMatches >= 50){
            UnlockAchievement(AchievementType.Play50Match, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_master));
        }
    }
    public static void CheckWonMatches(){
        if(Prefs.arenaData.wonMatches >= 3){
            UnlockAchievement(AchievementType.Win3Match, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_3_wins));
        }
        if(Prefs.arenaData.wonMatches >= 5){
            UnlockAchievement(AchievementType.Win5Match, () => 
            UnlockGPGSAchievement(GPGSIds.achievement_5_wins));
        }
        if(Prefs.arenaData.wonMatches >= 7){
            UnlockAchievement(AchievementType.Win7Match, ()=>
            UnlockGPGSAchievement(GPGSIds.achievement_7_wins));
        }
    }
    public static void UnlockAchievement(AchievementType t, UnityAction callback = null){
        AchievementData a = Prefs.arenaData.achievements.Find((AchievementData d)=>d.type == t);
        if(a == null && t != AchievementType.None){
            a = new AchievementData{type = t};
            Prefs.arenaData.achievements.Add(a);
        }
        if(a != null && !a.unlocked){
            a.unlocked = true;
            callback?.Invoke();
        }
    }
    public static void UnlockGPGSAchievement(string id){
        #if UNITY_ANDROID
        try{
            GPGSi.instance.UnlockAchievement(id);
        }catch{
            Debug.LogWarning("GPGServices intance not found");
        }
        #endif
    }
}
public enum AchievementType{
    None,
    Make3Combo,
    Make5Combo,
    Make7Combo,
    Keep20TerritoryInGame,
    Keep30TerritoryInGame,
    Keep40TerritoryInGame,
    WinRound,
    Win2RoundsInRow,
    Win3RoundsInRow,
    Play3ConsecutiveDay,
    Play5ConsecutiveDay,
    Play7ConsecutiveDay,
    Play10ConsecutiveDay,
    Win3Match,
    Win5Match,
    Win7Match,
    Play10Match,
    Play25Match,
    Play50Match,
    DontMissGoalIn1Round,
    DontMissGoalIn2Rounds,
    DontMissGoalIn3Rounds,

}
