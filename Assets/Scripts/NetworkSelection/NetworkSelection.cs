using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkSelectionState {

}

public class NetworkSelection : MonoBehaviour
{
    public Text txtIp;
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
        InvokeRepeating("ProcessNetworkSelection", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ProcessNetworkSelection()
    {
        Debug.Log("ProcessNetworkSelection: 500ms interval");
        txtIp.text = networkManager.GetIp();
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
