using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public string message;
    public string getMessage(){return message;}
    public void setMessage(string newMessage){this.message=newMessage;}
}
