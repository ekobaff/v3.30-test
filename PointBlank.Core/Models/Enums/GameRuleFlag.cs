// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Enums.GameRuleFlag
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using System;

namespace PointBlank.Core.Models.Enums
{
    [Flags]
    public enum GameRuleFlag
    {
        ไม่มี = 0,
        ห้ามใช้บาเรต = 1,
        ห้ามใช้ลูกซอง = 2,
        ห้ามใช้หน้ากาก = 4,
        กฎแข่ง = 8,


        //// Token: 0x040000F1 RID: 241
        //flag_0 = 0,
        //// Token: 0x040000F2 RID: 242
        //flag_1 = 1,
        //// Token: 0x040000F3 RID: 243
        //flag_2 = 2,
        //// Token: 0x040000F4 RID: 244
        //flag_3 = 4,
        //// Token: 0x040000F5 RID: 245
        //flag_4 = 8,
        //// Token: 0x040000F6 RID: 246
        //RPG7 = 16
    }
}
