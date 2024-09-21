// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Packets.PROTOCOL_CONNECT
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

namespace PointBlank.Battle.Network.Packets
{
  public class PROTOCOL_CONNECT
  {
    public static byte[] getCode()
    {
      using (SendPacket sendPacket = new SendPacket())
      {
        sendPacket.writeC((byte) 66);
        sendPacket.writeC((byte) 0);
        sendPacket.writeT(0.0f);
        sendPacket.writeC((byte) 0);
        sendPacket.writeH((short) 14);
        sendPacket.writeD(0);
        sendPacket.writeC((byte) 8);
        return sendPacket.mstream.ToArray();
      }
    }
  }
}
