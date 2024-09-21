// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BASE_GET_USER_BASIC_INFO_REQ
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
  public class PROTOCOL_BASE_GET_USER_BASIC_INFO_REQ : ReceivePacket
  {
    private string Name;

    public PROTOCOL_BASE_GET_USER_BASIC_INFO_REQ(GameClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read() => this.Name = this.readUnicode(33);

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player.player_name.Length == 0 || player.player_name == this.Name)
          return;
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_GET_USER_BASIC_INFO_ACK(AccountManager.getAccount(this.Name, 1, 0) == null ? 2147489795U : 0U));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
