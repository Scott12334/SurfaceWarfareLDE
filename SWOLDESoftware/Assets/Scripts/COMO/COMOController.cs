using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class COMOController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dials;
    private Dial[] dialScripts;
    bool debug=true;
    [SerializeField]
    private GameObject messageInput;
    private TextMeshProUGUI messageText;

    [SerializeField]
    private GameObject inputField;
    private TMP_InputField inputFieldText;

    int currentDestination=0;
    // Start is called before the first frame update
    void Start()
    {
        inputFieldText = inputField.GetComponent<TMP_InputField>();
        messageText = messageInput.GetComponent<TextMeshProUGUI>();
        messageText.enableWordWrapping=true;
        dialScripts= new Dial[dials.Length];
        for(int i=0; i<dials.Length; i++){
            dialScripts[i]=dials[i].GetComponent<Dial>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(debug){
            for(int i=0; i<dialScripts.Length; i++){
            switch(i){
                    case 0:
                    dialScripts[i].sendMessage("This is a Message from CIC");
                    dialScripts[i].sendMessage("This is a Message from CIC");
                    dialScripts[i].sendMessage("This is a Message from CIC");
                    break;

                    case 1:
                    dialScripts[i].sendMessage("This is a Message from DEFCON");
                    dialScripts[i].sendMessage("This is a Message from DEFCON");
                    dialScripts[i].sendMessage("This is a Message from DEFCON");
                    break;

                    case 2:
                    dialScripts[i].sendMessage("This is a Message from BRIDGE");
                    dialScripts[i].sendMessage("This is a Message from BRIDGE");
                    dialScripts[i].sendMessage("This is a Message from BRIDGE");
                    break;
                }
            }
            debug=false;
        }
    }
    public void sendMessage(){
        string destination= "";
        switch(currentDestination){
            case 0:
            destination="Press Button";
            break;

            case 1:
            destination="CIC";
            break;

            case 2:
            destination="DEFCON";
            break;

            case 3:
            destination="BRIDGE";
            break;
        }
        Debug.Log("Message to "+destination+":"+inputFieldText.text);
        inputFieldText.text="";
    }
    public void setCurrentDestination(int newDest){currentDestination=newDest;}
}
