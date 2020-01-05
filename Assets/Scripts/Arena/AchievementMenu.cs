using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AchievementMenu : MonoBehaviour{
    public UnityEvent opened, closed;
    public RectTransform itemsMenu;
    public ScrollRect scr;
    AchievementItem selectedItem;
    public List<AchievementItem> menuItems;
    public bool allOpen, isOpened;
    List<AchievementItem> unlockedItems;
    Image target;
    void Awake(){
        scr = GetComponent<ScrollRect>();
    }
    void Start(){
        if(allOpen){
            menuItems.ForEach(el=>{el.Open();});
        }
    }
    
    public void OpenMenuFor(Image img){
        target = img;
        Transform i = itemsMenu.Find(img.sprite.name);
        if(i != null){
            float scroll = (itemsMenu.rect.xMin + i.localPosition.x) / itemsMenu.rect.width;
            scr.horizontalNormalizedPosition = scroll < 0.5f ? 0 : scroll;
            i.GetComponent<AchievementItem>().Select();
        }
        OpenMenu();
    }
    public void OnItemSelected(AchievementItem item){
        if(target != null){
            target.sprite = item.image.sprite;
            target.SetNativeSize();
        }
        foreach(var el in menuItems){
            if(el != item && el.isUnlocked){
                el.Unselect();
            }
        }
    }
    public void OpenMenu(){
        if(!isOpened){
            opened.Invoke();
            isOpened = true;
        }
    }
    public void Close(){
        if(isOpened){
            closed.Invoke();
            isOpened = false;
        }
    }
    public Image SelectItemImage(string n){
        return menuItems.Find(el=>el.name == n)?.image;
    }
    public Image GetRandomUnlockedItemImage(){
        if(unlockedItems == null){
            unlockedItems = new List<AchievementItem>();
            foreach(var el in menuItems){
                if((el.saveData!=null && el.saveData.unlocked) || el.isUnlocked){
                    unlockedItems.Add(el);
                }
            }
        }
        return unlockedItems[Random.Range(0,unlockedItems.Count)].image;
    }
}