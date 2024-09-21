// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_AUTH_SEND_WHISPER_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_AUTH_SEND_WHISPER_ACK : SendPacket
  {
    private string name;
    private string msg;
    private uint erro;
    private int type;
    private int bantime;

    public PROTOCOL_AUTH_SEND_WHISPER_ACK(string name, string msg, uint erro)
    {
      this.name = name;
      this.msg = msg;
      this.erro = erro;
    }

    public PROTOCOL_AUTH_SEND_WHISPER_ACK(int type, int bantime)
    {
      this.type = type;
      this.bantime = bantime;
    }

    public override void write()
    {
      this.writeH((short) 805);
      this.writeC((byte) this.type);
      if (this.type == 0)
      {
        this.writeD(this.erro);
        this.writeUnicode(this.name, 66);
        if (this.erro != 0U)
          return;
        this.writeH((ushort) (this.msg.Length + 1));
        this.writeUnicode(this.msg, true);
      }
      else
        this.writeD(this.bantime);
    }
  }
}
