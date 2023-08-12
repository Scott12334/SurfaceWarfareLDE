using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class JODController : MonoBehaviour
{
    private string messageString;
    private int messageCount;
    [SerializeField] private TextMeshProUGUI messageText;
    // Start is called before the first frame update
    void Start()
    {
        messageString = "";
        messageCount = 1;
    }
    public void recieveMessage(string message){
        string[] inputs = message.Split(",");
        if(inputs.Length >4){
            if(inputs[1] == "2"){
                //STRINGs from COMO
                if(inputs[3] == "1"){
                    messageString += "Message " + messageCount + "- "+ inputs[4];
                    messageCount++;
                }
            }
            else if(inputs[1] == "3"){
                //Firing Solutions from CIC
                string firingSolutionUpdate = "Firing Solution for Contact ";
                firingSolutionUpdate+= inputs[3]+ ":"+"\n";
                if(int.Parse(inputs[4]) > 0){firingSolutionUpdate+= "Fire 5-Inch Gun"+"\n";}
                if(int.Parse(inputs[5]) > 0){firingSolutionUpdate+= "Fire "+inputs[5]+ " SAM(s)"+"\n";}
                if(int.Parse(inputs[6]) > 0){firingSolutionUpdate+= "Fire "+inputs[6]+ " SSM(s)"+"\n";}
                if(int.Parse(inputs[7]) > 0){firingSolutionUpdate+= "Fire KID"+"\n";}
                messageString += "Message " + messageCount + "- "+ firingSolutionUpdate;
                messageCount++;
            }
            else if(inputs[1] == "4"){
                //Contact Updates from CIC
                string[] latLon= new string[2];
                        for(int i=0; i<inputs.Length; i++){
                            if(inputs[i].Contains(":")){
                                latLon = inputs[i].Split(":");
                            }
                        }
                string posMessage = "Contact "+inputs[3] +" is now at Latitude: "+ latLon[0]+ " and Longitude: "+latLon[1]+". Heading at "+inputs[5]+" Degrees with a speed of "+inputs[6]+" knots. It is a "+inputs[7];
                messageString += "Message " + messageCount + "- "+ posMessage;
                messageCount++;
            }
            messageString +="<br>";
            messageText.text = messageString;
        }
    }
}
