// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ClientPacket.PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using System;
using System.Collections.Generic;

namespace PointBlank.Auth.Network.ClientPacket
{
  public class PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ : ReceivePacket
  {
    private int Select;
    private List<QuickStart> Quicks = new List<QuickStart>();

    public PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ(AuthClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read()
    {
      this.Select = (int) this.readC();
      for (int index = 0; index < 3; ++index)
        this.Quicks.Add(new QuickStart()
        {
          MapId = (int) this.readC(),
          Rule = (int) this.readC(),
          StageOptions = (int) this.readC(),
          Type = (int) this.readC()
        });
    }

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Este sistema ainda não está ativado."));
      }
      catch (Exception ex)
      {
        Logger.error("PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ: " + ex.ToString());
      }
    }
  }
}
