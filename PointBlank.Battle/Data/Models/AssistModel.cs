using System;

namespace PointBlank.Battle.Data.Models
{
    // Token: 0x02000036 RID: 54
    public class AssistModel
    {
        // Token: 0x1700000B RID: 11
        // (get) Token: 0x0600013B RID: 315 RVA: 0x000024E0 File Offset: 0x000006E0
        // (set) Token: 0x0600013C RID: 316 RVA: 0x000024E8 File Offset: 0x000006E8
        public int Killer { get; set; }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x0600013D RID: 317 RVA: 0x000024F1 File Offset: 0x000006F1
        // (set) Token: 0x0600013E RID: 318 RVA: 0x000024F9 File Offset: 0x000006F9
        public int Victim { get; set; }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x0600013F RID: 319 RVA: 0x00002502 File Offset: 0x00000702
        // (set) Token: 0x06000140 RID: 320 RVA: 0x0000250A File Offset: 0x0000070A
        public int Damage { get; set; }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000141 RID: 321 RVA: 0x00002513 File Offset: 0x00000713
        // (set) Token: 0x06000142 RID: 322 RVA: 0x0000251B File Offset: 0x0000071B
        public int RoomId { get; set; }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000143 RID: 323 RVA: 0x00002524 File Offset: 0x00000724
        // (set) Token: 0x06000144 RID: 324 RVA: 0x0000252C File Offset: 0x0000072C
        public bool IsAssist { get; set; }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000145 RID: 325 RVA: 0x00002535 File Offset: 0x00000735
        // (set) Token: 0x06000146 RID: 326 RVA: 0x0000253D File Offset: 0x0000073D
        public bool IsKiller { get; set; }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x06000147 RID: 327 RVA: 0x00002546 File Offset: 0x00000746
        // (set) Token: 0x06000148 RID: 328 RVA: 0x0000254E File Offset: 0x0000074E
        public bool VictimDead { get; set; }
    }
}
