// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Models.MapModel
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using System.Collections.Generic;

namespace PointBlank.Battle.Data.Models
{
  public class MapModel
  {
    public int Id;
    public List<ObjectModel> Objects = new List<ObjectModel>();
    public List<BombPosition> Bombs = new List<BombPosition>();

    public BombPosition GetBomb(int BombId)
    {
      try
      {
        return this.Bombs[BombId];
      }
      catch
      {
        return (BombPosition) null;
      }
    }
  }
}
