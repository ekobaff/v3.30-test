﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CS_MEMBER_CONTEXT_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CS_MEMBER_CONTEXT_REQ : ReceivePacket
  {
    public PROTOCOL_CS_MEMBER_CONTEXT_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        int findClanId = player.FindClanId;
        if (findClanId == 0)
          this._client.SendPacket((SendPacket) new PROTOCOL_CS_MEMBER_CONTEXT_ACK(-1));
        else
          this._client.SendPacket((SendPacket) new PROTOCOL_CS_MEMBER_CONTEXT_ACK(0, PlayerManager.getClanPlayers(findClanId)));
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
