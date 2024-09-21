// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CS_CHECK_MARK_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CS_CHECK_MARK_REQ : ReceivePacket
  {
    private uint logo;
    private uint erro;

    public PROTOCOL_CS_CHECK_MARK_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.logo = this.readUD();

    public override void run()
    {
      Account player = this._client._player;
      if (player == null || (int) ClanManager.getClan(player.clanId)._logo == (int) this.logo || ClanManager.isClanLogoExist(this.logo))
        this.erro = 2147483648U;
      this._client.SendPacket((SendPacket) new PROTOCOL_CS_CHECK_MARK_ACK(this.erro));
    }
  }
}
