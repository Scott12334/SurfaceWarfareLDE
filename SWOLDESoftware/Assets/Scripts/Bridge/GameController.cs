using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject CICSendMessage;
    private TextMeshProUGUI inputText;
    //Position Inputs
    //LAT
    [SerializeField]
    private GameObject LatInput,LongInput,SpeedInput,HeadingInput;
    private TextMeshProUGUI latText;
    private TextMeshProUGUI longText;
    private float[] position;
    private TextMeshProUGUI speedText;
    private TMP_InputField headingText;
    [SerializeField]
    private GameObject fromCICMessage,canvasFromCIC,currentShip,compassPointer;
    private TextMeshProUGUI fromCICMessageText;
    //Target Screen
    [SerializeField]
    private GameObject targetScreenCanvas, worldCalc, enemyPre, currentEnemy, targetButtons;
    [SerializeField]
    private GameObject[] ammoObjects;
    private PlacementCalc placementCalc;
    // Start is called before the first frame update
    void Start()
    {
        setTexts();
        position= new float[2]{0,0};
        placementCalc = worldCalc.GetComponent<PlacementCalc>();
    }

    public void sendPositionUpdate(){
        string positionString ="";
        positionString += "Ship 1 is at Latitude: "+latText.text;
        positionString += ", Longitutde: "+longText.text;
        positionString += ". With a speed of "+speedText.text;
        positionString += " knots. With a heading of "+headingText.text;
        placementCalc.setCurrentLoc(new string[]{latText.text, longText.text});
        //getHeadingFromString(headingText.text.Trim());
        currentShip.transform.eulerAngles= new Vector3(0,0,180-float.Parse(headingText.text.Trim()));
        compassPointer.transform.eulerAngles= new Vector3(0,0,-1*float.Parse(headingText.text.Trim()));
        Debug.Log(positionString);
    }
    public void getMinutesSeconds(string inputText){
        string[] minutesSeconds= inputText.Split("'");
        float minutes=float.Parse(minutesSeconds[0]);
        minutesSeconds[1]= minutesSeconds[1].Substring(0,minutesSeconds[1].Length-1);
        float seconds= float.Parse(minutesSeconds[1]);
        Debug.Log("minutes: "+ minutes);
        Debug.Log("Seconds: "+ seconds);

    }
    public void sendCICMessage(){
        Debug.Log(inputText.text);
    }

    private void setTexts(){
        inputText= CICSendMessage.GetComponent<TextMeshProUGUI>();
        inputText.enableWordWrapping = true;
        
        latText= LatInput.GetComponent<TextMeshProUGUI>();
        longText= LongInput.GetComponent<TextMeshProUGUI>();
        speedText= SpeedInput.GetComponent<TextMeshProUGUI>();
        headingText= HeadingInput.GetComponent<TMP_InputField>();
        fromCICMessageText= fromCICMessage.GetComponent<TextMeshProUGUI>();
    }

    //CODE FOR CIC MESSAGE
    public void cicButton(){
        if(targetScreenCanvas.activeSelf){
            targetScreenCanvas.SetActive(false);
            targetButtons.SetActive(false);
        }
        else{
            canvasFromCIC.SetActive(!canvasFromCIC.activeSelf);
        }
    }
    //MessageType,#,Lat:Lon,Heading,Speed
    public void recieveCIC(string message){
        canvasFromCIC.SetActive(true);
        fromCICMessageText.text=message;
        string[] inputs= message.Split(",");
        if(inputs[0] == "0"){
            string[] latLon= new string[2];
            for(int i=0; i<inputs.Length; i++){
                if(inputs[i].Contains(":")){
                    latLon = inputs[i].Split(":");
                }
            }
            GameObject newEnemy = Instantiate(enemyPre, placementCalc.calcWorldPos(latLon, currentShip.transform.position), Quaternion.identity);
            EnemyShip newContact= newEnemy.GetComponent<EnemyShip>();
            newContact.name = inputs[1];
            newContact.setContactValue(inputs,latLon);
        }
    }
    public void setSelectedEnemy(GameObject selectedShip){
        currentEnemy = selectedShip;
    }
    public void sendSolution(string stringAmmo){
        string[] ammoNumbers =stringAmmo.Split(",");
        int[] ammo = new int[5]{int.Parse(ammoNumbers[0]),int.Parse(ammoNumbers[1]),int.Parse(ammoNumbers[2]),int.Parse(ammoNumbers[3]),int.Parse(ammoNumbers[4])};
        EnemyShip selectedEnemy = GameObject.Find(ammoNumbers[0]).GetComponent<EnemyShip>();
        selectedEnemy.firingSolution(ammo);
    }
    public void targetButtonActive(){targetButtons.SetActive(true);}
    public void requestSolution(){
        Debug.Log("Bridge Requesting Firing Solution for Contact "+currentEnemy.name);
    }
    public void fire(){
        EnemyShip selectedEnemy = currentEnemy.GetComponent<EnemyShip>();
        int[] ammo=selectedEnemy.getAmmo();
        cicButton();
        StartCoroutine(attack(ammo));
    }
    IEnumerator attack(int[] ammo){
        for(int i=1; i<ammo.Length; i++){
            for(int j=0; j<ammo[i]; j++){
                GameObject spawnedBullet = Instantiate(ammoObjects[i-1], currentShip.transform.position, Quaternion.identity);
                Vector3 direction = (currentEnemy.transform.position - spawnedBullet.transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
                if(i!=1){rotation *= Quaternion.Euler(0,0,45);}
                else{rotation *= Quaternion.Euler(0,0,90);}
                spawnedBullet.transform.rotation= rotation;
                Rigidbody2D rigidbody2D= spawnedBullet.GetComponent<Rigidbody2D>();
                rigidbody2D.AddForce(direction *5, ForceMode2D.Impulse);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
