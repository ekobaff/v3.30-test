// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_MESSENGER_NOTE_CHECK_READED_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_MESSENGER_NOTE_CHECK_READED_ACK : SendPacket
  {
    private List<int> msgs;

    public PROTOCOL_MESSENGER_NOTE_CHECK_READED_ACK(List<int> msgs) => this.msgs = msgs;

    public override void write()
    {
      this.writeH((short) 935);
      this.writeC((byte) this.msgs.Count);
      for (int index = 0; index < this.msgs.Count; ++index)
        this.writeD(this.msgs[index]);
    }
  }
}
