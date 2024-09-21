// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Network.ServerPacket.PROTOCOL_BASE_GET_USER_INFO_ACK
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using PointBlank.Auth.Data.Managers;
using PointBlank.Core.Managers;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Managers.Server;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using System;

namespace PointBlank.Auth.Network.ServerPacket
{
  public class PROTOCOL_BASE_GET_USER_INFO_ACK : SendPacket
  {
    private PointBlank.Auth.Data.Model.Account Player;
    private PointBlank.Core.Models.Account.Clan.Clan Clan;
    private uint Error;
    private byte[] Flag = new byte[4];
        public PROTOCOL_BASE_GET_USER_INFO_ACK(PointBlank.Auth.Data.Model.Account Player)
    {
      this.Player = Player;
      if (Player != null)
        this.Clan = ClanManager.getClanDB((object) Player.clan_id, 1);
      else
        this.Error = 2147483648U;
    }

    private void CheckGameEvents(EventVisitModel evVisit)
    {
      int num = int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
      PlayerEvent pE = this.Player._event;
      if (pE != null)
      {
        QuestModel runningEvent1 = EventQuestSyncer.getRunningEvent();
        if (runningEvent1 != null)
        {
          long lastQuestDate = (long) pE.LastQuestDate;
          long lastQuestFinish = (long) pE.LastQuestFinish;
          if (pE.LastQuestDate < runningEvent1.startDate)
          {
            pE.LastQuestDate = 0U;
            pE.LastQuestFinish = 0;
          }
          if (pE.LastQuestFinish == 0)
          {
            this.Player._mission.mission4 = 13;
            if (pE.LastQuestDate == 0U)
              pE.LastQuestDate = (uint) num;
          }
          if ((long) pE.LastQuestDate != lastQuestDate || (long) pE.LastQuestFinish != lastQuestFinish)
            EventQuestSyncer.ResetPlayerEvent(this.Player.player_id, pE);
        }
        EventLoginModel runningEvent2 = EventLoginSyncer.getRunningEvent();
        if (runningEvent2 != null && (runningEvent2.startDate >= pE.LastLoginDate || pE.LastLoginDate >= runningEvent2.endDate))
        {
          PlayerManager.tryCreateItem(new ItemsModel(runningEvent2._rewardId, runningEvent2._category, "Login Event", 1, runningEvent2._count), this.Player._inventory, this.Player.player_id);
          ComDiv.updateDB("player_events", "last_login_date", (object) num, "player_id", (object) this.Player.player_id);
        }
        if (evVisit != null && pE.LastVisitEventId != evVisit.id)
        {
          pE.LastVisitEventId = evVisit.id;
          pE.LastVisitSequence1 = 0;
          pE.LastVisitSequence2 = 0;
          pE.NextVisitDate = 0;
          EventVisitSyncer.ResetPlayerEvent(this.Player.player_id, evVisit.id);
        }
      }
      ComDiv.updateDB("players", "last_login", (object) num, "player_id", (object) this.Player.player_id);
    }

    public override void write()
    {
      ServerConfig config = AuthManager.Config;
      EventVisitModel runningEvent = EventVisitSyncer.getRunningEvent();
      PlayerEvent playerEvent = this.Player._event;
      bool flag = runningEvent != null && (playerEvent.LastVisitSequence1 < runningEvent.checks && playerEvent.NextVisitDate <= int.Parse(DateTime.Now.ToString("yyMMdd")) || playerEvent.LastVisitSequence2 < runningEvent.checks && playerEvent.LastVisitSequence2 != playerEvent.LastVisitSequence1);
      this.writeH((short) 525);
      this.writeH((short) 0);
      this.writeD(this.Error);
      if (this.Error != 0U)
        return;
      this.writeC((byte) this.Player.Characters.Count);
      this.writeH((short) 210);
      this.writeC((byte) this.Player.Quickstarts.Count);
      for (int index = 0; index < this.Player.Quickstarts.Count; ++index)
      {
        QuickStart quickstart = this.Player.Quickstarts[index];
        this.writeC((byte) quickstart.MapId);
        this.writeC((byte) quickstart.Rule);
        this.writeC((byte) quickstart.StageOptions);
        this.writeC((byte) quickstart.Type);
      }
      this.writeB(new byte[33]);
      this.writeC((byte) 4);
      this.writeD(0);
      this.writeD(0);
      this.writeD(0);
      this.writeD(0);
      this.writeD(0);
      this.writeD(this.Player._titles.Slots);
      this.writeC((byte) 3);
      this.writeC((byte) this.Player._titles.Equiped1);
      this.writeC((byte) this.Player._titles.Equiped2);
      this.writeC((byte) this.Player._titles.Equiped3);
      this.writeQ(this.Player._titles.Flags);
      this.writeC((byte) 160);
      this.writeB(this.Player._mission.list1);
      this.writeB(this.Player._mission.list2);
      this.writeB(this.Player._mission.list3);
      this.writeB(this.Player._mission.list4);
      this.writeC((byte) this.Player._mission.actualMission);
      this.writeC((byte) this.Player._mission.card1);
      this.writeC((byte) this.Player._mission.card2);
      this.writeC((byte) this.Player._mission.card3);
      this.writeC((byte) this.Player._mission.card4);
      this.writeB(ComDiv.getCardFlags(this.Player._mission.mission1, this.Player._mission.list1));
      this.writeB(ComDiv.getCardFlags(this.Player._mission.mission2, this.Player._mission.list2));
      this.writeB(ComDiv.getCardFlags(this.Player._mission.mission3, this.Player._mission.list3));
      this.writeB(ComDiv.getCardFlags(this.Player._mission.mission4, this.Player._mission.list4));
      this.writeC((byte) this.Player._mission.mission1);
      this.writeC((byte) this.Player._mission.mission2);
      this.writeC((byte) this.Player._mission.mission3);
      this.writeC((byte) this.Player._mission.mission4);
      this.writeD(this.Player.blue_order);
      this.writeD(this.Player.medal);
      this.writeD(this.Player.insignia);
      this.writeD(this.Player.brooch);
      this.writeD(0);
      this.writeC((byte) 0);
      this.writeD(0);
      this.writeC((byte) 2);
      this.writeB(new byte[375]);
      if (runningEvent != null && (playerEvent.LastVisitSequence1 < runningEvent.checks && playerEvent.NextVisitDate <= int.Parse(DateTime.Now.ToString("yyMMdd")) || playerEvent.LastVisitSequence2 < runningEvent.checks && playerEvent.LastVisitSequence2 != playerEvent.LastVisitSequence1))
      {
        this.writeUnicode(runningEvent.title, 70);
        this.writeC((byte) 0);
        this.writeC((byte) runningEvent.checks);
        this.writeD(1);
        this.writeD(runningEvent.startDate);
        this.writeD(runningEvent.endDate);
        this.writeB(new byte[3]);
        for (int index = 0; index < 32; ++index)
        {
          if (index > 0 && runningEvent.box.Count >= index)
          {
            VisitBox visitBox = runningEvent.box[index - 1];
            this.writeC((byte) visitBox.RewardCount);
            this.writeD(visitBox.reward1.good_id);
            this.writeD(visitBox.reward2.good_id);
          }
          else
          {
            this.writeC((byte) 0);
            this.writeD(0);
            this.writeD(0);
          }
        }
      }
      else
        this.writeB(new byte[375]);
      this.writeC((byte) 2);
      this.writeD(0);
      this.writeC((byte) 0);
      this.writeD(0);
      this.writeD(flag ? 1 : 0);
      this.writeC((byte) playerEvent.LastVisitSequence1);
      this.writeC((byte) 0);
      this.writeC(flag ? (byte) 1 : (byte) 0);
      this.writeC((byte) 0);
      this.writeC((byte) 0);
      this.writeIP("127.0.0.1");
      this.writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
      this.writeC(this.Player.Characters.Count == 0 ? (byte) 0 : (byte) this.Player.getCharacter(this.Player._equip._red).Slot);
      this.writeC(this.Player.Characters.Count == 0 ? (byte) 1 : (byte) this.Player.getCharacter(this.Player._equip._blue).Slot);
      this.writeD(this.Player._inventory.getItem(this.Player._equip._dino)._id);
      this.writeD((uint) this.Player._inventory.getItem(this.Player._equip._dino)._objId);
      if (Player.effects.HasFlag(CouponEffects.Ammo40) ||
                             Player.effects.HasFlag(CouponEffects.Ammo10) ||
                             Player.effects.HasFlag(CouponEffects.GetDroppedWeapon) ||
                             Player.effects.HasFlag(CouponEffects.QuickChangeWeapon) ||
                             Player.effects.HasFlag(CouponEffects.QuickChangeReload))
            {
                if (Player.effects.HasFlag(CouponEffects.Ammo40))
                {
                    Flag[0] += (byte)EffectFlag.Ammo40;
                }
                if (Player.effects.HasFlag(CouponEffects.Ammo10))
                {
                    Flag[0] += (byte)EffectFlag.Ammo10;
                }
                if (Player.effects.HasFlag(CouponEffects.GetDroppedWeapon))
                {
                    Flag[0] += (byte)EffectFlag.GetDroppedWeapon;
                }
                if (Player.effects.HasFlag(CouponEffects.QuickChangeWeapon))
                {
                    Flag[0] += (byte)EffectFlag.QuickChangeWeapon;
                }
                if (Player.effects.HasFlag(CouponEffects.QuickChangeReload))
                {
                    Flag[0] += (byte)EffectFlag.QuickChangeReload;
                }
            }
            if (Player.effects.HasFlag(CouponEffects.Invincible) ||
                Player.effects.HasFlag(CouponEffects.FullMetalJack) ||
                Player.effects.HasFlag(CouponEffects.HollowPoint) ||
                Player.effects.HasFlag(CouponEffects.HollowPointPlus) ||
                Player.effects.HasFlag(CouponEffects.C4SpeedKit))
            {
                if (Player.effects.HasFlag(CouponEffects.Invincible))
                {
                    Flag[1] += (byte)EffectFlag.Invincible;
                }
                if (Player.effects.HasFlag(CouponEffects.FullMetalJack))
                {
                    Flag[1] += (byte)EffectFlag.FullMetalJack;
                }
                if (Player.effects.HasFlag(CouponEffects.HollowPoint))
                {
                    Flag[1] += (byte)EffectFlag.HollowPoint;
                }
                if (Player.effects.HasFlag(CouponEffects.HollowPointPlus))
                {
                    Flag[1] += (byte)EffectFlag.HollowPointPlus;
                }
                if (Player.effects.HasFlag(CouponEffects.C4SpeedKit))
                {
                    Flag[1] += (byte)EffectFlag.C4SpeedKit;
                }
            }
            if (Player.effects.HasFlag(CouponEffects.ExtraGrenade) ||
                Player.effects.HasFlag(CouponEffects.ExtraThrowGrenade) ||
                Player.effects.HasFlag(CouponEffects.JackHollowPoint) ||
                Player.effects.HasFlag(CouponEffects.HP5) ||
                Player.effects.HasFlag(CouponEffects.HP10) ||
                Player.effects.HasFlag(CouponEffects.Defense5) ||
                Player.effects.HasFlag(CouponEffects.Defense10) ||
                Player.effects.HasFlag(CouponEffects.Defense20))
            {
                if (Player.effects.HasFlag(CouponEffects.ExtraGrenade))
                {
                    Flag[2] += (byte)EffectFlag.ExtraGrenade;
                }
                if (Player.effects.HasFlag(CouponEffects.ExtraThrowGrenade))
                {
                    Flag[2] += (byte)EffectFlag.ExtraThrowGrenade;
                }
                if (Player.effects.HasFlag(CouponEffects.JackHollowPoint))
                {
                    Flag[2] += (byte)EffectFlag.JackHollowPoint;
                }
                if (Player.effects.HasFlag(CouponEffects.HP5))
                {
                    Flag[2] += (byte)EffectFlag.HP5;
                }
                if (Player.effects.HasFlag(CouponEffects.HP10))
                {
                    Flag[2] += (byte)EffectFlag.HP10;
                }
                if (Player.effects.HasFlag(CouponEffects.Defense5))
                {
                    Flag[2] += (byte)EffectFlag.Defense5;
                }
                if (Player.effects.HasFlag(CouponEffects.Defense10))
                {
                    Flag[2] += (byte)EffectFlag.Defense10;
                }
                if (Player.effects.HasFlag(CouponEffects.Defense20))
                {
                    Flag[2] += (byte)EffectFlag.Defense20;
                }
            }
            if (Player.effects.HasFlag(CouponEffects.Defense90) ||
                Player.effects.HasFlag(CouponEffects.Respawn20) ||
                Player.effects.HasFlag(CouponEffects.Respawn30) ||
                Player.effects.HasFlag(CouponEffects.Respawn50) ||
                Player.effects.HasFlag(CouponEffects.Respawn100))
            {
                if (Player.effects.HasFlag(CouponEffects.Defense90))
                {
                    Flag[3] += (byte)EffectFlag.Defense90;
                }
                if (Player.effects.HasFlag(CouponEffects.Respawn20))
                {
                    Flag[3] += (byte)EffectFlag.Respawn20;
                }
                if (Player.effects.HasFlag(CouponEffects.Respawn30))
                {
                    Flag[3] += (byte)EffectFlag.Respawn30;
                }
                if (Player.effects.HasFlag(CouponEffects.Respawn50))
                {
                    Flag[3] += (byte)EffectFlag.Respawn50;
                }
                if (Player.effects.HasFlag(CouponEffects.Respawn100))
                {
                    Flag[3] += (byte)EffectFlag.Respawn100;
                }
            }
            writeC(Flag[0]);
            writeC(Flag[1]);
            writeC(Flag[2]);
            writeC(Flag[3]);
            writeD(0);
            writeD(0);
            writeC(0);
            writeC(0);
            writeC(0);
            writeC(0);
            writeC(0);
            writeC(0);
            writeC(0);
            writeC(0);
            writeC(0);
            writeC(0);

      this.writeC((byte) this.Player.name_color);
      this.writeD(this.Player._bonus.fakeRank);
      this.writeD(this.Player._bonus.fakeRank);
      this.writeUnicode(this.Player._bonus.fakeNick, 66);
      this.writeH((short) this.Player._bonus.sightColor);
      this.writeH((short) this.Player._bonus.muzzle);
      this.writeD(this.Player._statistic.fights);
      this.writeD(this.Player._statistic.fights_win);
      this.writeD(this.Player._statistic.fights_lost);
      this.writeD(this.Player._statistic.fights_draw);
      this.writeD(this.Player._statistic.kills_count);
      this.writeD(this.Player._statistic.headshots_count);
      this.writeD(this.Player._statistic.deaths_count);
      this.writeD(this.Player._statistic.totalfights_count);
      this.writeD(this.Player._statistic.totalkills_count);
      this.writeD(this.Player._statistic.escapes);
      this.writeD(this.Player._statistic.assist);
      this.writeD(this.Player._statistic.fights);
      this.writeD(this.Player._statistic.fights_win);
      this.writeD(this.Player._statistic.fights_lost);
      this.writeD(this.Player._statistic.fights_draw);
      this.writeD(this.Player._statistic.kills_count);
      this.writeD(this.Player._statistic.headshots_count);
      this.writeD(this.Player._statistic.deaths_count);
      this.writeD(this.Player._statistic.totalfights_count);
      this.writeD(this.Player._statistic.totalkills_count);
      this.writeD(this.Player._statistic.escapes);
      this.writeD(this.Player._statistic.assist);
      this.writeUnicode(this.Player.player_name, 66);
      this.writeD(this.Player._rank);
      this.writeD(this.Player._rank);
      this.writeD(this.Player._gp);
      this.writeD(this.Player._exp);
      this.writeD(0);
      this.writeD(0);
      this.writeD(0);
            //this.writeD(this.Player.HavePermission("observer_enabled") ? 111111 : 0);
      if (Player.IsGM() || Player.HaveAcessLevel()) this.writeD(111111); else this.writeD(0);
      this.writeC((byte) 0);
      this.writeC((byte) 0);
      this.writeC((byte) 0);
      this.writeD(this.Player._money);
      this.writeD(this.Clan._id);
      this.writeD(this.Player.clanAccess);
      this.writeQ(this.Player.Status());
      this.writeC((byte) this.Player.pc_cafe);
      this.writeC((byte) this.Player.tourneyLevel);
      this.writeUnicode(this.Clan._name, 34);
      this.writeC((byte) this.Clan._rank);
      this.writeC((byte) this.Clan.getClanUnit());
      this.writeD(this.Clan._logo);
      this.writeC((byte) this.Clan._name_color);
      this.writeC((byte) this.Clan.effect);
      this.writeC(AuthManager.Config.BloodEnable ? (byte) this.Player.age : (byte) 27);
    }

    private void WriteDormantEvent() => this.writeB(new byte[375]);

    private void WriteVisitEvent(EventVisitModel ev)
    {
      PlayerEvent playerEvent = this.Player._event;
      if (ev != null && (playerEvent.LastVisitSequence1 < ev.checks && playerEvent.NextVisitDate <= int.Parse(DateTime.Now.ToString("yyMMdd")) || playerEvent.LastVisitSequence2 < ev.checks && playerEvent.LastVisitSequence2 != playerEvent.LastVisitSequence1))
      {
        this.writeUnicode(ev.title, 70);
        this.writeC((byte) playerEvent.LastVisitSequence1);
        this.writeC((byte) ev.checks);
        this.writeD(ev.id);
        this.writeD(ev.startDate);
        this.writeD(ev.endDate);
        this.writeB(new byte[12]);
        for (int index = 0; index < 32; ++index)
        {
          VisitBox visitBox = ev.box[index];
          this.writeC((byte) visitBox.RewardCount);
          this.writeD(visitBox.reward1.good_id);
          this.writeD(visitBox.reward2.good_id);
        }
      }
      else
        this.writeB(new byte[375]);
    }
  }
}
