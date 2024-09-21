// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CS_CREATE_CLAN_CONDITION_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CS_CREATE_CLAN_CONDITION_REQ : ReceivePacket
  {
    public PROTOCOL_CS_CREATE_CLAN_CONDITION_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        if (this._client == null)
          return;
        this._client.SendPacket((SendPacket) new PROTOCOL_CS_CREATE_CLAN_CONDITION_ACK());
      }
      catch
      {
      }
    }
  }
}
