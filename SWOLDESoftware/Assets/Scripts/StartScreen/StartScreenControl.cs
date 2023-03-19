using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;
public class StartScreenControl : NetworkBehaviour
{
    private int ship;
    [SerializeField]
    private GameObject shipSelection, screenSelection, messagePre, textDisplay;
    private int lastCount;
    private GameObject newMessage;
    // Start is called before the first frame update
    void Start()
    {
        lastCount = GameObject.FindGameObjectsWithTag("Message").Length; 
    }
    void Update(){ 
        int currentCount = GameObject.FindGameObjectsWithTag("Message").Length;
        if(currentCount != lastCount){
            GameObject[] messages = GameObject.FindGameObjectsWithTag("Message");
            textDisplay.GetComponent<TextMeshProUGUI>().text = messages[messages.Length-1].GetComponent<Message>().getMessage();
        }
    }
    public void shipSelected(int shipId){
        this.ship = shipId+1;
        //STAFF
        if(shipId == 5){
            onClickJoin();
            SceneManager.LoadScene(shipId);
        }
        //DESCON
        else if(shipId == 6){
            onClickJoin();
            SceneManager.LoadScene(shipId);
        }
        else{
            PlayerPrefs.SetInt("shipId", shipId);
            shipSelection.SetActive(false);
            screenSelection.SetActive(true);
        }
    }
    public void screenSelected(int screenID){
        onClickJoin();
        SceneManager.LoadScene(screenID);
    }
    public void OnClickHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void onClickJoin(){
        NetworkManager.Singleton.StartClient();
    }

    [ClientRpc]
    void setMessageOnClientRpc(string message){
        newMessage = Instantiate(messagePre, Vector3.zero, Quaternion.identity);
        newMessage.GetComponent<Message>().setMessage(message);
        newMessage.GetComponent<NetworkObject>().Spawn();
    }
    public void createNewMessage(){
        setMessageOnClientRpc(Random.Range(0,100)+ "Message Test");
    }
    
}
