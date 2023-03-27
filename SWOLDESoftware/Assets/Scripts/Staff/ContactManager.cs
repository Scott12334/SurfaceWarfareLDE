using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactManager
{
    private float actualSpeed;
    private float[] currentPos;
    private int bearing;
    private string contactNumber;
    private string initLoc;
    private string type;
    private float[] changeVar;
    private string[] contactinfo;
    public ContactManager(string contactNumber, int bearing, float speed, string initLoc, string type){
        actualSpeed = speed/12;
        this.bearing = bearing;
        this.contactNumber = contactNumber;
        this.initLoc = initLoc;
        changeVar = new float[2];
        string[] latLon = initLoc.Split(":");
        currentPos = new float[2]{ConvertDmsToDecimal(latLon[0]),ConvertDmsToDecimal(latLon[1])};
        this.type = type;
        calcChangeVars();
    }
    public int getBearing(){
        return bearing;
    }
    private float ConvertDmsToDecimal(string dms)
    {
        string[] parts = dms.Split(' ','\'', '\"');
        float degrees = float.Parse(parts[0]);
        float minutes = float.Parse(parts[1]);
        float seconds = float.Parse(parts[2]);
        float decimalDegrees = (degrees*60) + minutes + (seconds / 60);
        return decimalDegrees;
    }
    void calcChangeVars(){
        changeVar = new float[2];   
        //Change in lat
        changeVar[0] = actualSpeed * (Mathf.Cos(bearing * Mathf.Deg2Rad));
        //Change in lon
        changeVar[1] = actualSpeed * (Mathf.Sin(bearing * Mathf.Deg2Rad));
    }
    public void updatePos(){
        currentPos[0] += changeVar[0];
        currentPos[1] += changeVar[1];
    }
    public string getPosition(){
        string position ="";
        position += (int)(currentPos[0]/60)+" ";
        position += (int)(((currentPos[0]/60) % 1.0)*60)+"'";
        position += (int)(((((currentPos[0]/60) % 1.0)*60) % 1.0)*60)+"\":";
        position += (int)(currentPos[1]/60)+" ";
        position += (int)(((currentPos[1]/60) % 1.0)*60)+"'";
        position += (int)(((((currentPos[1]/60) % 1.0)*60) % 1.0)*60)+"\"";
        return position;
    }
    public string getMessage(){
        string fullMessage="0,";
        fullMessage += contactNumber+",";
        fullMessage += getPosition()+",";
        fullMessage += bearing+",";
        fullMessage += actualSpeed*12+",";
        fullMessage += type;
        return fullMessage;
    }
    public string getContactNumber(){return contactNumber;}
}
