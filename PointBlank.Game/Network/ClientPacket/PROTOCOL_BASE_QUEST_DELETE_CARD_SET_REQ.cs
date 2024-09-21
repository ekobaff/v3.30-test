// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ : ReceivePacket
  {
    private uint erro;
    private int missionIdx;

    public PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.missionIdx = (int) this.readC();

    public override void run()
    {
      try
      {
        PointBlank.Game.Data.Model.Account player = this._client._player;
        if (player == null)
          return;
				PlayerMissions missions = player._mission;
				bool failed = false;
				if (this.missionIdx >= 3 || (this.missionIdx == 0 && missions.mission1 == 0) || (this.missionIdx == 1 && missions.mission2 == 0) || (this.missionIdx == 2 && missions.mission3 == 0))
				{
					failed = true;
				}
				if (!failed && PlayerManager.updateMissionId(player.player_id, 0, this.missionIdx) && ComDiv.updateDB("player_missions", "owner_id", player.player_id, new string[]
				{
						"card" + (this.missionIdx + 1).ToString(),
						"mission" + (this.missionIdx + 1).ToString()
				}, new object[]
				{
						0,
						new byte[0]
				}))
				{
					if (this.missionIdx == 0)
					{
						missions.mission1 = 0;
						missions.card1 = 0;
						missions.list1 = new byte[40];
					}
					else if (this.missionIdx == 1)
					{
						missions.mission2 = 0;
						missions.card2 = 0;
						missions.list2 = new byte[40];
					}
					else if (this.missionIdx == 2)
					{
						missions.mission3 = 0;
						missions.card3 = 0;
						missions.list3 = new byte[40];
					}
				}
				else
				{
					this.erro = 2147487824U;

				}
				this._client.SendPacket((SendPacket) new PROTOCOL_BASE_QUEST_DELETE_CARD_SET_ACK(this.erro, player));
      }
      catch (Exception ex)
      {
        Logger.info("PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ: " + ex.ToString());
      }
    }
  }
}
