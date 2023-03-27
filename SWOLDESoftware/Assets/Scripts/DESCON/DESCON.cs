using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DESCON : MonoBehaviour
{
    [SerializeField]
    private GameObject[] fleet;
    [SerializeField]
    private GameObject placementControl, locationMarker, contactPre, lincoln;
    private PlacementCalc placementCalc;
    private bool firstFrame = false;
    // Start is called before the first frame update
    void Start()
    {
        placementCalc = placementControl.GetComponent<PlacementCalc>();
    }
    private void Update() {
        if(!firstFrame){
            placementCalc.setCurrentLoc(new string[]{"7 30'0\"", "134 30'0\""});
            firstFrame = true;
        }
    }
    public void recieveMessage(string message){
        string[] inputs= message.Split(",");
        if(inputs[2] == "0"){
            string[] latLon= new string[2];
            for(int i=0; i<inputs.Length; i++){
                if(inputs[i].Contains(":")){
                    latLon = inputs[i].Split(":");
                }
            }
            if(GameObject.Find(inputs[3]) == null){
                GameObject newContact = Instantiate(contactPre, placementCalc.calcWorldPos(latLon, locationMarker.transform.position),Quaternion.identity);            
                newContact.name = inputs[3];
            }
            else{
                if(inputs[3] == "Lincoln"){
                    lincoln.transform.position = placementCalc.calcWorldPos(latLon, locationMarker.transform.position);
                }
                else{
                    GameObject.Find(inputs[3]).transform.position = placementCalc.calcWorldPos(latLon, locationMarker.transform.position);
                }
            }
        }
        else if(inputs[2] == "6"){
            string[] latLon= new string[2];
            for(int i=0; i<inputs.Length; i++){
                if(inputs[i].Contains(":")){
                    latLon = inputs[i].Split(":");
                }
            }
            fleet[int.Parse(inputs[0])].transform.position = placementCalc.calcWorldPos(latLon, locationMarker.transform.position);
            fleet[int.Parse(inputs[0])].transform.eulerAngles= new Vector3(0,0,180-float.Parse(inputs[4]));
        }
    }
}
