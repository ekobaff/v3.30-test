// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.SubHead.StageObjAnim
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.SubHead;

namespace PointBlank.Battle.Network.Actions.SubHead
{
  public class StageObjAnim
  {
    public static byte[] ReadInfo(ReceivePacket p) => p.readB(9);

    public static StageAnimInfo ReadInfo(ReceivePacket p, bool genLog)
    {
      StageAnimInfo stageAnimInfo = new StageAnimInfo()
      {
        _unk = p.readC(),
        _life = p.readUH(),
        _syncDate = p.readT(),
        _anim1 = p.readC(),
        _anim2 = p.readC()
      };
      if (genLog)
        Logger.warning("[StageObjAnim] Unk: " + stageAnimInfo._unk.ToString() + " Life: " + stageAnimInfo._life.ToString() + " Sync: " + stageAnimInfo._syncDate.ToString() + " Animation[1]: " + stageAnimInfo._anim1.ToString() + " Animation[2]: " + stageAnimInfo._anim2.ToString());
      return stageAnimInfo;
    }

    public static void WriteInfo(SendPacket s, ReceivePacket p) => s.writeB(StageObjAnim.ReadInfo(p));

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      StageAnimInfo stageAnimInfo = StageObjAnim.ReadInfo(p, genLog);
      s.writeC(stageAnimInfo._unk);
      s.writeH(stageAnimInfo._life);
      s.writeT(stageAnimInfo._syncDate);
      s.writeC(stageAnimInfo._anim1);
      s.writeC(stageAnimInfo._anim2);
    }
  }
}
