// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_REQ : ReceivePacket
  {
    private uint sessionId;

    public PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.sessionId = this.readUD();

    public override void run()
    {
      Account player1 = this._client._player;
      if (player1 == null)
        return;
      long player2 = 0;
      try
      {
        player2 = player1.getChannel().getPlayer(this.sessionId)._playerId;
      }
      catch
      {
      }
      this._client.SendPacket((SendPacket) new PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_ACK(player2));
    }
  }
}
