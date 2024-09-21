// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ServerPacket.PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ServerPacket
{
  public class PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK : SendPacket
  {
    private uint Error;
    private List<QuickStart> Quicks;
    private QuickStart Select;
    private Room Room;

    public PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK(
      uint Error,
      List<QuickStart> Quicks = null,
      QuickStart Select = null,
      Room Room = null)
    {
      this.Error = Error;
      this.Quicks = Quicks;
      this.Select = Select;
      this.Room = Room;
    }

    public override void write()
    {
      this.writeH((short) 5378);
      this.writeD(this.Error);
      for (int index = 0; index < this.Quicks.Count; ++index)
      {
        QuickStart quick = this.Quicks[index];
        this.writeC((byte) quick.MapId);
        this.writeC((byte) quick.Rule);
        this.writeC((byte) quick.StageOptions);
        this.writeC((byte) quick.Type);
      }
      if (this.Error != 0U)
        return;
      this.writeC((byte) this.Room._channelId);
      this.writeD(this.Room._roomId);
      this.writeH((short) 1);
      if (this.Error != 0U)
      {
        this.writeC((byte) this.Select.MapId);
        this.writeC((byte) this.Select.Rule);
        this.writeC((byte) this.Select.StageOptions);
        this.writeC((byte) this.Select.Type);
      }
      else
      {
        this.writeC((byte) 0);
        this.writeC((byte) 0);
        this.writeC((byte) 0);
        this.writeC((byte) 0);
      }
      this.writeD(0);
      this.writeD(0);
      this.writeD(0);
      this.writeD(0);
    }
  }
}
