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
    private GameObject LatInput;
    private TextMeshProUGUI latText;
    //LON
    [SerializeField]
    private GameObject LongInput;
    private TextMeshProUGUI longText;

    private float[] position;
    //SPEED
    [SerializeField]
    private GameObject SpeedInput;
    private TextMeshProUGUI speedText;
    //HEADING
    [SerializeField]
    private GameObject HeadingInput;
    private TMP_InputField headingText;
    //COMPASS
    [SerializeField]
    private GameObject compassPointer;
    //SHIP
    [SerializeField]
    private GameObject currentShip;
    //FROM CIC MESSAGE
    [SerializeField]
    private GameObject canvasFromCIC;
    [SerializeField]
    private GameObject gameFromCIC;
    [SerializeField]
    private GameObject fromCICMessage;
    private TextMeshProUGUI fromCICMessageText;
    //Target Screen
    [SerializeField]
    private GameObject targetScreenGame;
    [SerializeField]
    private GameObject targetScreenCanvas;
    // Start is called before the first frame update
    void Start()
    {
        setTexts();
        position= new float[2]{0,0};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendPositionUpdate(){
        string positionString ="";
        positionString += "Ship 1 is at Latitude: "+latText.text;
        positionString += ", Longitutde: "+longText.text;
        positionString += ". With a speed of "+speedText.text;
        positionString += " knots. With a heading of "+headingText.text;
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
            targetScreenGame.SetActive(false);
        }
        else{
            canvasFromCIC.SetActive(!canvasFromCIC.activeSelf);
            gameFromCIC.SetActive(!gameFromCIC.activeSelf);
        }
    }
    public void recieveCIC(string message){
        canvasFromCIC.SetActive(true);
        gameFromCIC.SetActive(true);
        fromCICMessageText.text=message;
    }
}
