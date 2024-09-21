// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_MESSENGER_NOTE_DELETE_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_MESSENGER_NOTE_DELETE_ACK : SendPacket
  {
    private uint _erro;
    private List<object> _objs;

    public PROTOCOL_MESSENGER_NOTE_DELETE_ACK(uint erro, List<object> objs)
    {
      this._erro = erro;
      this._objs = objs;
    }

    public override void write()
    {
      this.writeH((short) 937);
      this.writeD(this._erro);
      this.writeC((byte) this._objs.Count);
      foreach (int num in this._objs)
        this.writeD(num);
    }
  }
}
