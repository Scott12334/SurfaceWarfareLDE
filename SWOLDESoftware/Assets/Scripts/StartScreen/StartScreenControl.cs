using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;
public class StartScreenControl : NetworkBehaviour
{
    private int selectedShip;
    [SerializeField]
    private GameObject shipSelection, screenSelection, messagePre, textDisplay, messageHandlerGO;
    [SerializeField]
    private GameObject[] screensObject, screensCanvas;
    private int lastCount;
    private GameObject newMessage;
    private MessageHandler messageHandler;
    // Start is called before the first frame update
    void Start()
    {
        lastCount = GameObject.FindGameObjectsWithTag("Message").Length; 
        messageHandler = messageHandlerGO.GetComponent<MessageHandler>();
    }
    void Update(){ 
        int currentCount = GameObject.FindGameObjectsWithTag("Message").Length;
        if(currentCount != lastCount){
            GameObject[] messages = GameObject.FindGameObjectsWithTag("Message");
            messageHandler.recieveMessage(messages[messages.Length-1].GetComponent<Message>().getMessage());
        }
        lastCount = currentCount;
    }
    public void shipSelected(int shipId){
        this.selectedShip = shipId;
        //STAFF
        if(shipId == 6){
            messageHandler.setShipID(6);
            messageHandler.setPanelID(5);
            screensObject[5].SetActive(true);
            screensCanvas[5].SetActive(true);
            screensCanvas[7].SetActive(false);
        }
        //DESCON
        else if(shipId == 7){
            messageHandler.setShipID(7);
            messageHandler.setPanelID(6);
            screensObject[6].SetActive(true);
            screensCanvas[6].SetActive(true);
            screensCanvas[7].SetActive(false);
        }
        else{
            PlayerPrefs.SetInt("shipId", shipId);
            shipSelection.SetActive(false);
            screenSelection.SetActive(true);
        }
    }
    public void screenSelected(int screenID){
            messageHandler.setShipID(selectedShip);
            messageHandler.setPanelID(screenID);
            screensObject[screenID].SetActive(true);
            screensCanvas[screenID].SetActive(true);
            screensCanvas[7].SetActive(false);
    }
    public void startHost(){
        NetworkManager.Singleton.StartHost();
    } 
    [ServerRpc(RequireOwnership = false)]
    void sendMessageServerRpc(string message){
        setMessageOnClientRpc(message);
    }
    [ClientRpc]
    void setMessageOnClientRpc(string message){
        newMessage = Instantiate(messagePre, Vector3.zero, Quaternion.identity);
        newMessage.GetComponent<Message>().setMessage(message);
        newMessage.GetComponent<NetworkObject>().Spawn();
    }
    public void createNewMessage(){
        sendMessageServerRpc("0,0,2,0,35'0\":35'0\"");
    }
    
}
