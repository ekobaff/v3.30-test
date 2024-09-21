// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.SubHead.StageInfoObjStatic
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.SubHead;

namespace PointBlank.Battle.Network.Actions.SubHead
{
  public class StageInfoObjStatic
  {
    public static StageStaticInfo ReadSyncInfo(ReceivePacket p, bool genLog)
    {
      StageStaticInfo stageStaticInfo = new StageStaticInfo()
      {
        _isDestroyed = p.readC()
      };
      if (genLog)
        Logger.warning("[StageInfoObjStatic] Destroyed: " + stageStaticInfo._isDestroyed.ToString());
      return stageStaticInfo;
    }

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      StageStaticInfo stageStaticInfo = StageInfoObjStatic.ReadSyncInfo(p, genLog);
      s.writeC(stageStaticInfo._isDestroyed);
    }
  }
}
