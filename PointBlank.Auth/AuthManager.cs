// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.AuthManager
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Data.Configs;
using PointBlank.Core;
using PointBlank.Core.Managers.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PointBlank.Auth
{
  public class AuthManager
  {
    public static ServerConfig Config;
    public static Socket mainSocket;
    public static List<AuthClient> _loginQueue = new List<AuthClient>();

    public static bool Start()
    {
      try
      {
        AuthManager.Config = ServerConfigSyncer.GenerateConfig(AuthConfig.configId);
        AuthManager.mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(AuthConfig.authIp), AuthConfig.authPort);
        AuthManager.mainSocket.Bind((EndPoint) ipEndPoint);
        AuthManager.mainSocket.Listen(int.MaxValue);
        AuthManager.mainSocket.BeginAccept(new AsyncCallback(AuthManager.AcceptCallback), (object) AuthManager.mainSocket);
        Logger.debug("Auth Iniciado. " + AuthConfig.authIp +"");
        return true;
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
        return false;
      }
    }

    private static void AcceptCallback(IAsyncResult result)
    {
      Socket asyncState = (Socket) result.AsyncState;
      try
      {
        Socket client = asyncState.EndAccept(result);
        if (client != null)
        {
          new AuthClient(client)?.Start();
          Thread.Sleep(5);
        }
      }
      catch
      {
        Logger.warning("Failed a Client Connection");
      }
      AuthManager.mainSocket.BeginAccept(new AsyncCallback(AuthManager.AcceptCallback), (object) AuthManager.mainSocket);
    }

    public static int EnterQueue(AuthClient sck)
    {
      if (sck == null)
        return -1;
      lock (AuthManager._loginQueue)
      {
        if (AuthManager._loginQueue.Contains(sck))
          return -1;
        AuthManager._loginQueue.Add(sck);
        return AuthManager._loginQueue.IndexOf(sck);
      }
    }
  }
}
