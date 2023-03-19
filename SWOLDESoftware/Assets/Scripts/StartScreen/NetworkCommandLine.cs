using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager;
    private void Start() {
        netManager = GetComponentInParent<NetworkManager>();

        if(Application.isEditor) return;

        var args = GetCommandLineArgs();

        if(args.TryGetValue("-mode", out string mode)){
            if(mode == "server"){
                netManager.StartServer();
            }
            else if (mode == "client"){
                netManager.StartClient();
            }
            else if (mode == "host"){
                netManager.StartHost();
            }
        }
    }
    private Dictionary<string, string> GetCommandLineArgs(){
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for(int i=0; i < args.Length; i++){
            var arg = args[i].ToLower();
            if(arg.StartsWith("-")){
                var value = i < args.Length-1 ? args[i+1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg,value);
            }
        }
        return argDictionary;
    }
    public void startPressed(){
        NetworkManager.Singleton.StartClient();
    }
}
