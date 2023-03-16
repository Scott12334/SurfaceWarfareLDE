using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyShip : MonoBehaviour
{
    private GameObject targetScreen, gameController;
    private TextMeshProUGUI contactInfoText, firingSolutionText;
    private GameController gameScript;
    private string[] contactinfo;
    private string currentFiringSolution;
    private bool fireSolutionBool;
    private int[] ammo;
    // Start is called before the first frame update
    void Start()
    {
        targetScreen = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        contactInfoText = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        firingSolutionText = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        gameScript = GameObject.Find("GameController").GetComponent<GameController>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("test");
        Destroy(other.gameObject);
    }
    private void OnMouseDown() {
        gameScript.setSelectedEnemy(this.gameObject);
        gameScript.targetButtonActive();
        targetScreen.SetActive(true);
        string fullContactInfo = "";
        fullContactInfo += "Contact: "+contactinfo[0]+"\n";
        fullContactInfo += "Lat: "+contactinfo[1]+"\n";
        fullContactInfo += "Lon: "+contactinfo[2]+"\n";
        fullContactInfo += "Heading: "+contactinfo[3]+"\n";
        fullContactInfo += "Speed: "+contactinfo[4]+"\n";
        fullContactInfo += "Type: "+contactinfo[5];
        contactInfoText.text = fullContactInfo;
        if(fireSolutionBool){
            firingSolutionText.text = currentFiringSolution;
        }
        else{
            firingSolutionText.text = "Press the blue button to request a firing solution for current target";
        }
    }
    public void setContactValue(string[] inputs, string[] latLon){
        contactinfo = new string[6];
        contactinfo[0] = inputs[1];
        contactinfo[1] = latLon[0];
        contactinfo[2] = latLon[1];
        contactinfo[3] = inputs[3]; 
        contactinfo[4] = inputs[4];
        contactinfo[5] = inputs[5];
    }
    public void firingSolution(int[] firingSolution){
        ammo = firingSolution;
        currentFiringSolution = "Firing Solution for Contact ";
        currentFiringSolution+= contactinfo[0]+ ":"+"\n";
        if(firingSolution[1] > 0){currentFiringSolution+= "Fire 5-Inch Gun"+"\n";}
        if(firingSolution[2] > 0){currentFiringSolution+= "Fire "+firingSolution[1]+ " SAM(s)"+"\n";}
        if(firingSolution[3] > 0){currentFiringSolution+= "Fire "+firingSolution[2]+ " SSM(s)"+"\n";}
        if(firingSolution[4] > 0){currentFiringSolution+= "Fire KID"+"\n";}
        firingSolutionText.text = currentFiringSolution;
        fireSolutionBool= true;
    }
    public int[] getAmmo(){return ammo;}
    public bool hasFireSolution(){return fireSolutionBool;}
}
