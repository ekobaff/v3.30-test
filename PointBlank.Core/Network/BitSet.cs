// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Network.BitSet
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System;
using System.Text;

namespace PointBlank.Core.Network
{
  [Serializable]
  public class BitSet : ICloneable
  {
    private const long serialVersionUID = 7997698588986878753;
    private const int LONG_MASK = 63;
    private long[] bits;

    public BitSet()
      : this(64)
    {
    }

    public BitSet(int nbits)
    {
      if (nbits < 0)
        throw new ArgumentOutOfRangeException("nbits may not be negative");
      uint length = (uint) (nbits >> 6);
      if ((nbits & 63) != 0)
        ++length;
      this.bits = new long[(int) length];
    }

    public void And(BitSet bs)
    {
      int num = Math.Min(this.bits.Length, bs.bits.Length);
      int index;
      for (index = 0; index < num; ++index)
        this.bits[index] &= bs.bits[index];
      while (index < this.bits.Length)
        this.bits[index++] = 0L;
    }

    public void AndNot(BitSet bs)
    {
      int index = Math.Min(this.bits.Length, bs.bits.Length);
      while (--index >= 0)
        this.bits[index] &= ~bs.bits[index];
    }

    public int Cardinality()
    {
      uint num1 = 0;
      for (int index = this.bits.Length - 1; index >= 0; --index)
      {
        long bit = this.bits[index];
        switch (bit)
        {
          case -1:
            num1 += 64U;
            continue;
          case 0:
            continue;
          default:
            long num2 = (bit >> 1 & 6148914691236517205L) + (bit & 6148914691236517205L);
            long num3 = (num2 >> 2 & 3689348814741910323L) + (num2 & 3689348814741910323L);
            uint num4 = (uint) ((num3 >> 32) + num3);
            uint num5 = (uint) (((int) (num4 >> 4) & 252645135) + ((int) num4 & 252645135));
            uint num6 = (uint) (((int) (num5 >> 8) & 16711935) + ((int) num5 & 16711935));
            num1 += (uint) (((int) (num6 >> 16) & (int) ushort.MaxValue) + ((int) num6 & (int) ushort.MaxValue));
            continue;
        }
      }
      return (int) num1;
    }

    public void Clear(int pos)
    {
      int lastElt = pos >> 6;
      this.Ensure(lastElt);
      this.bits[lastElt] &= ~(1L << pos);
    }

    public void Clear(int from, int to)
    {
      if (from < 0 || from > to)
        throw new ArgumentOutOfRangeException();
      if (from == to)
        return;
      uint index1 = (uint) (from >> 6);
      uint lastElt = (uint) (to >> 6);
      this.Ensure((int) lastElt);
      if ((int) index1 == (int) lastElt)
      {
        this.bits[(int) lastElt] &= (1L << from) - 1L | -1L << to;
      }
      else
      {
        this.bits[(int) index1] &= (1L << from) - 1L;
        this.bits[(int) lastElt] &= -1L << to;
        for (int index2 = (int) index1 + 1; (long) index2 < (long) lastElt; ++index2)
          this.bits[index2] = 0L;
      }
    }

    public object Clone()
    {
      try
      {
        BitSet bitSet = ObjectCopier.Clone<BitSet>(this);
        bitSet.bits = (long[]) this.bits.Clone();
        return (object) bitSet;
      }
      catch
      {
        return (object) null;
      }
    }

    public override bool Equals(object obj)
    {
      if (!(obj.GetType() == typeof (BitSet)))
        return false;
      BitSet bitSet = (BitSet) obj;
      int num = Math.Min(this.bits.Length, bitSet.bits.Length);
      int index1;
      for (index1 = 0; index1 < num; ++index1)
      {
        if (this.bits[index1] != bitSet.bits[index1])
          return false;
      }
      for (int index2 = index1; index2 < this.bits.Length; ++index2)
      {
        if (this.bits[index2] != 0L)
          return false;
      }
      for (int index3 = index1; index3 < bitSet.bits.Length; ++index3)
      {
        if (bitSet.bits[index3] != 0L)
          return false;
      }
      return true;
    }

    public void Flip(int index)
    {
      int lastElt = index >> 6;
      this.Ensure(lastElt);
      this.bits[lastElt] ^= 1L << index;
    }

    public void Flip(int from, int to)
    {
      if (from < 0 || from > to)
        throw new ArgumentOutOfRangeException();
      if (from == to)
        return;
      uint index1 = (uint) (from >> 6);
      uint lastElt = (uint) (to >> 6);
      this.Ensure((int) lastElt);
      if ((int) index1 == (int) lastElt)
      {
        this.bits[(int) lastElt] ^= -1L << from & (1L << to) - 1L;
      }
      else
      {
        this.bits[(int) index1] ^= -1L << from;
        this.bits[(int) lastElt] ^= (1L << to) - 1L;
        for (int index2 = (int) index1 + 1; (long) index2 < (long) lastElt; ++index2)
          this.bits[index2] ^= -1L;
      }
    }

    public bool Get(int pos)
    {
      int index = pos >> 6;
      return index < this.bits.Length && (this.bits[index] & 1L << pos) != 0L;
    }

    public BitSet Get(int from, int to)
    {
      BitSet bitSet = from >= 0 && from <= to ? new BitSet(to - from) : throw new ArgumentOutOfRangeException();
      uint sourceIndex = (uint) (from >> 6);
      if ((long) sourceIndex >= (long) this.bits.Length || to == from)
        return bitSet;
      int num1 = from & 63;
      uint val1 = (uint) (to >> 6);
      if (num1 == 0)
      {
        uint length = Math.Min((uint) ((int) val1 - (int) sourceIndex + 1), (uint) this.bits.Length - sourceIndex);
        Array.Copy((Array) this.bits, (long) sourceIndex, (Array) bitSet.bits, 0L, (long) length);
        if ((long) val1 < (long) this.bits.Length)
          bitSet.bits[(int) val1 - (int) sourceIndex] &= (1L << to) - 1L;
        return bitSet;
      }
      uint num2 = Math.Min(val1, (uint) (this.bits.Length - 1));
      int num3 = 64 - num1;
      int index = 0;
      while (sourceIndex < num2)
      {
        bitSet.bits[index] = this.bits[(int) sourceIndex] >> num1 | this.bits[(int) sourceIndex + 1] << num3;
        ++sourceIndex;
        ++index;
      }
      if ((to & 63) > num1)
        bitSet.bits[index++] = this.bits[(int) sourceIndex] >> num1;
      if ((long) val1 < (long) this.bits.Length)
        bitSet.bits[index - 1] &= (1L << to - from) - 1L;
      return bitSet;
    }

    public override int GetHashCode()
    {
      long num = 1234;
      int length = this.bits.Length;
      while (length > 0)
        num ^= (long) length * this.bits[--length];
      return (int) (num >> 32 ^ num);
    }

    public bool Intersects(BitSet set)
    {
      int index = Math.Min(this.bits.Length, set.bits.Length);
      while (--index >= 0)
      {
        if ((this.bits[index] & set.bits[index]) != 0L)
          return true;
      }
      return false;
    }

    public bool IsEmpty()
    {
      for (int index = this.bits.Length - 1; index >= 0; --index)
      {
        if (this.bits[index] != 0L)
          return false;
      }
      return true;
    }

    public int Length
    {
      get
      {
        int index = this.bits.Length - 1;
        while (index >= 0 && this.bits[index] == 0L)
          --index;
        if (index < 0)
          return 0;
        long bit = this.bits[index];
        int length = (index + 1) * 64;
        for (; bit >= 0L; bit <<= 1)
          --length;
        return length;
      }
    }

    public int NextClearBit(int from)
    {
      int index = from >> 6;
      long num = 1L << from;
label_6:
      if (index >= this.bits.Length)
        return from;
      long bit = this.bits[index];
      while ((bit & num) != 0L)
      {
        num <<= 1;
        ++from;
        if (num == 0L)
        {
          num = 1L;
          ++index;
          goto label_6;
        }
      }
      return from;
    }

    public int NextSetBit(int from)
    {
      int index = from >> 6;
      long num = 1L << from;
label_6:
      if (index >= this.bits.Length)
        return -1;
      long bit = this.bits[index];
      while ((bit & num) == 0L)
      {
        num <<= 1;
        ++from;
        if (num == 0L)
        {
          num = 1L;
          ++index;
          goto label_6;
        }
      }
      return from;
    }

    public void Or(BitSet bs)
    {
      this.Ensure(bs.bits.Length - 1);
      for (int index = bs.bits.Length - 1; index >= 0; --index)
        this.bits[index] |= bs.bits[index];
    }

    public void Set(int pos)
    {
      int lastElt = pos >> 6;
      this.Ensure(lastElt);
      this.bits[lastElt] |= 1L << pos;
    }

    public void Set(int index, bool value)
    {
      if (value)
        this.Set(index);
      else
        this.Clear(index);
    }

    public void Set(int from, int to)
    {
      if (from < 0 || from > to)
        throw new ArgumentOutOfRangeException();
      if (from == to)
        return;
      uint index1 = (uint) (from >> 6);
      uint lastElt = (uint) (to >> 6);
      this.Ensure((int) lastElt);
      if ((int) index1 == (int) lastElt)
      {
        this.bits[(int) lastElt] |= -1L << from & (1L << to) - 1L;
      }
      else
      {
        this.bits[(int) index1] |= -1L << from;
        this.bits[(int) lastElt] |= (1L << to) - 1L;
        for (int index2 = (int) index1 + 1; (long) index2 < (long) lastElt; ++index2)
          this.bits[index2] = -1L;
      }
    }

    public void Set(int from, int to, bool value)
    {
      if (value)
        this.Set(from, to);
      else
        this.Clear(from, to);
    }

    public int Size => this.bits.Length * 64;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("{");
      bool flag = true;
      for (int index1 = 0; index1 < this.bits.Length; ++index1)
      {
        long num = 1;
        long bit = this.bits[index1];
        if (bit != 0L)
        {
          for (int index2 = 0; index2 < 64; ++index2)
          {
            if ((bit & num) != 0L)
            {
              if (!flag)
                stringBuilder.Append(", ");
              stringBuilder.Append(64 * index1 + index2);
              flag = false;
            }
            num <<= 1;
          }
        }
      }
      return stringBuilder.Append("}").ToString();
    }

    public void XOr(BitSet bs)
    {
      this.Ensure(bs.bits.Length - 1);
      for (int index = bs.bits.Length - 1; index >= 0; --index)
        this.bits[index] ^= bs.bits[index];
    }

    private void Ensure(int lastElt)
    {
      if (lastElt < this.bits.Length)
        return;
      long[] destinationArray = new long[lastElt + 1];
      Array.Copy((Array) this.bits, 0, (Array) destinationArray, 0, this.bits.Length);
      this.bits = destinationArray;
    }

    public bool ContainsAll(BitSet other)
    {
      for (int index = other.bits.Length - 1; index >= 0; --index)
      {
        if ((this.bits[index] & other.bits[index]) != other.bits[index])
          return false;
      }
      return true;
    }
  }
}
