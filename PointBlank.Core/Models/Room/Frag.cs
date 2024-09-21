using System;
using PointBlank.Core.Models.Enums;

namespace PointBlank.Core.Models.Room
{
    // Token: 0x0200002B RID: 43
    public class Frag
    {
        // Token: 0x1700002C RID: 44
        // (get) Token: 0x06000164 RID: 356 RVA: 0x00002C9B File Offset: 0x00000E9B
        // (set) Token: 0x06000165 RID: 357 RVA: 0x00002CA3 File Offset: 0x00000EA3
        public byte victimWeaponClass { get; set; }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x06000166 RID: 358 RVA: 0x00002CAC File Offset: 0x00000EAC
        // (set) Token: 0x06000167 RID: 359 RVA: 0x00002CB4 File Offset: 0x00000EB4
        public byte hitspotInfo { get; set; }

        // Token: 0x1700002E RID: 46
        // (get) Token: 0x06000168 RID: 360 RVA: 0x00002CBD File Offset: 0x00000EBD
        // (set) Token: 0x06000169 RID: 361 RVA: 0x00002CC5 File Offset: 0x00000EC5
        public byte flag { get; set; }

        // Token: 0x1700002F RID: 47
        // (get) Token: 0x0600016A RID: 362 RVA: 0x00002CCE File Offset: 0x00000ECE
        // (set) Token: 0x0600016B RID: 363 RVA: 0x00002CD6 File Offset: 0x00000ED6
        public KillingMessage killFlag { get; set; }

        // Token: 0x17000030 RID: 48
        // (get) Token: 0x0600016C RID: 364 RVA: 0x00002CDF File Offset: 0x00000EDF
        // (set) Token: 0x0600016D RID: 365 RVA: 0x00002CE7 File Offset: 0x00000EE7
        public float x { get; set; }

        // Token: 0x17000031 RID: 49
        // (get) Token: 0x0600016E RID: 366 RVA: 0x00002CF0 File Offset: 0x00000EF0
        // (set) Token: 0x0600016F RID: 367 RVA: 0x00002CF8 File Offset: 0x00000EF8
        public float y { get; set; }

        // Token: 0x17000032 RID: 50
        // (get) Token: 0x06000170 RID: 368 RVA: 0x00002D01 File Offset: 0x00000F01
        // (set) Token: 0x06000171 RID: 369 RVA: 0x00002D09 File Offset: 0x00000F09
        public float z { get; set; }

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x06000172 RID: 370 RVA: 0x00002D12 File Offset: 0x00000F12
        // (set) Token: 0x06000173 RID: 371 RVA: 0x00002D1A File Offset: 0x00000F1A
        public int VictimSlot { get; set; }

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x06000174 RID: 372 RVA: 0x00002D23 File Offset: 0x00000F23
        // (set) Token: 0x06000175 RID: 373 RVA: 0x00002D2B File Offset: 0x00000F2B
        public int AssistSlot { get; set; }

        // Token: 0x06000176 RID: 374 RVA: 0x000020ED File Offset: 0x000002ED
        public Frag()
        {
        }

        // Token: 0x06000177 RID: 375 RVA: 0x00002D34 File Offset: 0x00000F34
        public Frag(byte hitspotInfo)
        {
            this.SetHitspotInfo(hitspotInfo);
        }

        // Token: 0x06000178 RID: 376 RVA: 0x00002D43 File Offset: 0x00000F43
        public void SetHitspotInfo(byte value)
        {
            this.hitspotInfo = value;
            this.VictimSlot = (int)(value & 15);
        }
    }
}
