using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkSelectionState {

}

public class NetworkSelection : MonoBehaviour
{
    public Text txtIp;
    public Text txtStatus;
    public Text txtMessage;

    public Button startClient;
    public Button startServer;
    public InputField ipInput;
    
    private NetworkManager networkManager;
    private NetworkSelectionState networkSelectionState = new NetworkSelectionState();

    // Start is called before the first frame update
    void Start()
    {
        networkManager = gameObject.GetComponent<NetworkManager>();
        startClient.onClick.AddListener(OnStartClientClick);
        startServer.onClick.AddListener(OnStartServerClick);
        InvokeRepeating("OnPeriodicalUpdate", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPeriodicalUpdate()
    {
        txtIp.text = networkManager.GetIp().ToString();
        txtStatus.text = networkManager.GetStatus();
        txtMessage.text = JsonUtility.ToJson(networkManager.GetMessage());

        networkManager.SendMessage(new NetworkMessage());
    }

    void OnStartClientClick()
    {
        networkManager.StartClient(ipInput.text);   
    }

    void OnStartServerClick()
    {
        networkManager.StartServer();
    }
}
