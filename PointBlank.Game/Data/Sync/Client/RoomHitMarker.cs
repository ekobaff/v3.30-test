// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Data.Sync.Client.RoomHitMarker
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Data.Sync.Client
{
  public static class RoomHitMarker
  {
        public static void Load(ReceiveGPacket p)
        {
            int roomID = (int)p.readH();
            int channelID = (int)p.readH();
            byte killerIdx = p.readC();
            byte deathtype = p.readC();
            byte hitEnum = p.readC();
            int damage = (int)p.readH();
            if (p.getBuffer().Length > 11)
                Logger.warning("Invalid Hit: " + BitConverter.ToString(p.getBuffer()));
            Channel channel = ChannelsXml.getChannel(channelID);
            if (channel == null)
                return;
            Room room = channel.getRoom(roomID);
            if (room == null || room._state != RoomState.Battle)
                return;
            Account playerBySlot = room.getPlayerBySlot((int)killerIdx);
            if (playerBySlot != null)
            {
              if (!playerBySlot._damage)
                    return;
                string message = "";
                if (deathtype == (byte)10)
                {
                    message = "Vida Restaurada " + damage;
                }
                else 
                {
                    switch (hitEnum)
                    {
                        case 0:
                            message = "Dano Aplicado [Nomal] - " + damage;
                            break;
                        case 1:
                            message = "Dano Aplicado [Crítico] - " + damage;
                            break;
                        case 2:
                            message = "O Jogador Foi Protegido Pelo Helmet";
                            break;
                        case 3:
                            message = "O Jogador Foi Protegido Pelo Helmet";
                            break;
                    }
                }
                playerBySlot.SendPacket((SendPacket)new PROTOCOL_LOBBY_CHATTING_ACK("Server", playerBySlot.getSessionId(), 0, true, message));
            }
        }
  }
}
