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

        startClient.interactable = networkManager != null && networkManager.GetStatus() == "";
        startServer.interactable = networkManager != null && networkManager.GetStatus() == "";

        txtIp.text = networkManager.GetIp().ToString();
        txtStatus.text = networkManager.GetStatus();
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
