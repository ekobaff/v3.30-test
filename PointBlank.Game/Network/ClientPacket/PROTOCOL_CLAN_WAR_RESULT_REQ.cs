// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Network.ClientPacket.PROTOCOL_CLAN_WAR_RESULT_REQ
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

namespace PointBlank.Game.Network.ClientPacket
{
  public class PROTOCOL_CLAN_WAR_RESULT_REQ : ReceivePacket
  {
    private int ClanId;

    public PROTOCOL_CLAN_WAR_RESULT_REQ(GameClient Client, byte[] Buffer) => this.makeme(Client, Buffer);

    public override void read() => this.ClanId = this.readD();

    public override void run()
    {
    }
  }
}
