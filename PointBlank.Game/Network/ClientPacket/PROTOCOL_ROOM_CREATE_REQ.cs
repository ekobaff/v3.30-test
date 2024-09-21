// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_CREATE_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_ROOM_CREATE_REQ : ReceivePacket
  {
    private uint erro;
    private Room room;
    private Account p;

    public PROTOCOL_ROOM_CREATE_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.p = this._client._player;
      Channel ch = this.p == null ? (Channel) null : this.p.getChannel();
      try
      {
        if (this.p == null || ch == null || this.p.player_name.Length == 0 || this.p._room != null || this.p._match != null)
        {
          this.erro = 2147483648U;
        }
        else
        {
          lock (ch._rooms)
          {
            for (int index = 0; index < 300; ++index)
            {
              if (ch.getRoom(index) == null)
              {
                this.room = new Room(index, ch);
                this.readD();
                this.room.name = this.readUnicode(46);
                this.room.mapId = (MapIdEnum) this.readC();
                this.room.rule = (int) this.readC();
                this.room.stage = this.readC();
                this.room.room_type = (RoomType) this.readC();
                if (this.room.room_type != RoomType.None)
                {
                  int num1 = (int) this.readC();
                  int num2 = (int) this.readC();
                  this.room.initSlotCount((int) this.readC());
                  int num3 = (int) this.readC();
                  this.room.weaponsFlag = (RoomWeaponsFlag) this.readH();
                  if ((byte) this.room.weaponsFlag <= (byte) 47 && (byte) this.room.weaponsFlag > (byte) 15)
                    this.room.SniperMode = true;
                  else if ((byte) this.room.weaponsFlag <= (byte) 79 && (byte) this.room.weaponsFlag > (byte) 47)
                    this.room.ShotgunMode = true;
                  this.room.Flag = (RoomStageFlag) this.readD();
                  int num4 = (int) this.readC();
                  int num5 = (int) this.readC();
                  int num6 = (int) this.readC();
                  if (this.room.isBotMode() && this.room._channelType == 4)
                  {
                    this.erro = 2147487869U;
                    return;
                  }
                  this.readUnicode(66);
                  this.room.killtime = (int) this.readC();
                  int num7 = (int) this.readC();
                  int num8 = (int) this.readC();
                  int num9 = (int) this.readC();
                  this.room.Limit = this.readC();
                  this.room.WatchRuleFlag = this.readC();
                  this.room.BalanceType = this.readH();
                  if (ch._type == 4)
                  {
                    this.room.Limit = (byte) 1;
                    this.room.BalanceType = (short) 0;
                  }
                  this.readB(16);
                  this.readB(4);
                  this.room.password = this.readS(4);
                  this.room.aiCount = this.readC();
                  this.room.aiLevel = this.readC();
                  this.room.aiType = this.readC();
                  this.room.addPlayer(this.p);
                  this.p.ResetPages();
                  this.room.SetSeed();
                  ch.AddRoom(this.room);
                  return;
                }
                break;
              }
            }
            this.erro = 2147483648U;
          }
        }
      }
      catch (Exception ex)
      {
        Logger.error("PROTOCOL_LOBBY_CREATE_ROOM_REQ: " + ex.ToString());
        this.erro = 2147483648U;
      }
    }

    public override void run() => this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_CREATE_ACK(this.erro, this.room, this.p));
  }
}
