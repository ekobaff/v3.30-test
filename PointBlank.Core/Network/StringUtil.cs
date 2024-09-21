// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Network.StringUtil
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System.Text;

namespace PointBlank.Core.Network
{
  public class StringUtil
  {
    private static StringBuilder builder;

    public StringUtil() => StringUtil.builder = new StringBuilder();

    public void AppendLine(string text) => StringUtil.builder.AppendLine(text);

    public string getString() => StringUtil.builder.Length == 0 ? StringUtil.builder.ToString() : StringUtil.builder.Remove(StringUtil.builder.Length - 1, 1).ToString();
  }
}
