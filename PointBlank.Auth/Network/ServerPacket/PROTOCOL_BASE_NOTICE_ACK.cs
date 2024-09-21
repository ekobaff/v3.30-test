// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_BASE_NOTICE_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Core.Managers.Server;
using PointBlank.Core.Network;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_BASE_NOTICE_ACK : SendPacket
  {
    public override void write()
    {
      ServerConfig config = AuthManager.Config;
      this.writeH((short) 662);
      this.writeH((short) 0);
      this.writeD(config.ChatColor);
      this.writeD(config.AnnouceColor);
      this.writeH((ushort) config.Chat.Length);
      this.writeText(config.Chat, config.Chat.Length);
      this.writeH((ushort) config.Annouce.Length);
      this.writeText(config.Annouce, config.Annouce.Length);
    }
  }
}
