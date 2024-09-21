// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_SERVER_MESSAGE_INVITED_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_SERVER_MESSAGE_INVITED_ACK : SendPacket
  {
    private Account sender;
    private Room room;

    public PROTOCOL_SERVER_MESSAGE_INVITED_ACK(Account sender, Room room)
    {
      this.sender = sender;
      this.room = room;
    }

    public override void write()
    {
      this.writeH((short) 2565);
      this.writeUnicode(this.sender.player_name, 66);
      this.writeD(this.room._roomId);
      this.writeQ(this.sender.player_id);
      this.writeS(this.room.password, 4);
    }
  }
}
