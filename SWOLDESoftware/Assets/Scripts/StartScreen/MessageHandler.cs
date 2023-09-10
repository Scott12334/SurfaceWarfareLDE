using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    private int shipID;
    private int panelID;
    [SerializeField] private GameObject[] controllers;
    private GameObject currentController;
    [SerializeField] private Clock clock;
    public void setShipID(int newShipID){
        this.shipID = newShipID;
        Debug.Log("SHIP ID-"+shipID);
    }
    public void setPanelID(int newPanelID){
        this.panelID = newPanelID;
        currentController = controllers[panelID];
        Debug.Log("Panel ID-"+panelID);
    } 
    public void recieveMessage(string latestMessage){
        string[] messageParts = latestMessage.Split(",");
        if(messageParts[0] == "11"){
            clock.clockSetTime(int.Parse(messageParts[1]),int.Parse(messageParts[2]), int.Parse(messageParts[3]));
        }
        else if(messageParts[0] == "12"){
            if(GameObject.Find(messageParts[2]) != null){
                switch(panelID){
                    case 0:
                    //BRIDGE
                    GameObject.Find(messageParts[2]).GetComponent<EnemyShip>().damage(int.Parse(messageParts[1]));
                    break;

                    case 4:
                    //Contact CIC
                    GameObject.Find(messageParts[2]).GetComponent<Contact>().damage(int.Parse(messageParts[1]));
                    break;
                }
                //DESCON
                if(shipID == 7){
                    GameObject.Find(messageParts[2]).GetComponent<EnemyShip>().damage(int.Parse(messageParts[1]));
                }
                if(shipID == 6){
                    GameObject.Find(messageParts[2]).GetComponent<ContactControl>().removeContact(messageParts[1]);
                }
            }
        }
        else if(latestMessage.Contains("disabled") && int.Parse(messageParts[0]) == shipID){
            GameObject.Find("SimController").GetComponent<StartScreenControl>().setToggle(true);
        }
        else if(latestMessage.Contains("enabled") && int.Parse(messageParts[0]) == shipID){
            GameObject.Find("SimController").GetComponent<StartScreenControl>().setUnToggle(true);
        }
        else{
            int messageShip = int.Parse(messageParts[0]);
            int messageType = int.Parse(messageParts[2]);
            //IF STAFF/DESCON get all messages
            if(shipID == 6){
                currentController.GetComponent<StaffController>().recieveMessage(latestMessage);
            }
            else if(shipID == 7){
                currentController.GetComponent<DESCON>().recieveMessage(latestMessage);
            }
            //Else only read the ones you need
            else{
                //Read all messages from Staff, Descon, not from other ships
                if(messageShip == shipID || messageShip == 6 || messageShip == 7 || messageType == 6){
                    if(panelID  ==3){
                        currentController.GetComponent<CICMainController>().recieveMessage(latestMessage);
                    }
                    else if(panelID == 4){
                        currentController.GetComponent<ContactManagerController>().recieveMessage(latestMessage);
                    }
                    else if(panelID == 2){
                        currentController.GetComponent<COMOController>().recieveMessage(latestMessage);
                    }
                    else if(panelID == 1){
                        currentController.GetComponent<JODController>().recieveMessage(latestMessage);
                    }
                    else if(panelID == 0){
                        currentController.GetComponent<GameController>().recieveMessage(latestMessage);
                    }
                }
            }
            if(messageParts[2] == "9"){
                GameObject clock = GameObject.FindGameObjectWithTag("Clock");
                if(clock != null){
                    clock.GetComponent<Clock>().toggleClock();
                }
                else{
                    Debug.Log("null");
                }
            }
        }
    }
    public string fixMessage(string[] messageParts){
        string finalMessage = "";
        for(int i = 2; i < messageParts.Length; i++){
            if( i != messageParts.Length-1){
                finalMessage += messageParts[i]+",";
            }
            else{
                finalMessage += messageParts[i];
            }
        }
        return finalMessage;
    }
    public string header(){return shipID+","+panelID+",";}
}
