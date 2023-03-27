using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StaffController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] fleet, viewPanels;
    private string[] names;
    [SerializeField]
    private GameObject placementControl, locationMarker, shipHolder, shipScreen, shipScreenInteract, toggleCommsObject, contactScreen, contactInter, contactEnter, contactControl;
    [SerializeField]
    private GameObject nameDisplay;
    private TextMeshProUGUI nameText;
    private PlacementCalc placementCalc;
    private int currentShip;
    private bool firstFrame = false;
    private bool commsActive = true;
    private Ship[] ships;
    bool clockRunning;
    // Start is called before the first frame update
    void Start()
    {
        clockRunning = false;
        placementCalc = placementControl.GetComponent<PlacementCalc>();       
        ships= new Ship[6];
        names = new string[6]{"Stethem","Gettysburg","Chosin","Kidd","Galagher","Lake Erie"};
        nameText = nameDisplay.GetComponent<TextMeshProUGUI>();
        initShips();
    }
    private void Update() {
        if(!firstFrame){
            placementCalc.setCurrentLoc(new string[]{"7 30'0\"", "134 30'0\""});
            firstFrame = true;
        }
    }
    private void initShips(){
        for(int i=0; i<ships.Length; i++){
            ships[i] = new Ship(names[i], i);
        }
    }
    public void recieveMessage(string message){
        string[] inputs= message.Split(",");
        if(inputs[2] == "6"){
            string[] latLon= new string[2];
            for(int i=0; i<inputs.Length; i++){
                if(inputs[i].Contains(":")){
                    latLon = inputs[i].Split(":");
                }
            }
            fleet[int.Parse(inputs[0])].transform.position = placementCalc.calcWorldPos(latLon, locationMarker.transform.position);
            fleet[int.Parse(inputs[0])].transform.eulerAngles= new Vector3(0,0,180-float.Parse(inputs[4]));
        }
        if(inputs[2] == "1"){
            int shipNumber = int.Parse(inputs[0]);
            int panelNumber = int.Parse(inputs[1]);
            int stateNumber = int.Parse(inputs[3]);
            ships[shipNumber].setPanelActive(panelNumber, stateNumber);
            setPanels();
        }
    }
    public void shipButtonPressed(int ship){
        currentShip = ship;
        if(!shipScreen.activeSelf){
            toggleShipScreen();
        }
        setPanels();
        nameText.text = "USS "+ships[currentShip].getName();
    }
    public void setPanels(){
        Ship selectedShip = ships[currentShip];
        if(selectedShip.getCommsActive()){
            toggleCommsObject.GetComponent<Image>().color = Color.green;
        }
        else{
            toggleCommsObject.GetComponent<Image>().color = Color.red;
        }
        bool[] panelStates= selectedShip.getPanelStates();
        for(int i=0 ;i<panelStates.Length; i++){
            if(panelStates[i]){
                viewPanels[i].GetComponent<Image>().color = Color.green;
            }
            else{
                viewPanels[i].GetComponent<Image>().color = Color.red;
            }
        }
    }
    public void toggleContact(){
        shipHolder.SetActive(!shipHolder.activeSelf);
        contactInter.SetActive(!contactInter.activeSelf);
        contactScreen.SetActive(!contactScreen.activeSelf);
    }
    public void showMap(){
        shipHolder.SetActive(true);
        shipScreen.SetActive(false);
        shipScreenInteract.SetActive(false);
        contactInter.SetActive(false);
        contactScreen.SetActive(false);
    }
    public void toggleShipScreen(){
        shipHolder.SetActive(!shipHolder.activeSelf);
        shipScreen.SetActive(!shipScreen.activeSelf);
        shipScreenInteract.SetActive(!shipScreenInteract.activeSelf);
        contactInter.SetActive(false);
        contactScreen.SetActive(false);
        if(shipScreen.activeSelf){
            shipHolder.SetActive(false);
        }
    }
    public void toggleComms(){
        commsActive = !commsActive;
        if(commsActive){
            Debug.Log("Enable Comms for Ship-"+currentShip);
            toggleCommsObject.GetComponent<Image>().color = Color.green;
            string toggleMessage = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
            toggleMessage += "4,"+currentShip+","+"0";
            GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(toggleMessage);
            Debug.Log(toggleMessage);
        }
        else{
            Debug.Log("Disable Comms for Ship-"+currentShip);
            toggleCommsObject.GetComponent<Image>().color = Color.green;
            string toggleMessage = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
            toggleMessage += "4,"+currentShip+","+"1";
            GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(toggleMessage);
            Debug.Log(toggleMessage);
            toggleCommsObject.GetComponent<Image>().color = Color.red;
        }
        ships[currentShip].setCommsActive(commsActive);
    }
    public void startClock(){
        string clockMessage = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        clockMessage += "9,";
        if(!clockRunning){
            clockRunning = true;
            clockMessage += "1";
        }
        else{
            clockRunning = false;
            clockMessage += "0";
        }
        contactControl.GetComponent<ContactControl>().startTheClock();
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(clockMessage);
    }
    public void destoryContact(){
        string message = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        message += "10,";
        message += contactEnter.GetComponent<TMP_InputField>().text;
        Debug.Log(message);
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(message);
    }
}
