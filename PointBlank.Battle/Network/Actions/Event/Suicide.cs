// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Network.Actions.Event.Suicide
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Models.Event;
using SharpDX;
using System.Collections.Generic;

namespace PointBlank.Battle.Network.Actions.Event
{
  public class Suicide
  {
    public static List<SuicideInfo> ReadInfo(
      ReceivePacket p,
      bool genLog,
      bool OnlyBytes = false)
    {
      return Suicide.BaseReadInfo(p, OnlyBytes, genLog);
    }

    private static List<SuicideInfo> BaseReadInfo(
      ReceivePacket p,
      bool OnlyBytes,
      bool genLog)
    {
      List<SuicideInfo> suicideInfoList = new List<SuicideInfo>();
      int num = (int) p.readC();
      for (int index = 0; index < num; ++index)
      {
        SuicideInfo suicideInfo = new SuicideInfo()
        {
          HitInfo = p.readUD(),
          Extensions = p.readC(),
          WeaponId = p.readD(),
          PlayerPos = p.readUHVector()
        };
        if (OnlyBytes)
          ;
        if (genLog)
        {
          string[] strArray = new string[12]
          {
            "[",
            index.ToString(),
            "] Suicide: Hit: ",
            suicideInfo.HitInfo.ToString(),
            " WeaponId: ",
            suicideInfo.WeaponId.ToString(),
            " X: ",
            null,
            null,
            null,
            null,
            null
          };
          Half half = suicideInfo.PlayerPos.X;
          strArray[7] = half.ToString();
          strArray[8] = " Y: ";
          half = suicideInfo.PlayerPos.Y;
          strArray[9] = half.ToString();
          strArray[10] = " Z: ";
          half = suicideInfo.PlayerPos.Z;
          strArray[11] = half.ToString();
          Logger.warning(string.Concat(strArray));
        }
        suicideInfoList.Add(suicideInfo);
      }
      return suicideInfoList;
    }

    public static void ReadInfo(ReceivePacket p)
    {
      int num = (int) p.readC();
      p.Advance(15 * num);
    }

    public static void WriteInfo(SendPacket s, ReceivePacket p, bool genLog)
    {
      List<SuicideInfo> hits = Suicide.ReadInfo(p, genLog, true);
      Suicide.WriteInfo(s, hits);
    }

    public static void WriteInfo(SendPacket s, List<SuicideInfo> hits)
    {
      s.writeC((byte) hits.Count);
      for (int index = 0; index < hits.Count; ++index)
      {
        SuicideInfo hit = hits[index];
        s.writeD(hit.HitInfo);
        s.writeC(hit.Extensions);
        s.writeD(hit.WeaponId);
        s.writeHVector(hit.PlayerPos);
      }
    }
  }
}
