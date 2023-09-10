using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class EnemyShip : MonoBehaviour
{
    private GameObject targetScreen, gameController;
    private TextMeshProUGUI contactInfoText, firingSolutionText;
    [SerializeField]
    private Sprite friendlySprite;
    private GameController gameScript;
    private string[] contactinfo;
    private string currentFiringSolution;
    private bool fireSolutionBool;
    private int[] ammo;
    private string[] storedLatLong;
    private float health = 1000;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Screen") ==  0){
            targetScreen = GameObject.Find("Canvi").transform.GetChild(0).transform.GetChild(0).gameObject;
            contactInfoText = targetScreen.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            firingSolutionText = targetScreen.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            gameScript = GameObject.Find("GameController").GetComponent<GameController>();
        }
        else if(PlayerPrefs.GetString("Screen") ==  "DESCON"){

        }
    }
    public void damage(float damageDone){
        health -= damageDone;
        if(health <=0){
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(other.gameObject);
    }
    private void OnMouseDown() {
        Debug.Log(PlayerPrefs.GetInt("Screen"));
        if(PlayerPrefs.GetInt("Screen") ==  0 && PlayerPrefs.GetInt("Ship") !=  7){
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
        else if(PlayerPrefs.GetInt("Ship") ==  7){
            GameObject infoDisplay = GameObject.Find("ContactDisplay");
            Debug.Log(infoDisplay.transform.GetChild(0).name);
            infoDisplay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Name: " + contactinfo[0];
            infoDisplay.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Heading: " + contactinfo[3];
            infoDisplay.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Speed: " + contactinfo[4];
            infoDisplay.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Location: " + contactinfo[1] + "-" + contactinfo[2];
            infoDisplay.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Type: " + contactinfo[5];
        }
    }
    public string[] getLatLon(){
        return storedLatLong;
    }
    public void setContactValue(string[] inputs, string[] latLon){
        storedLatLong = latLon;
        contactinfo = new string[6];
        contactinfo[0] = inputs[3];
        contactinfo[1] = latLon[0];
        contactinfo[2] = latLon[1];
        contactinfo[3] = inputs[5]; 
        contactinfo[4] = inputs[6];
        contactinfo[5] = inputs[7];
        if(contactinfo[5] == "Friendly"){
            this.GetComponent<SpriteRenderer>().sprite = friendlySprite;
        }
    }
    public void firingSolution(int[] firingSolution){
        ammo = firingSolution;
        currentFiringSolution = "Firing Solution for Contact ";
        currentFiringSolution+= contactinfo[0]+ ":"+"\n";
        if(firingSolution[1] > 0){currentFiringSolution+= "Fire 5-Inch Gun"+"\n";}
        if(firingSolution[2] > 0){currentFiringSolution+= "Fire "+firingSolution[2]+ " SAM(s)"+"\n";}
        if(firingSolution[3] > 0){currentFiringSolution+= "Fire "+firingSolution[3]+ " SSM(s)"+"\n";}
        if(firingSolution[4] > 0){currentFiringSolution+= "Fire KID"+"\n";}
        firingSolutionText.text = currentFiringSolution;
        fireSolutionBool= true;
    }
    public int[] getAmmo(){return ammo;}
    public bool hasFireSolution(){return fireSolutionBool;}
    public void setPos(string[] latLon){
        storedLatLong = latLon;
    }
}
