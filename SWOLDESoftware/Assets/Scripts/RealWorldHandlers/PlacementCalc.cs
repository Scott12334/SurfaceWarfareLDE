using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlacementCalc : MonoBehaviour
{
    [SerializeField]
    private GameObject marker1X, marker2X, marker1Y, marker2Y;
    [SerializeField]
    private float distanceX, distanceY;
    //Variables
    private double xNautMile;
    private double yNautMile;
    private double[] currentLoc; //[LatM, LatS, LonM, LonS]
    void Start()
    {
        determineDistance();
        currentLoc = new double[2];
    }
    public void determineDistance(){
        xNautMile = (marker2X.transform.position.x - marker1X.transform.position.x)/distanceX;
        yNautMile = (marker2Y.transform.position.y - marker1Y.transform.position.y)/distanceY;
        Destroy(marker1X);
        Destroy(marker2X);
        Destroy(marker1Y);
        Destroy(marker2Y);
    }
    public Vector3 calcWorldPos(string[] position, Vector3 currentPos){
        double latChange = CalculateLatChange(position);
        double lonChange = CalculateLonChange(position);
        Vector3 worldPos = new Vector3(currentPos.x + (float)(xNautMile*lonChange),currentPos.y + (float)(yNautMile*latChange),0);
        return worldPos;
    }
    public double CalculateLatChange(string[] positions)
    {
        double lat2 = ConvertDmsToDecimal(positions[0]);
        double change = lat2 - currentLoc[0];
        return change;
    }
    public double CalculateLonChange(string[] positions)
    {
        double lon2 = ConvertDmsToDecimal(positions[1]);
        double change = lon2 - currentLoc[1];
        return change;
    }
    private double ConvertDmsToDecimal(string dms)
    {
        string[] parts = dms.Split(' ','\'', '\"');
        double degrees = double.Parse(parts[0]);
        double minutes = double.Parse(parts[1]);
        double seconds = double.Parse(parts[2]);
        double decimalDegrees = (degrees*60) + minutes + (seconds / 60);
        return decimalDegrees;
    }
    public void setCurrentLoc(string[] latLon){
        currentLoc[0] = ConvertDmsToDecimal(latLon[0]);
        currentLoc[1] = ConvertDmsToDecimal(latLon[1]);
    }
    public bool inRange(string[] position, int range){
        double distance =0;
        double latChange = CalculateLatChange(position);
        double lonChange = CalculateLonChange(position);
        distance = Math.Sqrt((latChange*latChange)+(lonChange*lonChange));
        if(distance <= range){
            return true;
        }
        else{
            return false;
        }
    }
}
