// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.SubHead.ObjectAnim
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.SubHead;

namespace PointBlank.Battle.Network.Actions.SubHead
{
  public class ObjectAnim
  {
    public static byte[] ReadInfo(ReceivePacket p) => p.readB(8);

    public static ObjectAnimInfo ReadInfo(ReceivePacket p, bool genLog)
    {
      ObjectAnimInfo objectAnimInfo = new ObjectAnimInfo()
      {
        _life = p.readUH(),
        _anim1 = p.readC(),
        _anim2 = p.readC(),
        _syncDate = p.readT()
      };
      if (genLog)
        Logger.warning("[ObjectAnim] Life: " + objectAnimInfo._life.ToString() + " Animation[1]: " + objectAnimInfo._anim1.ToString() + " Animation[2]: " + objectAnimInfo._anim2.ToString() + " Sync: " + objectAnimInfo._syncDate.ToString());
      return objectAnimInfo;
    }

    public static void WriteInfo(SendPacket s, ReceivePacket p) => s.writeB(ObjectAnim.ReadInfo(p));

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      ObjectAnimInfo objectAnimInfo = ObjectAnim.ReadInfo(p, genLog);
      s.writeH(objectAnimInfo._life);
      s.writeC(objectAnimInfo._anim1);
      s.writeC(objectAnimInfo._anim2);
      s.writeT(objectAnimInfo._syncDate);
    }
  }
}
