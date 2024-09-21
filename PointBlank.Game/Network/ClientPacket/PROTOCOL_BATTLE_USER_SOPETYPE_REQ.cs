// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_BATTLE_USER_SOPETYPE_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_BATTLE_USER_SOPETYPE_REQ : ReceivePacket
  {
    private int Sight;

    public PROTOCOL_BATTLE_USER_SOPETYPE_REQ(GameClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read() => this.Sight = (int) this.readC();

    public override void run()
    {
      Account player = this._client._player;
      Room room = player._room;
      if (player == null)
        return;
      player.Sight = this.Sight;
    }
  }
}
