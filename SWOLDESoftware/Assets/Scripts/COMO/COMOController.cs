using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class COMOController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dials;
    private Dial[] dialScripts;
    bool debug=true;

    [SerializeField]
    private GameObject inputField;
    private TMP_InputField inputFieldText;

    int currentDestination=0;
    // Start is called before the first frame update
    void Start()
    {
        inputFieldText = inputField.GetComponent<TMP_InputField>();
        Debug.Log(inputFieldText);
        dialScripts= new Dial[dials.Length];
        for(int i=0; i<dials.Length; i++){
            dialScripts[i]=dials[i].GetComponent<Dial>();
        }
    }

    public void recieveMessage(string message){
        string[] inputs = message.Split(",");
        Debug.Log(message);
        if(inputs[1] == "3"){
            //Message sent from CIC
            string firingSolutionUpdate = "Firing Solution for Contact ";
            firingSolutionUpdate+= inputs[3]+ ":"+"\n";
            if(int.Parse(inputs[4]) > 0){firingSolutionUpdate+= "Fire 5-Inch Gun"+"\n";}
            if(int.Parse(inputs[5]) > 0){firingSolutionUpdate+= "Fire "+inputs[5]+ " SAM(s)"+"\n";}
            if(int.Parse(inputs[6]) > 0){firingSolutionUpdate+= "Fire "+inputs[6]+ " SSM(s)"+"\n";}
            if(int.Parse(inputs[7]) > 0){firingSolutionUpdate+= "Fire KID"+"\n";}
            dialScripts[0].sendMessage(firingSolutionUpdate);
        }
        else if(inputs[1] == "4"){
            string[] latLon= new string[2];
                    for(int i=0; i<inputs.Length; i++){
                        if(inputs[i].Contains(":")){
                            latLon = inputs[i].Split(":");
                        }
                    }
            dialScripts[0].sendMessage("Contact "+inputs[3] +" is now at Latitude: "+ latLon[0]+ " and Longitude: "+latLon[1]+". Heading at "+inputs[5]+" Degrees with a speed of "+inputs[6]+" knots. It is a "+inputs[7]);
        }
        else if(inputs[1] == "0"){
            //Message sent from Bridge
            //Bridge Can send Pos Update, and request firing solution
            if(inputs.Length > 3){
                if(inputs[2] == "5"){
                    dialScripts[1].sendMessage("Bridge Requesting Firing Solution on Contact "+ inputs[3]);
                }
                else if(inputs[2] == "6"){
                    string[] latLon= new string[2];
                    for(int i=0; i<inputs.Length; i++){
                        if(inputs[i].Contains(":")){
                            latLon = inputs[i].Split(":");
                        }
                    }
                    dialScripts[1].sendMessage("Ship is now at Latitude: "+ latLon[0]+ " and Longitude: "+latLon[1]+". Heading at "+inputs[4]+" Degrees with a speed of "+inputs[5]+" knots.");
                }
                else{
                    dialScripts[1].sendMessage(inputs[3]);
                }
            }
        }
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
