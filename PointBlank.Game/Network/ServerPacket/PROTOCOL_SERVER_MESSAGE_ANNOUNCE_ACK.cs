// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK : SendPacket
  {
    private string _message;

    public PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(string msg)
    {
      this._message = msg;
      if (msg.Length < 1024)
        return;
      Logger.error("Message with size bigger than 1024 sent!!");
    }

    public override void write()
    {
      this.writeH((short) 2567);
      this.writeH((short) 0);
      this.writeD(0);
      this.writeH((ushort) this._message.Length);
      this.writeUnicode(this._message, false);
      this.writeD(2);
    }
  }
}
