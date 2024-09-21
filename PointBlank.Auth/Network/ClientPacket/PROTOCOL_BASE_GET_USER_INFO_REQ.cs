// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ClientPacket.PROTOCOL_BASE_GET_USER_INFO_REQ
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Data.Model;
using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core;
using PointBlank.Core.Network;
using System;

namespace PointBlank.Auth.Network.ClientPacket
{
  public class PROTOCOL_BASE_GET_USER_INFO_REQ : ReceivePacket
  {
    public PROTOCOL_BASE_GET_USER_INFO_REQ(AuthClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || player._inventory._items.Count != 0)
          return;
        player.CheckVIP(); 
        player.LoadInventory();
        player.LoadMissionList();
        player.DiscountPlayerItems();
        player.LoadQuickstarts();
        this._client.SendPacket((SendPacket) new PROTOCOL_BASE_GET_USER_INFO_ACK(player));
      }
      catch (Exception ex)
      {
        Logger.warning("PROTOCOL_BASE_GET_USER_INFO_REQ: " + ex.ToString());
      }
    }
  }
}
