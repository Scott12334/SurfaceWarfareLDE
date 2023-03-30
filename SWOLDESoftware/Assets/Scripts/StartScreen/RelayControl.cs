using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class RelayControl : MonoBehaviour
{
    [SerializeField]
    private GameObject codeDisplay, inputField, relayStuff;
    private bool launched = false;
    private async void Start(){
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>{
            Debug.Log("Signed in "+ AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    private void Update() {
        if(GameObject.FindGameObjectWithTag("Player") != null && launched == false){
            relayStuff.SetActive(false);
            launched = true;
        }
    }
    public async void CreateRelay(){
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(50);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            codeDisplay.GetComponent<TextMeshProUGUI>().text = joinCode;

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartServer();
        } catch(RelayServiceException e){
            Debug.Log(e);
        }
    }
    public void joinSession(){
        string newJoinCode = inputField.GetComponent<TMP_InputField>().text;
        JoinRelay(newJoinCode.ToUpper());
    }
    private async void JoinRelay(string joinCode){
        try{
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        } catch(RelayServiceException e){
            Debug.Log(e);
        }
    }
}
