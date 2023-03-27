using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContactManagerController : MonoBehaviour
{
    [SerializeField]
    private GameObject contactInfo;
    private TextMeshProUGUI contactInfoText;
    [SerializeField]
    private GameObject speedInput,typeInput,lonInput,latInput,headingInput;
    private TMP_InputField speedInputText,headingInputText,latInputText,lonInputText, typeInputText;
    [SerializeField]
    private GameObject blipPre, placementControl, marker;
    private PlacementCalc placementCalc;
    string[] currentContactValue;
    string[] shipNames;
    List<GameObject> enemies;
    void Start() {
        contactInfoText = contactInfo.GetComponent<TextMeshProUGUI>();
        speedInputText = speedInput.GetComponent<TMP_InputField>();
        headingInputText = headingInput.GetComponent<TMP_InputField>();
        latInputText = latInput.GetComponent<TMP_InputField>();
        lonInputText = lonInput.GetComponent<TMP_InputField>();
        typeInputText = typeInput.GetComponent<TMP_InputField>();
        placementCalc = placementControl.GetComponent<PlacementCalc>();
        shipNames = new string[6]{"Stethem","Gettysburg","Chosin","Kidd","Galagher","Lake Erie"};
        enemies = new List<GameObject>();
    }
    public void contactPressed(string[] contactValues){
        currentContactValue= contactValues;
        string fullContactInfo = "";
        fullContactInfo += "Contact: "+contactValues[0]+"\n";
        fullContactInfo += "Lat: "+contactValues[1]+"\n";
        fullContactInfo += "Lon: "+contactValues[2]+"\n";
        fullContactInfo += "Heading: "+contactValues[3]+"\n";
        fullContactInfo += "Speed: "+contactValues[4]+"\n";
        fullContactInfo += "Type: "+contactValues[5];
        contactInfoText.text = fullContactInfo;
    }
    public void sendMessage(){
        string messageToBridge="";
        messageToBridge += GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        messageToBridge += "0,";
        messageToBridge += currentContactValue[0]+",";
        messageToBridge += latInputText.text+":"+lonInputText.text+",";
        messageToBridge += headingInputText.text+",";
        messageToBridge += speedInputText.text+",";
        messageToBridge += typeInputText.text;
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(messageToBridge);
    }
    //#,Lat:Lon,Heading,Speed,Type
    public void recieveMessage(string newContactMessage){
        string[] inputs= newContactMessage.Split(",");
        //Contacts from Staff
        if(inputs[0] == "6"){
            if(inputs[2] == "0"){
                string[] latLon= new string[2];
                for(int i=0; i<inputs.Length; i++){
                    if(inputs[i].Contains(":")){
                        latLon = inputs[i].Split(":");
                    }
                }
                if(GameObject.Find(inputs[3]) == null){
                    GameObject newBlip = Instantiate(blipPre, placementCalc.calcWorldPos(latLon, marker.transform.position),Quaternion.identity);
                    newBlip.name = inputs[3];
                    Contact newContact = newBlip.GetComponent<Contact>();
                    newContact.setContactValue(inputs, latLon, false, "");
                    enemies.Add(newBlip);
                    if(!placementCalc.inRange(latLon,4)){
                        newBlip.GetComponent<SpriteRenderer>().enabled =false;
                    }
                }
                else{
                    Contact newContact = GameObject.Find(inputs[3]).GetComponent<Contact>();
                    newContact.setContactValue(inputs, latLon, false, "");
                    GameObject.Find(inputs[3]).transform.position = placementCalc.calcWorldPos(latLon, marker.transform.position);
                    if(!placementCalc.inRange(latLon,4)){
                        GameObject.Find(inputs[3]).GetComponent<SpriteRenderer>().enabled =false;
                    }
                    else{
                        GameObject.Find(inputs[3]).GetComponent<SpriteRenderer>().enabled =true;
                    }
                }
            }
        }
        if(inputs[2] == "6"){
            if(GameObject.Find("SimController").GetComponent<StartScreenControl>().getShipID() != int.Parse(inputs[0])){
                string[] latLon= new string[2];
                for(int i=0; i<inputs.Length; i++){
                    if(inputs[i].Contains(":")){
                        latLon = inputs[i].Split(":");
                    }                
                }
                if(GameObject.Find(shipNames[int.Parse(inputs[0])]) == null){
                    GameObject newBlip = Instantiate(blipPre, placementCalc.calcWorldPos(latLon, marker.transform.position),Quaternion.identity);                
                    Contact newContact = newBlip.GetComponent<Contact>();
                    newContact.name = shipNames[int.Parse(inputs[0])];    
                    newContact.setContactValue(inputs, latLon, true, shipNames[int.Parse(inputs[0])]);
                    newContact.setContactName(shipNames[int.Parse(inputs[0])]);
                    enemies.Add(newBlip);
                    if(!placementCalc.inRange(latLon,4)){
                        newBlip.GetComponent<SpriteRenderer>().enabled =false;
                    }
                }
                else{
                    Contact newContact= GameObject.Find(shipNames[int.Parse(inputs[0])]).GetComponent<Contact>();
                    newContact.setContactValue(inputs, latLon, true, shipNames[int.Parse(inputs[0])]);
                    GameObject.Find(shipNames[int.Parse(inputs[0])]).transform.position = placementCalc.calcWorldPos(latLon, marker.transform.position);
                }
            }
            else{
                positionUpdate(newContactMessage);
            }
        }
        else if(inputs[2] == "10"){
            enemies.Remove(GameObject.Find(inputs[3]));
            Destroy(GameObject.Find(inputs[3]));
        }
    }
    //#,Lat:Lon,Heading,Speed
    public void positionUpdate(string message){
        string[] inputs= message.Split(",");
        string[] latLon= new string[2];
        for(int i=0; i<inputs.Length; i++){
            if(inputs[i].Contains(":")){
                latLon = inputs[i].Split(":");
            }
        }
        placementCalc.setCurrentLoc(latLon);
        for(int i=0 ; i<enemies.Count; i++){
            if(enemies[i] != null){
                enemies[i].transform.position=placementCalc.calcWorldPos(enemies[i].GetComponent<Contact>().getLatLon(), marker.transform.position);
                if(placementCalc.inRange(enemies[i].GetComponent<Contact>().getLatLon(),4)){
                        enemies[i].GetComponent<SpriteRenderer>().enabled =true;
                }
            }
        }
    }
}
