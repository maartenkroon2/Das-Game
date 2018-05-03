using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField]
    InputField networkAddressField;

    public void HostGame()
    {
        StartHost();
    }

    public void JoinGame()
    {  
        StartClient();
    }
}
