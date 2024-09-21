﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BATTLE_DEATH_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync.Client;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BATTLE_DEATH_REQ : ReceivePacket
  {
    private FragInfos kills = new FragInfos();
    private bool isSuicide;

    public PROTOCOL_BATTLE_DEATH_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.kills.killingType = (CharaKillType) this.readC();
      this.kills.killsCount = this.readC();
      this.kills.killerIdx = this.readC();
      this.kills.weapon = this.readD();
      this.kills.x = this.readT();
      this.kills.y = this.readT();
      this.kills.z = this.readT();
      this.kills.flag = this.readC();
      for (int index = 0; index < (int) this.kills.killsCount; ++index)
      {
        Frag frag = new Frag();
        frag.victimWeaponClass = this.readC();
        frag.SetHitspotInfo(this.readC());
        int num = (int) this.readH();
        frag.flag = this.readC();
        frag.x = this.readT();
        frag.y = this.readT();
        frag.z = this.readT();
        frag.AssistSlot = (int) this.readC();
        this.kills.frags.Add(frag);
        if (frag.VictimSlot == (int) this.kills.killerIdx)
          this.isSuicide = true;
      }
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        PointBlank.Game.Data.Model.Room room = player._room;
        if (room == null || room.round.Timer != null || room._state < RoomState.Battle)
          return;
        bool isBotMode = room.isBotMode();
        Slot slot = room.getSlot((int) this.kills.killerIdx);
        if (slot == null || !isBotMode && (slot.state < SlotState.BATTLE || slot._id != player._slotId))
          return;
        int score;
        RoomDeath.RegistryFragInfos(room, slot, out score, isBotMode, this.isSuicide, this.kills);
        if (isBotMode)
        {
          slot.Score += slot.killsOnLife + (int) room.IngameAiLevel + score;
          if (slot.Score > (int) ushort.MaxValue)
          {
            slot.Score = (int) ushort.MaxValue;
            Logger.LogHack("[Nick: " + player.player_name + " PlayerId: " + player.player_id.ToString() + " Login: " + player.login + "] reached the maximum score of BOT mode.");
          }
          this.kills.Score = slot.Score;
        }
        else
        {
          slot.Score += score;
          AllUtils.CompleteMission(room, player, slot, this.kills, MissionType.NA, 0);
          this.kills.Score = score;
        }
        using (PROTOCOL_BATTLE_DEATH_ACK protocolBattleDeathAck = new PROTOCOL_BATTLE_DEATH_ACK(room, this.kills, slot, isBotMode))
          room.SendPacketToPlayers((SendPacket) protocolBattleDeathAck, SlotState.BATTLE, 0);
        RoomDeath.EndBattleByDeath(room, slot, isBotMode, this.isSuicide);
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_BATTLE_DEATH_REQ: " + ex.ToString());
        this._client.Close(0);
      }
    }
  }
}
