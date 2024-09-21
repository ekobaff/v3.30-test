// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_AUTH_FIND_USER_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_AUTH_FIND_USER_REQ : ReceivePacket
  {
    private string name;

    public PROTOCOL_AUTH_FIND_USER_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.name = this.readUnicode(33);

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player.player_name.Length == 0 || player.player_name == this.name)
          return;
        player.FindPlayer = this.name;
        Account account = AccountManager.getAccount(player.FindPlayer, 1, 0);
        this._client.SendPacket((SendPacket) new PROTOCOL_AUTH_FIND_USER_ACK(account == null ? 2147489795U : 0U, account));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
