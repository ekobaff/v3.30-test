// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ : ReceivePacket
  {
    private int cardIdx;
    private int actualMission;
    private int cardFlags;

    public PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.actualMission = (int) this.readC();
      this.cardIdx = (int) this.readC();
      this.cardFlags = (int) this.readUH();
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        PointBlank.Game.Data.Model.Account player = this._client._player;
        if (player == null)
          return;
        PlayerMissions mission = player._mission;
        DBQuery dbQuery = new DBQuery();
        if (mission.getCard(this.actualMission) != this.cardIdx)
        {
          if (this.actualMission == 0)
            mission.card1 = this.cardIdx;
          else if (this.actualMission == 1)
            mission.card2 = this.cardIdx;
          else if (this.actualMission == 2)
            mission.card3 = this.cardIdx;
          else if (this.actualMission == 3)
            mission.card4 = this.cardIdx;
          dbQuery.AddQuery("card" + (this.actualMission + 1).ToString(), (object) this.cardIdx);
        }
        mission.selectedCard = this.cardFlags == (int) ushort.MaxValue;
        if (mission.actualMission != this.actualMission)
        {
          dbQuery.AddQuery("actual_mission", (object) this.actualMission);
          mission.actualMission = this.actualMission;
        }
        ComDiv.updateDB("player_missions", "owner_id", (object) this._client.player_id, dbQuery.GetTables(), dbQuery.GetValues());
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ: " + ex.ToString());
      }
    }
  }
}
