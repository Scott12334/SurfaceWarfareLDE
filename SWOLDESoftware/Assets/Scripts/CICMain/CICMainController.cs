using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CICMainController : MonoBehaviour
{
    [SerializeField]
    private GameObject firingSolutionMessage;
    private TextMeshProUGUI firingSolutionText;
    [SerializeField]
    private GameObject[] shotsLeft;
    private TextMeshProUGUI[] shotsLeftText;
    //Missile Arrays 0=GUN, 1=SAM, 2=SSM, 3=KID
    [SerializeField]
    private int[] firingSolution;
    private int[] ammo;
    string currentContact="000";

    [SerializeField]
    private GameObject contactInput;
    private TMP_InputField contactInputText;
    
    private string bridgeMessageString , commoMessageString;
    [SerializeField] private TextMeshProUGUI bridgeMessageText, commoMessageText;
    private int bridgeMessageCount, commoMessageCount;
    // Start is called before the first frame update
    void Start()
    {
        ammo= new int[]{50,50,50,1};
        firingSolution= new int[4];
        firingSolutionText = firingSolutionMessage.GetComponent<TextMeshProUGUI>();
        contactInputText= contactInput.GetComponent<TMP_InputField>();
        shotsLeftText= new TextMeshProUGUI[shotsLeft.Length];
        for(int i=0; i<shotsLeft.Length; i++){
            shotsLeftText[i]= shotsLeft[i].GetComponent<TextMeshProUGUI>();
            shotsLeftText[i].text= "Shots Left: "+ammo[i];
        }
        updateText();

        bridgeMessageString = "";
        commoMessageString = "";
        bridgeMessageCount = 1;
        commoMessageCount = 1;
    }

    public void recieveMessage(string message){
        string[] inputs = message.Split(",");
        Debug.Log(message);
        if(inputs[1] == "2" && inputs.Length > 4){
            //Message sent from COMO
            if(inputs[3] == "3"){
                commoMessageString += "Message " + commoMessageCount + " - " + inputs[4] + "<br>";
                commoMessageCount ++;
            }
        }
        else if(inputs[1] == "0"){
            //Message sent from Bridge
            //Bridge Can send Pos Update, and request firing solution
            if(inputs.Length > 3){
                if(inputs[2] == "5"){
                    string requestSolution = "Bridge Requesting Firing Solution on Contact "+ inputs[3];
                    bridgeMessageString += "Message " + bridgeMessageCount + " - " + requestSolution + "<br>";
                    bridgeMessageCount ++;
                }
                else if(inputs[2] == "6"){
                    string[] latLon= new string[2];
                    for(int i=0; i<inputs.Length; i++){
                        if(inputs[i].Contains(":")){
                            latLon = inputs[i].Split(":");
                        }
                    }
                    string positionMessage = "Ship is now at Latitude: "+ latLon[0]+ " and Longitude: "+latLon[1]+". Heading at "+inputs[4]+" Degrees with a speed of "+inputs[5]+" knots.";
                    bridgeMessageString += "Message " + bridgeMessageCount + " - " + positionMessage + "<br>";
                    bridgeMessageCount ++;
                }
                else{
                    bridgeMessageString += "Message " + bridgeMessageCount + " - " + inputs[3] + "<br>";
                    bridgeMessageCount ++;
                }
            }
        }
        bridgeMessageText.text = bridgeMessageString;
        commoMessageText.text = commoMessageString;
    }

    public void upButtonPressed(int missle){
        if(ammo[missle] >0){
            if(missle==0 || missle==3){
                if(firingSolution[missle]>0){

                }
                else{
                    firingSolution[missle] ++;
                    ammo[missle] --;
                    updateText();
                }
            }
            else{
                firingSolution[missle] ++;
                ammo[missle] --;
                updateText();
            }
        }
    }
    public void downButtonPressed(int missle){
        if(firingSolution[missle] >0){
            firingSolution[missle] --;
            ammo[missle] ++;
            updateText();
        }
    }
    public void updateText(){
        string firingSolutionUpdate = "Firing Solution for Contact ";
        firingSolutionUpdate+= currentContact+ ":"+"\n";
        if(firingSolution[0] > 0){firingSolutionUpdate+= "Fire 5-Inch Gun"+"\n";}
        if(firingSolution[1] > 0){firingSolutionUpdate+= "Fire "+firingSolution[1]+ " SAM(s)"+"\n";}
        if(firingSolution[2] > 0){firingSolutionUpdate+= "Fire "+firingSolution[2]+ " SSM(s)"+"\n";}
        if(firingSolution[3] > 0){firingSolutionUpdate+= "Fire KID"+"\n";}
        firingSolutionText.text = firingSolutionUpdate;
        
        for(int i=0; i<shotsLeftText.Length; i++){
            shotsLeftText[i].text= "Shots Left: "+ammo[i];
        }
    }
    public void sendFiringSolution(){
        string messageToBridge="";
        messageToBridge += GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        messageToBridge += "7,";
        messageToBridge += currentContact+",";
        messageToBridge += firingSolution[0]+",";
        messageToBridge += firingSolution[1]+",";
        messageToBridge += firingSolution[2]+",";
        messageToBridge += firingSolution[3];
        Debug.Log(messageToBridge);
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(messageToBridge);
        for(int i=0; i<firingSolution.Length; i++){
            firingSolution[i]=0;
        }
        updateText();
    }
    public void setContact(){
        currentContact= contactInputText.text;
        contactInputText.text= "";
        updateText();
    }
}
