// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Network.ServerPacket;

namespace PointBlank.Game.Network.ClientPacket
{
  internal class PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_REQ : ReceivePacket
  {
    private int SlotId;
    private int Unk;

    public PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_REQ(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.SlotId = (int) this.readC();
      this.Unk = (int) this.readC();
    }

    public override void run()
    {
      if (this._client == null || this._client._player == null || this._client._player._room == null)
        return;
      this._client.SendPacket((SendPacket) new PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_ACK(this._client._player._room.getPlayerBySlot(this._client._player._room.getSlot(this.SlotId))));
    }
  }
}
