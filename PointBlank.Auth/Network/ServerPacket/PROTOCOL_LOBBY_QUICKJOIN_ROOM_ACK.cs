// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK : SendPacket
  {
    public override void write() => this.writeH((short) 5378);
  }
}
