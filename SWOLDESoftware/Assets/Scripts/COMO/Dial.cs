using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dial : MonoBehaviour
{
    //Messages
    ArrayList messages= new ArrayList();
    int currentMessage=0;
    //Forward or backward
    [SerializeField]
    bool backward;
    //HEADING
    [SerializeField]
    private GameObject MessageDisplay;
    private TextMeshProUGUI messageText;
    [SerializeField]
    private GameObject forwardButton;
    private Dial forwardScript;
    private int dialCurrentRotate=0;
    // Start is called before the first frame update
    void Start()
    {
        messageText= MessageDisplay.GetComponent<TextMeshProUGUI>();
        if(backward){
            forwardScript = forwardButton.GetComponent<Dial>();
        }
    }
    private void OnMouseDown() {
        if(backward){
            currentMessage = forwardScript.getCurrentMessage();
            if(forwardScript.getMessages() >0){
                if(currentMessage > 0){
                    currentMessage--;
                    forwardScript.setCurrentMessage(this.currentMessage);
                }
                else{
                    currentMessage= forwardScript.getMessages()-1;
                    forwardScript.setCurrentMessage(this.currentMessage);
                }
                rotate();
            }
        }
        else{
            if(messages.Count > 0){
                if(currentMessage < messages.Count-1){
                    currentMessage++;
                }
                else{
                    currentMessage=0;
                }
                rotate();
                messageText.text= (string)messages[currentMessage];
            }
        }
    }
    public void rotate(){
        if(backward){
            float zRotation = -1*(dialCurrentRotate+1)*(360/forwardScript.getMessages());
            this.transform.eulerAngles= new Vector3(0,0,-1*zRotation);
        }
        else{
            float zRotation = -1*(dialCurrentRotate+1)*(360/messages.Count);
            this.transform.eulerAngles= new Vector3(0,0,zRotation);
        }
        dialCurrentRotate++;
    }
    public void sendMessage(string newMessage){
        string recieveMessage = "Message -"+(messages.Count+1)+"\n"+newMessage;
        messages.Add(recieveMessage);
        if(messages.Count!=1){
            currentMessage++;
        }
        messageText.text= (string)messages[currentMessage];
    }
    public void setCurrentMessage(int newMessage){
        currentMessage=newMessage;

        messageText.text= (string)messages[currentMessage];
    }
    public int getMessages(){
        return messages.Count;
    }
    public int getCurrentMessage(){
        return currentMessage;
    }
}
