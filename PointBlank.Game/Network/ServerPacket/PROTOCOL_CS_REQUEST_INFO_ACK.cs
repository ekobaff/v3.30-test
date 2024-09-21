// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_REQUEST_INFO_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_REQUEST_INFO_ACK : SendPacket
  {
    private string text;
    private uint _erro;
    private Account p;

    public PROTOCOL_CS_REQUEST_INFO_ACK(long id, string txt)
    {
      this.text = txt;
      this.p = AccountManager.getAccount(id, 0);
      if (this.p != null && this.text != null)
        return;
      this._erro = 2147483648U;
    }

    public override void write()
    {
      this.writeH((short) 1845);
      this.writeD(this._erro);
      if (this._erro != 0U)
        return;
      this.writeQ(this.p.player_id);
      this.writeUnicode(this.p.player_name, 66);
      this.writeC((byte) this.p._rank);
      this.writeD(this.p._statistic.kills_count);
      this.writeD(this.p._statistic.deaths_count);
      this.writeD(this.p._statistic.fights);
      this.writeD(this.p._statistic.fights_win);
      this.writeD(this.p._statistic.fights_lost);
      this.writeUnicode(this.text, true);
    }
  }
}
