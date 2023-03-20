using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    private int shipID;
    private int panelID;
    [SerializeField]
    private GameObject[] controllers;
    private GameObject currentController;
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
        int messageShip = int.Parse(messageParts[0]);
        //IF STAFF/DESCON get all messages
        if(shipID == 6){
            currentController.GetComponent<StaffController>().recieveMessage(fixMessage(messageParts));
        }
        else if(shipID == 7){

        }
        //Else only read the ones you need
        else{
            //Read all messages from Staff, Descon, not from other ships
            if(messageShip == shipID || messageShip == 6 || messageShip == 7){

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
}
