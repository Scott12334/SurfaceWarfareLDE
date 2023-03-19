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
        string[] latLon = initLoc.Split(",");
        currentPos = new float[2]{ConvertDmsToDecimal(latLon[0]),ConvertDmsToDecimal(latLon[1])};
        this.type = type;
        calcChangeVars();
    }
    private float ConvertDmsToDecimal(string dms)
    {
        string[] parts = dms.Split('\'', '\"');
        float minutes = float.Parse(parts[0]);
        float seconds = float.Parse(parts[1]);
        float decimalDegrees = minutes + (seconds / 60);
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
        position += (int)currentPos[0]+"'";
        position += (int)((currentPos[0] % 1.0)*60)+"\":";
        position += (int)currentPos[1]+"'";
        position += (int)((currentPos[1] % 1.0)*60)+"\"";
        return position;
    }
    public string[] getContactInfo(){
        contactinfo = new string[6];
        string[] currentLatLon = getPosition().Split(":");
        contactinfo[0] = contactNumber;
        contactinfo[1] = currentLatLon[0];
        contactinfo[2] = currentLatLon[1];
        contactinfo[3] = bearing+"";
        contactinfo[4] = (actualSpeed*12)+"";
        contactinfo[5] = type;
        return contactinfo;
    }
    public string getMessage(){
        string fullMessage="0,";
        fullMessage += getPosition()+",";
        fullMessage += bearing+",";
        fullMessage += actualSpeed*12+",";
        fullMessage += type;
        return fullMessage;
    }
    public string getContactNumber(){return contactNumber;}
    public void setPosition(float newPos, int latLon){
        currentPos[latLon] = newPos;
    }
    public void setHeading(int heading){
        this.bearing = heading;
    }
    public void setSpeed(float newSpeed){
        this.actualSpeed = newSpeed/12;
    }
    public void setType(string type){
        this.type = type;
    }
}
