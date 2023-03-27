using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Clock : MonoBehaviour
{
    private int seconds;
    private int minutes;
    private int hours;
    [SerializeField]
    private GameObject[] clockDisplay;
    private int currentPanel;
    private bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        isRunning = false;
    }
    void Update(){
        string time ="";
        if(hours < 10){
            time += "0"+hours+":";
        }
        else{time+= hours+":";}

        if(minutes < 10){
            time += "0"+minutes+":";
        }
        else{time+= minutes+":";}
        
        if(seconds < 10){
            time += "0"+seconds;
        }
        else{time+= seconds;}

        clockDisplay[currentPanel].GetComponent<TextMeshProUGUI>().text = time;
    }
    public void setCurrentPanel(int panelID){this.currentPanel = panelID;}
    public void toggleClock(){
        isRunning = !isRunning;
        if(isRunning){
            StartCoroutine(clockControl());
        }
        else{
            StopCoroutine(clockControl());
        }
    }
    IEnumerator clockControl(){
        while(true){
            seconds++;
            if(seconds >= 60){
                seconds = 0;
                minutes ++;
            }
            if(minutes >= 60){
                minutes = 0;
                hours ++;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
