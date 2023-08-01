using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class RoomControl : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject joinInputField, createInputField;
    // Start is called before the first frame update
    public void CreateRoom(){
        PhotonNetwork.CreateRoom(createInputField.GetComponent<TMP_InputField>().text);
    }
    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinInputField.GetComponent<TMP_InputField>().text);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("StartScreen");
    }
}
