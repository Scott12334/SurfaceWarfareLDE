using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class COMOController : MonoBehaviour
{
    

    [SerializeField] private GameObject inputField;
    [SerializeField] private TextMeshProUGUI cicMessageText, bridgeMessageText;
    private TMP_InputField inputFieldText;

    private string cicMessageString;
    private string bridgeMessageString;
    private int cicMessageCount;
    private int bridgeMessageCount;
    int currentDestination=0;
    // Start is called before the first frame update
    void Start()
    {
        inputFieldText = inputField.GetComponent<TMP_InputField>();
        Debug.Log(inputFieldText);
        cicMessageString = "";
        bridgeMessageString = "";
        cicMessageCount = 1;
        bridgeMessageCount = 1;
    }

    public void recieveMessage(string message){
        string[] inputs = message.Split(",");
        Debug.Log(message);
        if(inputs[1] == "3" && inputs.Length > 4){
            //Message sent from CIC
            string firingSolutionUpdate = "Firing Solution for Contact ";
            firingSolutionUpdate+= inputs[3]+ ":"+"\n";
            if(int.Parse(inputs[4]) > 0){firingSolutionUpdate+= "Fire 5-Inch Gun"+"\n";}
            if(int.Parse(inputs[5]) > 0){firingSolutionUpdate+= "Fire "+inputs[5]+ " SAM(s)"+"\n";}
            if(int.Parse(inputs[6]) > 0){firingSolutionUpdate+= "Fire "+inputs[6]+ " SSM(s)"+"\n";}
            if(int.Parse(inputs[7]) > 0){firingSolutionUpdate+= "Fire KID"+"\n";}
            cicMessageString += "Message " + cicMessageCount + "- " + firingSolutionUpdate;
            cicMessageCount ++;
        }
        else if(inputs[1] == "4" && inputs.Length > 4){
            string[] latLon= new string[2];
                    for(int i=0; i<inputs.Length; i++){
                        if(inputs[i].Contains(":")){
                            latLon = inputs[i].Split(":");
                        }
                    }
            string contactMessage = "Contact "+inputs[3] +" is now at Latitude: "+ latLon[0]+ " and Longitude: "+latLon[1]+". Heading at "+inputs[5]+" Degrees with a speed of "+inputs[6]+" knots. It is a "+inputs[7];
            cicMessageString += "Message " + cicMessageCount + "- " + contactMessage + "<br>";
            cicMessageCount ++;
        }
        else if(inputs[1] == "0"){
            //Message sent from Bridge
            //Bridge Can send Pos Update, and request firing solution
            if(inputs.Length > 3){
                if(inputs[2] == "5"){
                    string bridgeRequest = "Bridge Requesting Firing Solution on Contact "+ inputs[3];
                    bridgeMessageString += "Message " + bridgeMessageCount + "- " + bridgeRequest + "<br>";
                    bridgeMessageCount++;
                }
                else if(inputs[2] == "6"){
                    string[] latLon= new string[2];
                    for(int i=0; i<inputs.Length; i++){
                        if(inputs[i].Contains(":")){
                            latLon = inputs[i].Split(":");
                        }
                    }
                    string positionMessage = "Ship is now at Latitude: "+ latLon[0]+ " and Longitude: "+latLon[1]+". Heading at "+inputs[4]+" Degrees with a speed of "+inputs[5]+" knots.";
                    bridgeMessageString += "Message " + bridgeMessageCount + "- " + positionMessage + "<br>";
                    bridgeMessageCount++;
                }
                else{
                    string requestSolution = inputs[3];
                    bridgeMessageString += "Message " + bridgeMessageCount + "- " + requestSolution + "<br>";
                    bridgeMessageCount++;
                }
            }
        }
        bridgeMessageText.text = bridgeMessageString;
        cicMessageText.text = cicMessageString;
    }
    public void sendMessage(){
        string destination= "";
        switch(currentDestination){
            case 0:
            destination="Press Button";
            break;

            case 1:
            destination="3";
            break;

            case 2:
            destination="DEFCON";
            break;

            case 3:
            destination="1";
            break;
        }
        string messageToServer = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        messageToServer += "8,";
        messageToServer += destination+",";
        Debug.Log(inputFieldText.text);
        messageToServer += inputFieldText.text;
        Debug.Log(messageToServer);
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(messageToServer);
        inputFieldText.text="";
    }
    public void setCurrentDestination(int newDest){currentDestination=newDest;}
}
