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
    List<GameObject> enemies;
    string[] shipNames;
    // Start is called before the first frame update
    void Start()
    {
        setTexts();
        position= new float[2]{0,0};
        placementCalc = worldCalc.GetComponent<PlacementCalc>();
        enemies = new List<GameObject>();
        shipNames = new string[6]{"Stethem","Gettysburg","Chosin","Kidd","Galagher","Lake Erie"};
        startPosition();
    }

    public void sendPositionUpdate(){
        string positionString ="";
        positionString += "Ship 1 is at Latitude: "+latText.text;
        positionString += ", Longitutde: "+longText.text;
        positionString += ". With a speed of "+speedText.text;
        positionString += " knots. With a heading of "+headingText.text;
        placementCalc.setCurrentLoc(new string[]{latText.text, longText.text});
        currentShip.transform.eulerAngles= new Vector3(0,0,180-float.Parse(headingText.text.Trim()));
        compassPointer.transform.eulerAngles= new Vector3(0,0,-1*float.Parse(headingText.text.Trim()));
        for(int i=0 ; i<enemies.Count; i++){
            if(enemies[i] != null){
                enemies[i].transform.position=placementCalc.calcWorldPos(enemies[i].GetComponent<EnemyShip>().getLatLon(), currentShip.transform.position);
            }
        }
        updatePositionMessage();
    }
    public void updatePositionMessage(){
        string messageToServer = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        messageToServer += "6,";
        messageToServer += latText.text+":"+longText.text+",";
        messageToServer += headingText.text +",";
        messageToServer += speedText.text;
        Debug.Log(messageToServer);
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(messageToServer);
    }
    public void startPosition(){
        string[] startLoc = new string[6];
        //STEH
        startLoc[0] = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header()+"6,7 44'45\":135 6'45\",235,20";
        //GET
        startLoc[1] = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header()+"6,7 40'30\":135 6'15\",235,20";
        //CHOSIN
        startLoc[2] = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header()+"6,7 40'00\":134 59'15\",235,20";
        //KIDD
        startLoc[3] = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header()+"6,7 45'15\":134 59'45\",235,20";
        //GALAGHER
        startLoc[4] = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header()+"6,7 38'30\":135 2'45\",235,20";
        //ERIE
        startLoc[5] = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header()+"6,7 46'15\":135 3'30\",235,20";
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(startLoc[GameObject.Find("SimController").GetComponent<StartScreenControl>().getShipID()]);
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
        string messageToServer = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        messageToServer += "3,";
        messageToServer += inputText.text;
        Debug.Log(messageToServer);
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(messageToServer);
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
    //MessageType,#,Lat:Lon,Heading,Speed, Type
    public void recieveMessage(string message){
        string[] inputs= message.Split(",");
        if(GameObject.Find("SimController").GetComponent<StartScreenControl>().getShipID() != int.Parse(inputs[0]) && inputs[2] == "6"){
            string[] latLon= new string[2];
            for(int i=0; i<inputs.Length; i++){
                if(inputs[i].Contains(":")){
                    latLon = inputs[i].Split(":");
                }
            }
            if(GameObject.Find(shipNames[int.Parse(inputs[0])]) == null){
                GameObject newEnemy = Instantiate(enemyPre, placementCalc.calcWorldPos(latLon, currentShip.transform.position), Quaternion.identity);
                EnemyShip newContact= newEnemy.GetComponent<EnemyShip>();
                newContact.name = shipNames[int.Parse(inputs[0])];
                string[] contactinfo = new string[8]{"Unknown","Unknown","Unknown","Unknown","Unknown","Unknown","Unknown","Unknown"};
                newContact.setContactValue(contactinfo,latLon);
                enemies.Add(newEnemy);
            }
            else{
                EnemyShip newContact= GameObject.Find(shipNames[int.Parse(inputs[0])]).GetComponent<EnemyShip>();
                newContact.setPos(latLon);
                GameObject.Find(shipNames[int.Parse(inputs[0])]).transform.position = placementCalc.calcWorldPos(latLon, currentShip.transform.position);
            }
        }
        if(inputs[2] == "10"){
            enemies.Remove(GameObject.Find(inputs[3]));
            Destroy(GameObject.Find(inputs[3]));
        }
        if(inputs[1] == "4"){
            //Contacts
            if(inputs[2] == "0"){
                string[] latLon= new string[2];
                for(int i=0; i<inputs.Length; i++){
                    if(inputs[i].Contains(":")){
                        latLon = inputs[i].Split(":");
                    }
                }
                EnemyShip newContact= GameObject.Find(inputs[3]).GetComponent<EnemyShip>();
                newContact.setContactValue(inputs,latLon);
                GameObject.Find(inputs[3]).transform.position = placementCalc.calcWorldPos(latLon, currentShip.transform.position);
            }
        }
        else if(inputs[1] == "3"){
            //Firing Solution
            if(inputs[2] == "7"){
                string firingSolution ="";
                firingSolution += inputs[3]+",";
                firingSolution += inputs[4]+",";
                firingSolution += inputs[5]+",";
                firingSolution += inputs[6]+",";
                firingSolution += inputs[7];
                sendSolution(firingSolution);
            }
        }
        else if(inputs[1] == "5"){
            if(inputs[2] == "0"){
                string[] latLon= new string[2];
                for(int i=0; i<inputs.Length; i++){
                    if(inputs[i].Contains(":")){
                        latLon = inputs[i].Split(":");
                    }
                }
                if(GameObject.Find(inputs[3]) == null){
                    GameObject newEnemy = Instantiate(enemyPre, placementCalc.calcWorldPos(latLon, currentShip.transform.position), Quaternion.identity);
                    EnemyShip newContact= newEnemy.GetComponent<EnemyShip>();
                    newContact.name = inputs[3];
                    string[] contactinfo = new string[8]{"Unknown","Unknown","Unknown","Unknown","Unknown","Unknown","Unknown","Unknown"};
                    newContact.setContactValue(contactinfo,latLon);
                    enemies.Add(newEnemy);
                }
                else{
                    EnemyShip newContact= GameObject.Find(inputs[3]).GetComponent<EnemyShip>();
                    newContact.setPos(latLon);
                    GameObject.Find(inputs[3]).transform.position = placementCalc.calcWorldPos(latLon, currentShip.transform.position);
                }
            }
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
        string messageToServer = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        messageToServer += "5,";
        messageToServer += currentEnemy.name;
        Debug.Log(messageToServer);
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(messageToServer);
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
