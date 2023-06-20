using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

[System.Serializable]
public class JsonData
{
    public string direction { get; set; }
}

public delegate void CallbackDirection(JsonData message);


public class UdpManager : MonoBehaviourSingleton<UdpManager>
{
    public string _IpAddr = "127.0.0.1";
    public int _Port = 50002;
    CallbackDirection _CallbackDirection;

    #region private members
    private Thread ListenerThread;
    #endregion

    public UdpManager(string IpAddr, int Port)
    {
        _IpAddr = IpAddr;
        _Port = Port;
    }

    public void Init()
    {
        // Start TcpServer background thread
        ListenerThread = new Thread(new ThreadStart(ListenForIncommingRequest));
        ListenerThread.IsBackground = true;
        ListenerThread.Start();
    }

    void ListenForIncommingRequest()
    {
        Debug.Log("Start Server : " + _IpAddr + ", " + _Port);
        UdpClient listener = new UdpClient(_Port);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, _Port);
        try
        {
            while (true)
            {
                byte[] bytes = listener.Receive(ref groupEP);
                // Debug.Log(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                var json_data = GetJsonFromByte(bytes);
                MessageHandler(json_data);
            }
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException " + e.ToString());
        }
        finally
        {
            listener.Close();
        }
    }

    public JsonData GetJsonFromByte(byte[] data)
    {
        string JsonString = Encoding.UTF8.GetString(data);
        Debug.Log(JsonString);
        var JsonData = JsonConvert.DeserializeObject<JsonData>(JsonString);
        return JsonData;
    }

    public void SetMessageCallback(CallbackDirection callback)
    {
        if (_CallbackDirection == null)
        {
            _CallbackDirection = callback;
        }
        else
        {
            _CallbackDirection += callback;
        }
    }

    private void MessageHandler(JsonData json_data)
    {
        if (_CallbackDirection != null)
        {
            _CallbackDirection(json_data);
        }
    }
}