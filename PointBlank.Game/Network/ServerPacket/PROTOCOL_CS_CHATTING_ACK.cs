// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_CHATTING_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_CHATTING_ACK : SendPacket
  {
    private string text;
    private Account p;
    private int type;
    private int bantime;

    public PROTOCOL_CS_CHATTING_ACK(string text, Account player)
    {
      this.text = text;
      this.p = player;
    }

    public PROTOCOL_CS_CHATTING_ACK(int type, int bantime)
    {
      this.type = type;
      this.bantime = bantime;
    }

    public override void write()
    {
      this.writeH((short) 1879);
      if (this.type == 0)
      {
        this.writeC((byte) (this.p.player_name.Length + 1));
        this.writeUnicode(this.p.player_name, true);
        this.writeC(this.p.UseChatGM());
        this.writeC((byte) this.p.name_color);
        this.writeC((byte) (this.text.Length + 1));
        this.writeUnicode(this.text, true);
      }
      else
        this.writeD(this.bantime);
    }
  }
}
