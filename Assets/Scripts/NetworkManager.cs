using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public enum NetworkType
{
    CLIENT,
    SERVER,
    NONE
}
public class NetworkStatus
{
    public NetworkType networkType = NetworkType.NONE;
    public bool connected;
}

public class NetworkManager : MonoBehaviour
{
    private TcpClient tcpClient;
    private TcpListener tcpListener;
    private TcpClient connectedTcpClient;
    private Thread tcpListenerThread;
    private Thread tcpClientThread;
    private NetworkMessage incomingMessage;
    public int _port = 7531;

    public NetworkType GetNetworkType()
    {
        NetworkType networkType = NetworkType.NONE;
        if (tcpClient != null && tcpClient.Client != null) networkType = NetworkType.CLIENT;
        if (tcpListener != null && tcpListener.Server != null) networkType = NetworkType.SERVER;
        return networkType;
    }

    public NetworkStatus GetStatus()
    {
        var networkType = GetNetworkType();
        return new NetworkStatus()
        {
            networkType = networkType,
            connected = networkType == NetworkType.SERVER ? connectedTcpClient != null : networkType == NetworkType.CLIENT ? tcpClient.Client.Connected : false
        };
    }

    public IPAddress GetIp()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip;
            }
        }
        return IPAddress.None;
    }

    #region server
    public void StartServer()
    {
        Debug.Log("Starting server");

        tcpListener = new TcpListener(GetIp(), _port);
        tcpListener.Start();
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    private void ListenForIncommingRequests()
    {
        try
        {
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                if (connectedTcpClient == null)
                    connectedTcpClient = tcpListener.AcceptTcpClient();
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }

    public void SendMessage(NetworkMessage message)
    {
        if (connectedTcpClient == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = JsonUtility.ToJson(message);
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent his message - should be received by client");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    #endregion

    #region client
    public void StartClient(string ip)
    {
        Debug.Log($"Starting client");
        try
        {
            tcpClient = new TcpClient(ip, _port);
            tcpClientThread = new Thread(new ThreadStart(ReceiveMessage));
            tcpClientThread.IsBackground = true;
            tcpClientThread.Start();
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void ReceiveMessage()
    {
        Byte[] bytes = new Byte[1024];
        while (true)
        {
            if (tcpClient.Connected)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        incomingMessage = JsonUtility.FromJson<NetworkMessage>(Encoding.ASCII.GetString(incommingData));
                        Debug.Log("server message received as: " + incomingMessage);
                    }
                }
            }
        }
    }

    public NetworkMessage GetMessage()
    {
        return incomingMessage;
    }
    #endregion
}
