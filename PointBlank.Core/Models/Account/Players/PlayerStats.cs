using System;

namespace PointBlank.Core.Models.Account.Players
{
    // Token: 0x02000075 RID: 117
    public class PlayerStats
    {
        // Token: 0x170000B3 RID: 179
        // (get) Token: 0x060002E9 RID: 745 RVA: 0x00003CC9 File Offset: 0x00001EC9
        // (set) Token: 0x060002EA RID: 746 RVA: 0x00003CD1 File Offset: 0x00001ED1
        public int fights { get; set; }

        // Token: 0x170000B4 RID: 180
        // (get) Token: 0x060002EB RID: 747 RVA: 0x00003CDA File Offset: 0x00001EDA
        // (set) Token: 0x060002EC RID: 748 RVA: 0x00003CE2 File Offset: 0x00001EE2
        public int fights_win { get; set; }

        // Token: 0x170000B5 RID: 181
        // (get) Token: 0x060002ED RID: 749 RVA: 0x00003CEB File Offset: 0x00001EEB
        // (set) Token: 0x060002EE RID: 750 RVA: 0x00003CF3 File Offset: 0x00001EF3
        public int fights_lost { get; set; }

        // Token: 0x170000B6 RID: 182
        // (get) Token: 0x060002EF RID: 751 RVA: 0x00003CFC File Offset: 0x00001EFC
        // (set) Token: 0x060002F0 RID: 752 RVA: 0x00003D04 File Offset: 0x00001F04
        public int fights_draw { get; set; }

        // Token: 0x170000B7 RID: 183
        // (get) Token: 0x060002F1 RID: 753 RVA: 0x00003D0D File Offset: 0x00001F0D
        // (set) Token: 0x060002F2 RID: 754 RVA: 0x00003D15 File Offset: 0x00001F15
        public int kills_count { get; set; }

        // Token: 0x170000B8 RID: 184
        // (get) Token: 0x060002F3 RID: 755 RVA: 0x00003D1E File Offset: 0x00001F1E
        // (set) Token: 0x060002F4 RID: 756 RVA: 0x00003D26 File Offset: 0x00001F26
        public int totalkills_count { get; set; }

        // Token: 0x170000B9 RID: 185
        // (get) Token: 0x060002F5 RID: 757 RVA: 0x00003D2F File Offset: 0x00001F2F
        // (set) Token: 0x060002F6 RID: 758 RVA: 0x00003D37 File Offset: 0x00001F37
        public int totalfights_count { get; set; }

        // Token: 0x170000BA RID: 186
        // (get) Token: 0x060002F7 RID: 759 RVA: 0x00003D40 File Offset: 0x00001F40
        // (set) Token: 0x060002F8 RID: 760 RVA: 0x00003D48 File Offset: 0x00001F48
        public int deaths_count { get; set; }

        // Token: 0x170000BB RID: 187
        // (get) Token: 0x060002F9 RID: 761 RVA: 0x00003D51 File Offset: 0x00001F51
        // (set) Token: 0x060002FA RID: 762 RVA: 0x00003D59 File Offset: 0x00001F59
        public int escapes { get; set; }

        // Token: 0x170000BC RID: 188
        // (get) Token: 0x060002FB RID: 763 RVA: 0x00003D62 File Offset: 0x00001F62
        // (set) Token: 0x060002FC RID: 764 RVA: 0x00003D6A File Offset: 0x00001F6A
        public int headshots_count { get; set; }

        // Token: 0x170000BD RID: 189
        // (get) Token: 0x060002FD RID: 765 RVA: 0x00003D73 File Offset: 0x00001F73
        // (set) Token: 0x060002FE RID: 766 RVA: 0x00003D7B File Offset: 0x00001F7B
        public int assist { get; set; }

        // Token: 0x170000BE RID: 190
        // (get) Token: 0x060002FF RID: 767 RVA: 0x00003D84 File Offset: 0x00001F84
        // (set) Token: 0x06000300 RID: 768 RVA: 0x00003D8C File Offset: 0x00001F8C
        public int ClanGames { get; set; }

        // Token: 0x170000BF RID: 191
        // (get) Token: 0x06000301 RID: 769 RVA: 0x00003D95 File Offset: 0x00001F95
        // (set) Token: 0x06000302 RID: 770 RVA: 0x00003D9D File Offset: 0x00001F9D
        public int ClanWins { get; set; }

        // Token: 0x170000C0 RID: 192
        // (get) Token: 0x06000303 RID: 771 RVA: 0x00003DA6 File Offset: 0x00001FA6
        // (set) Token: 0x06000304 RID: 772 RVA: 0x00003DAE File Offset: 0x00001FAE
        public int ClanLoses { get; set; }

        // Token: 0x06000305 RID: 773 RVA: 0x00003DB7 File Offset: 0x00001FB7
        public int GetKDRatio()
        {
            if (this.headshots_count <= 0 && this.kills_count <= 0)
            {
                return 0;
            }
            return (int)Math.Floor(((double)(this.kills_count * 100) + 0.5) / (double)(this.kills_count + this.deaths_count));
        }

        // Token: 0x06000306 RID: 774 RVA: 0x00003DF6 File Offset: 0x00001FF6
        public int GetHSRatio()
        {
            if (this.kills_count <= 0)
            {
                return 0;
            }
            return (int)Math.Floor((double)(this.headshots_count * 100) / (double)this.kills_count + 0.5);
        }
    }
}
