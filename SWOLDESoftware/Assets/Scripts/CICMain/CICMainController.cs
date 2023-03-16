using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CICMainController : MonoBehaviour
{
    [SerializeField]
    private GameObject firingSolutionMessage;
    private TextMeshProUGUI firingSolutionText;
    [SerializeField]
    private GameObject[] shotsLeft;
    private TextMeshProUGUI[] shotsLeftText;
    //Missile Arrays 0=GUN, 1=SAM, 2=SSM, 3=KID
    [SerializeField]
    private int[] firingSolution;
    private int[] ammo;
    string currentContact="000";
    [SerializeField]
    private GameObject[] dials;
    private Dial[] dialScripts;
    [SerializeField]
    private GameObject contactInput;
    private TMP_InputField contactInputText;
    bool debug=true;
    // Start is called before the first frame update
    void Start()
    {
        ammo= new int[]{50,50,50,1};
        firingSolution= new int[4];
        firingSolutionText = firingSolutionMessage.GetComponent<TextMeshProUGUI>();
        contactInputText= contactInput.GetComponent<TMP_InputField>();
        shotsLeftText= new TextMeshProUGUI[shotsLeft.Length];
        dialScripts= new Dial[dials.Length];
        for(int i=0; i<dials.Length; i++){
            dialScripts[i]=dials[i].GetComponent<Dial>();
        }
        for(int i=0; i<shotsLeft.Length; i++){
            shotsLeftText[i]= shotsLeft[i].GetComponent<TextMeshProUGUI>();
            shotsLeftText[i].text= "Shots Left: "+ammo[i];
        }
        updateText();
    }

    // Update is called once per frame
    void Update()
    {
        if(debug){
            for(int i=0; i<dialScripts.Length; i++){
            switch(i){
                    case 0:
                    dialScripts[i].sendMessage("This is the first message from bridge");
                    dialScripts[i].sendMessage("This is another message from the bridge");
                    dialScripts[i].sendMessage("This is the final message from the bridge");
                    break;

                    case 1:
                    dialScripts[i].sendMessage("This is the first message from COMO");
                    dialScripts[i].sendMessage("This is another message from the COMO");
                    dialScripts[i].sendMessage("This is the final message from the COMO");
                    break;
                }
            }
            debug=false;
        }
    }

    public void upButtonPressed(int missle){
        if(ammo[missle] >0){
            if(missle==0 || missle==3){
                if(firingSolution[missle]>0){

                }
                else{
                    firingSolution[missle] ++;
                    ammo[missle] --;
                    updateText();
                }
            }
            else{
                firingSolution[missle] ++;
                ammo[missle] --;
                updateText();
            }
        }
    }
    public void downButtonPressed(int missle){
        if(firingSolution[missle] >0){
            firingSolution[missle] --;
            ammo[missle] ++;
            updateText();
        }
    }
    public void updateText(){
        string firingSolutionUpdate = "Firing Solution for Contact ";
        firingSolutionUpdate+= currentContact+ ":"+"\n";
        if(firingSolution[0] > 0){firingSolutionUpdate+= "Fire 5-Inch Gun"+"\n";}
        if(firingSolution[1] > 0){firingSolutionUpdate+= "Fire "+firingSolution[1]+ " SAM(s)"+"\n";}
        if(firingSolution[2] > 0){firingSolutionUpdate+= "Fire "+firingSolution[2]+ " SSM(s)"+"\n";}
        if(firingSolution[3] > 0){firingSolutionUpdate+= "Fire KID"+"\n";}
        firingSolutionText.text = firingSolutionUpdate;
        
        for(int i=0; i<shotsLeftText.Length; i++){
            shotsLeftText[i].text= "Shots Left: "+ammo[i];
        }
    }
    public void sendFiringSolution(){
        Debug.Log(firingSolutionText.text);
        for(int i=0; i<firingSolution.Length; i++){
            firingSolution[i]=0;
        }
        updateText();
    }
    public void setContact(){
        currentContact= contactInputText.text;
        contactInputText.text= "";
        updateText();
    }
}
