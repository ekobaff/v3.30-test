// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_CS_PAGE_CHATTING_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_CS_PAGE_CHATTING_ACK : SendPacket
  {
    private string sender;
    private string message;
    private int type;
    private int bantime;
    private int name_color;
    private bool isGM;

    public PROTOCOL_CS_PAGE_CHATTING_ACK(Account p, string msg)
    {
      this.sender = p.player_name;
      this.message = msg;
      this.isGM = p.UseChatGM();
      this.name_color = p.name_color;
    }

    public PROTOCOL_CS_PAGE_CHATTING_ACK(int type, int bantime)
    {
      this.type = type;
      this.bantime = bantime;
    }

    public override void write()
    {
      this.writeH((short) 1911);
      this.writeC((byte) this.type);
      if (this.type == 0)
      {
        this.writeC((byte) (this.sender.Length + 1));
        this.writeUnicode(this.sender, true);
        this.writeC(this.isGM);
        this.writeC((byte) this.name_color);
        this.writeC((byte) (this.message.Length + 1));
        this.writeUnicode(this.message, true);
      }
      else
        this.writeD(this.bantime);
    }
  }
}
