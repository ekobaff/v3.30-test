// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_AUTH_SHOP_GET_GIFTLIST_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_AUTH_SHOP_GET_GIFTLIST_REQ : ReceivePacket
  {
    public PROTOCOL_AUTH_SHOP_GET_GIFTLIST_REQ(GameClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        this._client.SendPacket((SendPacket) new PROTOCOL_AUTH_SHOP_GET_GIFTLIST_ACK(2148110592U));
      }
      catch
      {
      }
    }
  }
}
