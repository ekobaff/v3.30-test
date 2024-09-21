// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_BASE_QUEST_DELETE_CARD_SET_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_BASE_QUEST_DELETE_CARD_SET_ACK : SendPacket
  {
    private uint erro;
    private Account p;

    public PROTOCOL_BASE_QUEST_DELETE_CARD_SET_ACK(uint erro, Account player)
    {
      this.erro = erro;
      this.p = player;
    }

    public override void write()
    {
      this.writeH((short) 575);
      this.writeD(this.erro);
      if (this.erro != 0U)
        return;
      this.writeC((byte) this.p._mission.actualMission);
      this.writeC((byte) this.p._mission.card1);
      this.writeC((byte) this.p._mission.card2);
      this.writeC((byte) this.p._mission.card3);
      this.writeC((byte) this.p._mission.card4);
      this.writeB(ComDiv.getCardFlags(this.p._mission.mission1, this.p._mission.list1));
      this.writeB(ComDiv.getCardFlags(this.p._mission.mission2, this.p._mission.list2));
      this.writeB(ComDiv.getCardFlags(this.p._mission.mission3, this.p._mission.list3));
      this.writeB(ComDiv.getCardFlags(this.p._mission.mission4, this.p._mission.list4));
      this.writeC((byte) this.p._mission.mission1);
      this.writeC((byte) this.p._mission.mission2);
      this.writeC((byte) this.p._mission.mission3);
      this.writeC((byte) this.p._mission.mission4);
    }
  }
}
