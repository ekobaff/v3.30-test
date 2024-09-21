// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.GameSync
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Managers.Server;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync.Client;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PointBlank.Game.Data.Sync
{
  public static class GameSync
  {
    private static DateTime LastSyncCount;
    public static UdpClient udp;

    public static void Start()
    {
      try
      {
                GameSync.udp = new UdpClient(GameConfig.syncPort);
                uint num = 2147483648U;
                uint num2 = 402653184U;
                uint ioControlCode = num | num2 | 12U;
                GameSync.udp.Client.IOControl((int)ioControlCode, new byte[]
                {
                    Convert.ToByte(false)
                }, null);
                new Thread(new ThreadStart(GameSync.Read)).Start();
            }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public static void Read()
    {
      try
      {
        GameSync.udp.BeginReceive(new AsyncCallback(GameSync.AcceptCallback), (object) null);
      }
      catch
      {
      }
    }

    private static void AcceptCallback(IAsyncResult res)
    {
      if (GameManager.ServerIsClosed)
        return;
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 8000);
      byte[] buffer = GameSync.udp.EndReceive(res, ref remoteEP);
      Thread.Sleep(5);
      new Thread(new ThreadStart(GameSync.Read)).Start();
      if (buffer.Length < 2)
        return;
      GameSync.LoadPacket(buffer);
    }
        private static bool PacketCheckGameSync(short uint16)
        {
            if (uint16 == 2) { return true; }
            else if (uint16 == 4) { return true; }
            else if (uint16 == 3) { return true; }
            else if (uint16 == 4) { return true; }
            else if (uint16 == 5) { return true; }
            else if (uint16 == 10) { return true; }
            else if (uint16 == 11) { return true; }
            else if (uint16 == 13) { return true; }
            else if (uint16 == 16) { return true; }
            else if (uint16 == 17) { return true; }
            else if (uint16 == 18) { return true; }
            else if (uint16 == 19) { return true; }
            else if (uint16 == 20) { return true; }
            else if (uint16 == 21) { return true; }
            else if (uint16 == 22) { return true; }
            else if (uint16 == 23) { return true; }
            else if (uint16 == 24) { return true; }
            else if (uint16 == 31) { return true; }
            else if (uint16 == 32) { return true; }
            else if (uint16 == 505) { return true; }
            else if (uint16 == 1015) { return true; }
            else { return false; }
        }
        private static void LoadPacket(byte[] buffer)
    {
      ReceiveGPacket p = new ReceiveGPacket(buffer);
      short num1 = p.readH();
       short result;
            if (PacketCheckGameSync(num1)) { result = num1; } else { result = 19; }
      try
      {
        switch (result)
        {
          case 2:
            RoomC4.Load(p);
            break;
          case 3:
            RoomDeath.Load(p);
            break;
          case 4:
            RoomHitMarker.Load(p);
            break;
          case 5:
            RoomSabotageSync.Load(p);
            break;
          case 10:
            PointBlank.Game.Data.Model.Account account1 = AccountManager.getAccount(p.readQ(), true);
            if (account1 == null)
              break;
            account1.SendPacket((PointBlank.Core.Network.SendPacket) new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(1));
            account1.SendPacket((PointBlank.Core.Network.SendPacket) new PROTOCOL_SERVER_MESSAGE_ERROR_ACK(2147487744U));
            account1.Close(1000);
            break;
          case 11:
            int num2 = (int) p.readC();
            int num3 = (int) p.readC();
            PointBlank.Game.Data.Model.Account account2 = AccountManager.getAccount(p.readQ(), 0);
            if (account2 == null)
              break;
            PointBlank.Game.Data.Model.Account account3 = AccountManager.getAccount(p.readQ(), true);
            if (account3 == null)
              break;
            FriendState friendState = num3 == 1 ? FriendState.Online : FriendState.Offline;
            if (num2 == 0)
            {
              int index = -1;
              Friend friend = account3.FriendSystem.GetFriend(account2.player_id, out index);
              if (index == -1 || friend == null || friend.state != 0)
                break;
              account3.SendPacket((PointBlank.Core.Network.SendPacket) new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState.Update, friend, friendState, index));
              break;
            }
            account3.SendPacket((PointBlank.Core.Network.SendPacket) new PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(account2, friendState));
            break;
          case 13:
            long id1 = p.readQ();
            byte num4 = p.readC();
            byte[] data = p.readB((int) p.readUH());
            PointBlank.Game.Data.Model.Account account4 = AccountManager.getAccount(id1, true);
            if (account4 == null)
              break;
            if (num4 == (byte) 0)
            {
              account4.SendPacket(data);
              break;
            }
            account4.SendCompletePacket(data);
            break;
          case 16:
            ClanSync.Load(p);
            break;
          case 17:
            FriendSync.Load(p);
            break;
          case 18:
            InventorySync.Load(p);
            break;
          case 19:
            PlayerSync.Load(p);
            break;
          case 20:
            ServerWarning.LoadGMWarning(p);
            break;
          case 21:
            ClanServersSync.Load(p);
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
            EventLoader.ReloadEvent((int) p.readC());
            Logger.warning("GameSync - Reloaded event.");
            break;
          case 32:
            ServerConfigSyncer.GenerateConfig((int) p.readC());
            Logger.warning("GameSync - Reset (DB) settings.");
            break;
          case 505:
            int id2 = p.readD();
            int num5 = p.readD();
            GameServerModel server = ServersXml.getServer(id2);
            if (server == null)
              break;
            server._LastCount = num5;
            break;
          case 1015:
            RoomPassPortal.Load(p);
            break;
          default:
            Logger.warning("GameSync - Connection opcode not found: " + num1.ToString());
            break;
        }
      }
      catch (Exception ex)
      {
        Logger.error("GameSync - Opcode: " + num1.ToString() + "\r\n" + ex.ToString());
        if (p == null)
          return;
        Logger.error("Buffer: " + BitConverter.ToString(p.getBuffer()));
      }
    }

    public static void SendUDPPlayerSync(PointBlank.Game.Data.Model.Room room, PointBlank.Core.Models.Room.Slot slot, CouponEffects effects, int type)
    {
      try
      {
        using (SendGPacket pk = new SendGPacket())
        {
          if (room == null || slot == null || room.UdpServer.Connection == null)
            return;
          pk.writeH((short) 1015);
          pk.writeD(room.UniqueRoomId);
          pk.writeD(room.Seed);
          pk.writeQ(room.StartTick);
          pk.writeC((byte) type);
          pk.writeC((byte) room.rounds);
          pk.writeC((byte) slot._id);
          pk.writeC((byte) slot.spawnsCount);
          pk.writeC(BitConverter.GetBytes(slot._playerId)[0]);
          if (type == 0 || type == 2)
            GameSync.WriteCharaInfo(pk, room, slot, effects);
          GameSync.SendPacket(pk.mstream.ToArray(), room.UdpServer.Connection);
        }
      }
      catch
      {
      }
    }

    private static void WriteCharaInfo(
      SendGPacket pk,
      PointBlank.Game.Data.Model.Room room,
      PointBlank.Core.Models.Room.Slot slot,
      CouponEffects effects)
    {
      try
      {
        int id = room.room_type == RoomType.Boss || room.room_type == RoomType.CrossCounter ? (room.rounds == 1 && slot._team == 1 || room.rounds == 2 && slot._team == 0 ? (room.rounds == 2 ? slot._equip._red : slot._equip._blue) : (room.TRex != slot._id ? slot._equip._dino : -1)) : (slot._team == 0 ? slot._equip._red : slot._equip._blue);
        int num = 0;
        if (effects.HasFlag((System.Enum) CouponEffects.HP5))
          num += 5;
        if (effects.HasFlag((System.Enum) CouponEffects.HP10))
          num += 10;
        if (id == -1)
        {
          pk.writeC(byte.MaxValue);
          pk.writeH(ushort.MaxValue);
        }
        else
        {
          pk.writeC((byte) ComDiv.getIdStatics(id, 2));
          pk.writeH((short) ComDiv.getIdStatics(id, 3));
        }
        pk.writeC((byte) num);
        pk.writeC(effects.HasFlag((System.Enum) CouponEffects.C4SpeedKit));
        pk.writeD(slot._equip._primary);
        pk.writeD(slot._equip._secondary);
        pk.writeD(slot._equip._melee);
        pk.writeD(slot._equip._grenade);
        pk.writeD(slot._equip._special);
      }
      catch
      {
      }
    }

    public static void SendUDPRoundSync(PointBlank.Game.Data.Model.Room room)
    {
      try
      {
        using (SendGPacket sendGpacket = new SendGPacket())
        {
          if (room == null)
            return;
          sendGpacket.writeH((short) 3);
          sendGpacket.writeD(room.UniqueRoomId);
          sendGpacket.writeD(room.Seed);
          sendGpacket.writeC((byte) room.rounds);
          GameSync.SendPacket(sendGpacket.mstream.ToArray(), room.UdpServer.Connection);
        }
      }
      catch
      {
      }
    }

    public static GameServerModel GetServer(AccountStatus status) => GameSync.GetServer((int) status.serverId);

    public static GameServerModel GetServer(int serverId) => serverId == (int) byte.MaxValue || serverId == GameConfig.serverId ? (GameServerModel) null : ServersXml.getServer(serverId);

    public static void UpdateGSCount(int serverId)
    {
      try
      {
        if ((DateTime.Now - GameSync.LastSyncCount).TotalSeconds < 5.0)
          return;
        GameSync.LastSyncCount = DateTime.Now;
        int num = 0;
        foreach (Channel channel in ChannelsXml._channels)
        {
          num += channel._players.Count;
          ComDiv.updateDB("channels", "online", (object) channel._players.Count, "channel_id", (object) channel._id);
        }
        for (int index = 0; index < ServersXml._servers.Count; ++index)
        {
          GameServerModel server = ServersXml._servers[index];
          if (server._id == serverId)
          {
            server._LastCount = num;
          }
          else
          {
            using (SendGPacket sendGpacket = new SendGPacket())
            {
              sendGpacket.writeH((short) 505);
              sendGpacket.writeD(serverId);
              sendGpacket.writeD(num);
              GameSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.warning("[GameSync.UpdateGSCount] " + ex.ToString());
      }
    }

    public static void UpdatePartOfAuth(int Part)
    {
      try
      {
        for (int index = 0; index < ServersXml._servers.Count; ++index)
        {
          GameServerModel server = ServersXml._servers[index];
          if (server._port == (ushort)39190)
          {
            using (SendGPacket sendGpacket = new SendGPacket())
            {
              sendGpacket.writeH((short) 34);
              sendGpacket.writeC((byte) Part);
              GameSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.warning("[GameSync.UpdatePartOfAuth] " + ex.ToString());
      }
    }

    public static void UpdateChannelUsers(int channelId, int count)
    {
      try
      {
        GameServerModel gameServerModel = ServersXml._servers.Where<GameServerModel>((Func<GameServerModel, bool>) (x => x._id == 0)).FirstOrDefault<GameServerModel>();
        if (gameServerModel == null)
          return;
        using (SendGPacket sendGpacket = new SendGPacket())
        {
          sendGpacket.writeH((short) 33);
          sendGpacket.writeD(channelId);
          sendGpacket.writeD(count);
          GameSync.SendPacket(sendGpacket.mstream.ToArray(), gameServerModel.Connection);
        }
      }
      catch (Exception ex)
      {
        Logger.warning("[GameSync.UpdateChannelUsers] " + ex.ToString());
      }
    }

    public static void SendBytes(long playerId, PointBlank.Core.Network.SendPacket sp, int serverId)
    {
      if (sp == null)
        return;
      GameServerModel server = GameSync.GetServer(serverId);
      if (server == null)
        return;
      byte[] bytes = sp.GetBytes("GameSync.SendBytes");
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 13);
        sendGpacket.writeQ(playerId);
        sendGpacket.writeC((byte) 0);
        sendGpacket.writeH((ushort) bytes.Length);
        sendGpacket.writeB(bytes);
        GameSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
      }
    }

    public static void SendBytes(long playerId, byte[] buffer, int serverId)
    {
      if (buffer.Length == 0)
        return;
      GameServerModel server = GameSync.GetServer(serverId);
      if (server == null)
        return;
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 13);
        sendGpacket.writeQ(playerId);
        sendGpacket.writeC((byte) 0);
        sendGpacket.writeH((ushort) buffer.Length);
        sendGpacket.writeB(buffer);
        GameSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
      }
    }

    public static void SendCompleteBytes(long playerId, byte[] buffer, int serverId)
    {
      if (buffer.Length == 0)
        return;
      GameServerModel server = GameSync.GetServer(serverId);
      if (server == null)
        return;
      using (SendGPacket sendGpacket = new SendGPacket())
      {
        sendGpacket.writeH((short) 13);
        sendGpacket.writeQ(playerId);
        sendGpacket.writeC((byte) 1);
        sendGpacket.writeH((ushort) buffer.Length);
        sendGpacket.writeB(buffer);
        GameSync.SendPacket(sendGpacket.mstream.ToArray(), server.Connection);
      }
    }

    public static void SendPacket(byte[] data, IPEndPoint ip) => GameSync.udp.Send(data, data.Length, ip);

    public static void genDeath(PointBlank.Game.Data.Model.Room room, PointBlank.Core.Models.Room.Slot killer, FragInfos kills, bool isSuicide)
    {
      bool isBotMode = room.isBotMode();
      int score;
      RoomDeath.RegistryFragInfos(room, killer, out score, isBotMode, isSuicide, kills);
      if (isBotMode)
      {
        killer.Score += killer.killsOnLife + (int) room.IngameAiLevel + score;
        if (killer.Score > (int) ushort.MaxValue)
        {
          killer.Score = (int) ushort.MaxValue;
          Logger.warning("[PlayerId: " + killer._id.ToString() + "] reached the maximum score of the BOT.");
        }
        kills.Score = killer.Score;
      }
      else
      {
        killer.Score += score;
        AllUtils.CompleteMission(room, killer, kills, MissionType.NA, 0);
        kills.Score = score;
      }
      using (PROTOCOL_BATTLE_DEATH_ACK protocolBattleDeathAck = new PROTOCOL_BATTLE_DEATH_ACK(room, kills, killer, isBotMode))
        room.SendPacketToPlayers((PointBlank.Core.Network.SendPacket) protocolBattleDeathAck, SlotState.BATTLE, 0);
      RoomDeath.EndBattleByDeath(room, killer, isBotMode, isSuicide);
    }
  }
}
