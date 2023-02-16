using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dial : MonoBehaviour
{
    //Messages
    ArrayList messages= new ArrayList();
    int currentMessage=0;
    //HEADING
    [SerializeField]
    private GameObject MessageDisplay;
    private TextMeshProUGUI messageText;
    
    // Start is called before the first frame update
    void Start()
    {
        messageText= MessageDisplay.GetComponent<TextMeshProUGUI>();
    }
    private void OnMouseDown() {
        if(messages.Count >0){
            if(currentMessage < messages.Count-1){
                currentMessage++;
            }
            else{
                currentMessage=0;
            }
            messageText.text= (string)messages[currentMessage];
            float zRotation = -1*(currentMessage+1)*(360/messages.Count);
            this.transform.eulerAngles= new Vector3(0,0,zRotation);
        }
    }
    public void sendMessage(string newMessage){
        string recieveMessage = "Message -"+(messages.Count+1)+"\n"+newMessage;
        messages.Add(recieveMessage);
        if(messages.Count!=1){
            currentMessage++;
        }
        messageText.text= (string)messages[currentMessage];
    }
}
