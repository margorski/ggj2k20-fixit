using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkSelection : MonoBehaviour
{
    public Text txtIp;
    public Text txtStatus;
    public Text txtMessage;

    public Button startClient;
    public Button startServer;
    public InputField ipInput;
    
    public NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        startClient.onClick.AddListener(OnStartClientClick);
        startServer.onClick.AddListener(OnStartServerClick);
        if (networkManager != null) ipInput.text = networkManager.GetIp().ToString();
        InvokeRepeating("OnPeriodicalUpdate", 0, 0.5f);
    }

    void OnPeriodicalUpdate()
    {
        if (networkManager == null) return;


        txtIp.text = networkManager.GetIp().ToString();
        NetworkStatus networkStatus = networkManager.GetStatus();

        startClient.interactable = networkManager != null && networkStatus.networkType == NetworkType.NONE;
        startServer.interactable = networkManager != null && networkStatus.networkType == NetworkType.NONE;

        txtStatus.text = $"{networkStatus.networkType.ToString()}: \tCONNECTED: {networkStatus.connected.ToString()}";

        txtMessage.text = JsonUtility.ToJson(networkManager.GetMessage());

        networkManager.SendMessage(new NetworkMessage());
    }

    void OnStartClientClick()
    {
        if (networkManager == null) return;
        networkManager.StartClient(ipInput.text);   
    }

    void OnStartServerClick()
    {
        if (networkManager == null) return;

        networkManager.StartServer();
    }
}
