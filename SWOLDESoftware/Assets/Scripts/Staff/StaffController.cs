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
    private GameObject placementControl, locationMarker, shipHolder, shipScreen, shipScreenInteract, toggleCommsObject;
    [SerializeField]
    private GameObject nameDisplay;
    private TextMeshProUGUI nameText;
    private PlacementCalc placementCalc;
    private int currentShip;
    private bool firstFrame = false;
    private bool commsActive = true;
    private Ship[] ships;
    // Start is called before the first frame update
    void Start()
    {
        placementCalc = placementControl.GetComponent<PlacementCalc>();
        ships= new Ship[6];
        names = new string[6]{"Sampson","Gettysburg","Chosin","Kidd","Galagher","Lake Erie"};
        nameText = nameDisplay.GetComponent<TextMeshProUGUI>();
        initShips();
    }
    private void Update() {
        if(!firstFrame){
            placementCalc.setCurrentLoc(new string[]{"30'0\"", "30'0\""});
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
        if(inputs[0] == "0"){
            string[] latLon= new string[2];
            for(int i=0; i<inputs.Length; i++){
                if(inputs[i].Contains(":")){
                    latLon = inputs[i].Split(":");
                }
            }
            fleet[int.Parse(inputs[0])].transform.position = placementCalc.calcWorldPos(latLon, locationMarker.transform.position);
        }
        if(inputs[0] == "1"){
            int shipNumber = int.Parse(inputs[1]);
            int panelNumber = int.Parse(inputs[2]);
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
    public void showMap(){
        shipHolder.SetActive(true);
        shipScreen.SetActive(false);
        shipScreenInteract.SetActive(false);
    }
    public void toggleShipScreen(){
        shipHolder.SetActive(!shipHolder.activeSelf);
        shipScreen.SetActive(!shipScreen.activeSelf);
        shipScreenInteract.SetActive(!shipScreenInteract.activeSelf);
    }
    public void toggleComms(){
        commsActive = !commsActive;
        if(commsActive){
            Debug.Log("Enable Comms for Ship-"+currentShip);
            toggleCommsObject.GetComponent<Image>().color = Color.green;
        }
        else{
            Debug.Log("Disable Comms for Ship-"+currentShip);
            toggleCommsObject.GetComponent<Image>().color = Color.red;
        }
        ships[currentShip].setCommsActive(commsActive);
    }
}
