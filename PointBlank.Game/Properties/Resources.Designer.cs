﻿// Decompiled with JetBrains decompiler
// Type: PointBlank.Game.Properties.Resources
// Assembly: PointBlank.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9391C126-F6F2-4165-85EA-1FCDF75131C4
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Game.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PointBlank.Game.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (PointBlank.Game.Properties.Resources.resourceMan == null)
          PointBlank.Game.Properties.Resources.resourceMan = new ResourceManager("PointBlank.Game.Properties.Resources", typeof (PointBlank.Game.Properties.Resources).Assembly);
        return PointBlank.Game.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => PointBlank.Game.Properties.Resources.resourceCulture;
      set => PointBlank.Game.Properties.Resources.resourceCulture = value;
    }
  }
}
