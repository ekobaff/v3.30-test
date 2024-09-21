// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Data.Sync.AuthSync
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Data.Configs;
using PointBlank.Auth.Data.Managers;
using PointBlank.Auth.Data.Model;
using PointBlank.Auth.Data.Sync.Client;
using PointBlank.Auth.Data.Xml;
using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Managers.Server;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PointBlank.Auth.Data.Sync
{
  public class AuthSync
  {
    private static DateTime LastSyncCount;
    public static UdpClient udp;

    public static void Start()
    {
            try
            {

                AuthSync.udp = new UdpClient(AuthConfig.syncPort);
                uint num = 2147483648U;
                uint num2 = 402653184U;
                uint ioControlCode = num | num2 | 12U;
                AuthSync.udp.Client.IOControl((int)ioControlCode, new byte[]
                {
                    Convert.ToByte(false)
                }, null);
                new Thread(new ThreadStart(AuthSync.read)).Start();
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
    }

    public static void read()
    {
      try
      {
        AuthSync.udp.BeginReceive(new AsyncCallback(AuthSync.recv), (object) null);
      }
      catch
      {
      }
    }

    private static void recv(IAsyncResult res)
    {
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 8000);
      byte[] buffer = AuthSync.udp.EndReceive(res, ref remoteEP);
      Thread.Sleep(5);
      new Thread(new ThreadStart(AuthSync.read)).Start();
      if (buffer.Length < 2)
        return;
      AuthSync.LoadPacket(buffer);
    }

    private static void LoadPacket(byte[] buffer)
    {
      ReceiveGPacket p = new ReceiveGPacket(buffer);
      short num1 = p.readH();
      switch (num1)
      {
        case 11:
          int num2 = (int) p.readC();
          int num3 = (int) p.readC();
          PointBlank.Auth.Data.Model.Account account1 = AccountManager.getInstance().getAccount(p.readQ(), true);
          if (account1 == null)
            break;
          PointBlank.Auth.Data.Model.Account account2 = AccountManager.getInstance().getAccount(p.readQ(), true);
          if (account2 == null)
            break;
          FriendState friendState = num3 == 1 ? FriendState.Online : FriendState.Offline;
          if (num2 == 0)
          {
            int index = -1;
            Friend friend = account2.FriendSystem.GetFriend(account1.player_id, out index);
            if (index == -1 || friend == null)
              break;
            account2.SendPacket((PointBlank.Core.Network.SendPacket) new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState.Update, friend, friendState, index));
            break;
          }
          account2.SendPacket((PointBlank.Core.Network.SendPacket) new PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(account1, friendState));
          break;
        case 13:
          long id1 = p.readQ();
          byte num4 = p.readC();
          byte[] data = p.readB((int) p.readUH());
          PointBlank.Auth.Data.Model.Account account3 = AccountManager.getInstance().getAccount(id1, true);
          if (account3 == null)
            break;
          if (num4 == (byte) 0)
          {
            account3.SendPacket(data);
            break;
          }
          account3.SendCompletePacket(data);
          break;
        case 16:
          ClanSync.Load(p);
          break;
        case 17:
          FriendSync.Load(p);
          break;
        case 19:
          PlayerSync.Load(p);
          break;
        case 20:
          ServerWarning.LoadGMWarning(p);
          break;
        case 22:
          ServerWarning.LoadShopRestart(p);
          break;
        case 23:
          ServerWarning.LoadServerUpdate(p);
          break;
        case 24:
          ServerWarning.LoadShutdown(p);
          break;
        case 31:
          int index1 = (int) p.readC();
          EventLoader.ReloadEvent(index1);
          Logger.warning("AuthSync Refresh event.");
          Logger.LogCMD("Refresh event; Type: " + index1.ToString() + "; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
          Logger.LogCMD("Refresh event; Type: " + index1.ToString() + "; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
          break;
        case 32:
          ServerConfigSyncer.GenerateConfig((int) p.readC());
          Logger.warning("AuthSync Configuration (Database) Refills.; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
          Logger.LogCMD("Configuration (Database) Refills.; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
          break;
        case 33:
          int channelId = p.readD();
          int num5 = p.readD();
          Channel channel = ChannelsXml._channels.Where<Channel>((Func<Channel, bool>) (x => x._id == channelId)).FirstOrDefault<Channel>();
          if (channel != null)
          {
            channel._players = num5;
            break;
          }
          Logger.warning("Channel not founded");
          break;
        case 34:
          int num6 = (int) p.readC();
          switch (num6)
          {
            case 1:
              EventLoader.ReloadAll();
              Logger.warning("Events reloaded: " + DateTime.Now.ToString("dd/MM/yy HH:mm"));
              return;
            default:
              Logger.warning("Updating null part: " + num6.ToString());
              return;
          }
        case 505:
          int id2 = p.readD();
          int num7 = p.readD();
          GameServerModel server = ServersXml.getServer(id2);
          if (server == null)
            break;
          server._LastCount = num7;
          break;
        default:
          Logger.warning("AuthSync Connection opcode not found: " + num1.ToString());
          break;
      }
    }

    public static void UpdateGSCount(int serverId)
    {
    }

    public static void SendLoginKickInfo(PointBlank.Auth.Data.Model.Account player)
    {
      int serverId = (int) player._status.serverId;
      switch (serverId)
      {
        case 0:
        case (int) byte.MaxValue:
          player.setOnlineStatus(false);
          break;
        default:
          GameServerModel server = ServersXml.getServer(serverId);
          if (server == null)
            break;
          using (SendGPacket sendGpacket = new SendGPacket())
          {
            sendGpacket.writeH((short) 10);
            sendGpacket.writeQ(player.player_id);
            AuthSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
            break;
          }
      }
    }

    public static void SendPacket(byte[] data, IPEndPoint ip) => AuthSync.udp.Send(data, data.Length, ip);
  }
}
