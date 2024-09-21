using System;

namespace PointBlank.Core.Models.Enums
{
    // Token: 0x02000034 RID: 52
    [Flags]
    public enum EffectFlag
    {
        // Token: 0x040000D9 RID: 217
        Ammo40 = 1,
        // Token: 0x040000DA RID: 218
        Ammo10 = 2,
        // Token: 0x040000DB RID: 219
        GetDroppedWeapon = 4,
        // Token: 0x040000DC RID: 220
        QuickChangeWeapon = 16,
        // Token: 0x040000DD RID: 221
        QuickChangeReload = 64,
        // Token: 0x040000DE RID: 222
        Invincible = 1,
        // Token: 0x040000DF RID: 223
        FullMetalJack = 4,
        // Token: 0x040000E0 RID: 224
        HollowPoint = 16,
        // Token: 0x040000E1 RID: 225
        HollowPointPlus = 64,
        // Token: 0x040000E2 RID: 226
        C4SpeedKit = 128,
        // Token: 0x040000E3 RID: 227
        ExtraGrenade = 1,
        // Token: 0x040000E4 RID: 228
        ExtraThrowGrenade = 2,
        // Token: 0x040000E5 RID: 229
        JackHollowPoint = 4,
        // Token: 0x040000E6 RID: 230
        HP5 = 8,
        // Token: 0x040000E7 RID: 231
        HP10 = 16,
        // Token: 0x040000E8 RID: 232
        Defense5 = 32,
        // Token: 0x040000E9 RID: 233
        Defense10 = 64,
        // Token: 0x040000EA RID: 234
        Defense20 = 128,
        // Token: 0x040000EB RID: 235
        Defense90 = 1,
        // Token: 0x040000EC RID: 236
        Respawn20 = 2,
        // Token: 0x040000ED RID: 237
        Respawn30 = 8,
        // Token: 0x040000EE RID: 238
        Respawn50 = 32,
        // Token: 0x040000EF RID: 239
        Respawn100 = 128
    }
}
