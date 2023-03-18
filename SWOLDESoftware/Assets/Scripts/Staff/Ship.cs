using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    private string shipName;
    private int id;
    private bool[] panelActive;
    private bool commsActive;
    public Ship(string shipName, int id){
        this.shipName = shipName;
        this.id = id;
        panelActive = new bool[5]{false,false,false,false,false};
        commsActive = true;
    }
    public void setPanelActive(int panel, int state){
        if(state == 0){
            this.panelActive[panel] = false;
        }
        else{
            this.panelActive[panel] = true;
        }
    }
    public bool[] getPanelStates(){
        return panelActive;
    }
    public void setCommsActive(bool commsActive){
        this.commsActive = commsActive;
    }
    public bool getCommsActive(){return commsActive;}
    public string getName(){return shipName;}
}
