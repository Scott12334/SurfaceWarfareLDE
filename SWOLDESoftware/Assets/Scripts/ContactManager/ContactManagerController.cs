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
    void Start() {
        contactInfoText = contactInfo.GetComponent<TextMeshProUGUI>();
        speedInputText = speedInput.GetComponent<TMP_InputField>();
        headingInputText = headingInput.GetComponent<TMP_InputField>();
        latInputText = latInput.GetComponent<TMP_InputField>();
        lonInputText = lonInput.GetComponent<TMP_InputField>();
        typeInputText = typeInput.GetComponent<TMP_InputField>();
        placementCalc = placementControl.GetComponent<PlacementCalc>();
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
    public void sendMessageToBridge(){
        string messageToBridge="";
        messageToBridge += 
            "Contact: "+ currentContactValue[0]+ " at Lat: "+latInputText.text+", Lon: "+lonInputText.text+". Going "
            +speedInputText.text+" knots with a heading of "+headingInputText.text+" Degrees. It is a "+typeInputText.text+".";
        Debug.Log(messageToBridge);
    }
    //#,Lat:Lon,Heading,Speed,Type
    public void newContact(string newContactMessage){
        string[] inputs= newContactMessage.Split(",");
        string[] latLon= new string[2];
        for(int i=0; i<inputs.Length; i++){
            if(inputs[i].Contains(":")){
                latLon = inputs[i].Split(":");
            }
        }
        if(placementCalc.inRange(latLon,4)){
            GameObject newBlip = Instantiate(blipPre, placementCalc.calcWorldPos(latLon, marker.transform.position),Quaternion.identity);
            newBlip.name = inputs[0];
            Contact newContact = newBlip.GetComponent<Contact>();
            newContact.setContactValue(inputs, latLon);
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
    }
}
