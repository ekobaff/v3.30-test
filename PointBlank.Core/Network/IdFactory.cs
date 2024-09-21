// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Network.IdFactory
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

namespace PointBlank.Core.Network
{
  public class IdFactory
  {
    private static IdFactory Instance;
    private BitSet IdList = new BitSet();
    private BitSet SeedList = new BitSet();
    private int NextMinId = 0;
    private int NextMinSeed = 1;

    public int NextId()
    {
      int pos = 0;
      if (this.NextMinId != int.MinValue)
        pos = this.IdList.NextClearBit(this.NextMinId);
      this.IdList.Set(pos);
      this.NextMinId = pos + 1;
      return pos;
    }

    public ushort NextSeed()
    {
      ushort pos = 0;
      if (this.NextMinSeed != 0)
        pos = (ushort) this.SeedList.NextClearBit(this.NextMinSeed);
      this.SeedList.Set((int) pos);
      this.NextMinSeed = (int) pos + 1;
      return pos;
    }

    public static IdFactory GetInstance()
    {
      if (IdFactory.Instance == null)
        IdFactory.Instance = new IdFactory();
      return IdFactory.Instance;
    }
  }
}
