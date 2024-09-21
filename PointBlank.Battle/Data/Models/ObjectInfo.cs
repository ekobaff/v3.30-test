// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Models.ObjectInfo
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using System;

namespace PointBlank.Battle.Data.Models
{
  public class ObjectInfo
  {
    public int Id;
    public int Life = 100;
    public int DestroyState;
    public AnimModel Animation;
    public DateTime UseDate;
    public ObjectModel Model;

    public ObjectInfo()
    {
    }

    public ObjectInfo(int Id) => this.Id = Id;
  }
}
