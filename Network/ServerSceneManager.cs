using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ServerSceneManager : MonoBehaviour
{
    Server server;
    UdpClient udp;

    private const int gameID = 420;

   public void startServer()
    {
        server = new Server();
        server.StartServer();
        udp = new UdpClient(3001);
    }

}
