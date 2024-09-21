// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_QUEST_BUY_CARD_SET_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_QUEST_BUY_CARD_SET_ACK : SendPacket
  {
    private Account player;
    private uint _erro;

    public PROTOCOL_BASE_QUEST_BUY_CARD_SET_ACK(uint erro, Account p)
    {
      this._erro = erro;
      this.player = p;
    }

    public override void write()
    {
      this.writeH((short) 573);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeD(this.player._gp);
      this.writeC((byte) this.player._mission.actualMission);
      this.writeC((byte) this.player._mission.actualMission);
      this.writeC((byte) this.player._mission.card1);
      this.writeC((byte) this.player._mission.card2);
      this.writeC((byte) this.player._mission.card3);
      this.writeC((byte) this.player._mission.card4);
      this.writeB(ComDiv.getCardFlags(this.player._mission.mission1, this.player._mission.list1));
      this.writeB(ComDiv.getCardFlags(this.player._mission.mission2, this.player._mission.list2));
      this.writeB(ComDiv.getCardFlags(this.player._mission.mission3, this.player._mission.list3));
      this.writeB(ComDiv.getCardFlags(this.player._mission.mission4, this.player._mission.list4));
      this.writeC((byte) this.player._mission.mission1);
      this.writeC((byte) this.player._mission.mission2);
      this.writeC((byte) this.player._mission.mission3);
      this.writeC((byte) this.player._mission.mission4);
    }
  }
}
