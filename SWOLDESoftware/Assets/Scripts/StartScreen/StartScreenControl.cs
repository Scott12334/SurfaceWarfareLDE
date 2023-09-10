using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
public class StartScreenControl : MonoBehaviourPun
{
    private int selectedShip;
    [SerializeField] private GameObject shipSelection, screenSelection, messagePre, textDisplay, messageHandlerGO;
    [SerializeField] private GameObject[] screensObject, screensCanvas;
    private int lastCount;
    private GameObject newMessage;
    private MessageHandler messageHandler;
    private bool[] shipsBlocked;
    [SerializeField] private GameObject clock, lightning, commsOffline, commsOnline;
    public bool toggled, untoggled, canRecieve;
    private List<string> messages;
    [SerializeField] private TMP_InputField passwordInput;
    // Start is called before the first frame update
    void Start()
    {
        canRecieve = false;
        lastCount = GameObject.FindGameObjectsWithTag("Message").Length; 
        messageHandler = messageHandlerGO.GetComponent<MessageHandler>();
        shipsBlocked = new bool[6]{false,false,false,false,false,false};
        messages = new List<string>();
        StartCoroutine(flashingCOMMS());
    }
    void Update(){ 
        for(int i = 0; i < messages.Count; i++){
            if(canRecieve){
                messageHandler.recieveMessage(messages[i]);
                messages.RemoveAt(i);
            }
        }
    }
    public void shipSelected(int shipId){
        this.selectedShip = shipId;
        //STAFF
        //Unsecure password check but doesn't need to be secure
        if(shipId == 6 && passwordInput.text == "SW0T1VAT3D"){
            messageHandler.setShipID(6);
            messageHandler.setPanelID(5);
            screensObject[5].SetActive(true);
            screensCanvas[5].SetActive(true);
            screensCanvas[7].SetActive(false);
            clock.GetComponent<Clock>().setCurrentPanel(5);
            canRecieve = true;
        }
        //DESCON
        else if(shipId == 7 && passwordInput.text == "$UBDR@FT"){
            messageHandler.setShipID(7);
            messageHandler.setPanelID(6);
            screensObject[6].SetActive(true);
            screensCanvas[6].SetActive(true);
            screensCanvas[7].SetActive(false);
            PlayerPrefs.SetInt("Ship",7);
            clock.GetComponent<Clock>().setCurrentPanel(6);
            canRecieve = true;
        }
        else if(shipId !=6 && shipId != 7){
            PlayerPrefs.SetInt("shipId", shipId);
            PlayerPrefs.SetInt("Ship",shipId);
            passwordInput.gameObject.SetActive(false);
            shipSelection.SetActive(false);
            screenSelection.SetActive(true);
        }
    }
    public void screenSelected(int screenID){
            PlayerPrefs.SetInt("Screen", screenID);
            string onlineMessage = selectedShip +","+ screenID+",1,1";
            Debug.Log(onlineMessage);
            sendMessage(onlineMessage);
            messageHandler.setShipID(selectedShip);
            messageHandler.setPanelID(screenID);
            screensObject[screenID].SetActive(true);
            screensCanvas[screenID].SetActive(true);
            screensCanvas[7].SetActive(false);
            clock.GetComponent<Clock>().setCurrentPanel(screenID);
            LabelControl.Instance.setLabel();
            canRecieve = true;
    }
    /*public void startHost(){
        NetworkManager.Singleton.StartHost();
    } */
    public void setToggle(bool disabled){toggled = disabled;}
    public void setUnToggle(bool enabled){untoggled = enabled;}
    IEnumerator flashingCOMMS(){
        while(true){
            while(toggled){
                lightning.SetActive(true);
                for(int i = 0; i < 6; i++){
                    commsOffline.SetActive(!commsOffline.activeSelf);
                    yield return new WaitForSeconds(0.5f);
                }
                lightning.SetActive(false);
                toggled = false;
            }
            while(untoggled){
                for(int i = 0; i < 6; i++){
                    commsOnline.SetActive(!commsOnline.activeSelf);
                    yield return new WaitForSeconds(0.5f);
                }
                untoggled = false;
            }
            yield return new WaitForSeconds(0.2f); 
        }
    }
    //Photon Solution
    public void sendMessage(string message){
        this.photonView.RPC("sendServerMessage", RpcTarget.AllBuffered, message);
    }
    [PunRPC]
    private void sendServerMessage(string message){
        messages.Add(message);
    }
    //Unity NetCode soultion
    /*
    [ClientRpc]
    void setMessageOnClientRpc(string message){
        newMessage = Instantiate(messagePre, Vector3.zero, Quaternion.identity);
        newMessage.GetComponent<Message>().setMessage(message);
        newMessage.GetComponent<NetworkObject>().Spawn();
    }
    [ServerRpc(RequireOwnership = false)]
    void sendMessageServerRpc(string message){
        string[] inputs= message.Split(",");
        if(inputs[2] == "4"){
            //Ship Blocked
            if(inputs[4] == "1"){
                shipsBlocked[int.Parse(inputs[3])] = true;
                setMessageOnClientRpc(inputs[3]+",disabled");
            }
            else if(inputs[4] == "0"){
                shipsBlocked[int.Parse(inputs[3])] = false;
                setMessageOnClientRpc(inputs[3]+",enabled");
            }
        }
        if(int.Parse(inputs[0]) < 6){
            if(shipsBlocked[int.Parse(inputs[0])] == false){
                setMessageOnClientRpc(message);
            }
        }
        else{
            setMessageOnClientRpc(message);
        }
    } */

    public void createNewMessage(){
        //35'0\":35'0\"
        Debug.Log("test");
        //sendMessageServerRpc("0,4,0,045,35'0\":35'0\",45,12,Schooner");
    }
    public int getShipID(){
        return selectedShip;
    }
    
}
