using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DESCON : MonoBehaviour
{
    [SerializeField]
    private GameObject[] fleet;
    [SerializeField]
    private GameObject placementControl, locationMarker;
    private PlacementCalc placementCalc;
    private bool firstFrame = false;
    // Start is called before the first frame update
    void Start()
    {
        placementCalc = placementControl.GetComponent<PlacementCalc>();
    }
    private void Update() {
        if(!firstFrame){
            placementCalc.setCurrentLoc(new string[]{"30'0\"", "30'0\""});
            firstFrame = true;
        }
    }
    public void recieveLocationUpdate(string message){
        string[] inputs= message.Split(",");
        string[] latLon= new string[2];
        for(int i=0; i<inputs.Length; i++){
            if(inputs[i].Contains(":")){
                latLon = inputs[i].Split(":");
            }
        }
        Debug.Log(latLon[0]);
        fleet[int.Parse(inputs[0])].transform.position = placementCalc.calcWorldPos(latLon, locationMarker.transform.position);
    }
}
