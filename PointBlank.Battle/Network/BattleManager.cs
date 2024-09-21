// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.BattleManager
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Configs;
using System;
using System.Net;
using System.Net.Sockets;

namespace PointBlank.Battle.Network
{
  public class BattleManager
  {
    private static UdpClient UdpClient;

    public static void Connect()
    {
      try
      {
                BattleManager.UdpClient = new UdpClient();
                uint num = 2147483648U;
                uint num2 = 402653184U;
                uint ioControlCode = num | num2 | 12U;
                BattleManager.UdpClient.Client.IOControl((int)ioControlCode, new byte[]
                {
                    Convert.ToByte(false)
                }, null);
                IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Parse(BattleConfig.hosIp), (int)BattleConfig.hosPort);
                BattleManager.UdpState state = new BattleManager.UdpState(ipendPoint, BattleManager.UdpClient);
                BattleManager.UdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                BattleManager.UdpClient.Client.Bind(ipendPoint);
                BattleManager.UdpClient.BeginReceive(new AsyncCallback(BattleManager.AcceptCallback), state);
                Logger.debug("Active Server. (" + DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + ")");
            }
      catch (Exception ex)
      {
        Logger.error(ex.ToString() + "\r\nAn error occurred while listing the Udp connections!!");
      }
    }

    private static void Read(BattleManager.UdpState state)
    {
      try
      {
        BattleManager.UdpClient.BeginReceive(new AsyncCallback(BattleManager.AcceptCallback), (object) state);
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    private static void AcceptCallback(IAsyncResult ar)
    {
      if (!ar.IsCompleted)
        Logger.warning("Result is not completed.");
      ar.AsyncWaitHandle.WaitOne(5000);
      DateTime now = DateTime.Now;
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
      UdpClient udpClient = ((BattleManager.UdpState) ar.AsyncState).UdpClient;
      IPEndPoint endPoint = ((BattleManager.UdpState) ar.AsyncState).EndPoint;
      try
      {
        byte[] Buff = udpClient.EndReceive(ar, ref remoteEP);
        if (Buff.Length >= 22)
        {
          BattleHandler battleHandler = new BattleHandler(BattleManager.UdpClient, Buff, remoteEP, now);
        }
        else
          Logger.warning("No Length (22) Buffer: " + BitConverter.ToString(Buff));
      }
      catch (Exception ex)
      {
        Logger.warning("Exception: " + remoteEP.Address?.ToString() + ":" + remoteEP.Port.ToString());
        Logger.warning(ex.ToString());
      }
      BattleManager.Read(new BattleManager.UdpState(endPoint, udpClient));
    }

    public static void Send(byte[] data, IPEndPoint ip) => BattleManager.UdpClient.Send(data, data.Length, ip);

    private class UdpState
    {
      public IPEndPoint EndPoint;
      public UdpClient UdpClient;

      public UdpState(IPEndPoint EndPoint, UdpClient UdpClient)
      {
        this.EndPoint = EndPoint;
        this.UdpClient = UdpClient;
      }
    }
  }
}
