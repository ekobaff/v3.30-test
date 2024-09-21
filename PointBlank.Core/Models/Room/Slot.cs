// Decompiled with JetBrains decompiler
// Type: PointBlank.Core.Models.Room.Slot
// Assembly: PointBlank.Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 98ADB923-CC0E-41E2-8CF2-9775427811AE
// Assembly location: C:\Users\Server\Desktop\PointBlank.Core.dll

using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PointBlank.Core.Models.Room
{
  public class Slot
  {
    public SlotState state;
    public ResultIcon bonusFlags;
    public PlayerEquipedItems _equip;
    public long _playerId;
    public DeadEnum _deathState = DeadEnum.Alive;
    public bool firstRespawn = true;
    public bool repeatLastState;
    public bool check;
    public bool espectador;
    public bool specGM;
    public bool withHost;
    public int _id;
    public int Flag;
    public int _team;
    public int aiLevel;
    public int latency;
    public int failLatencyTimes;
    public int ping = 5;
    public int passSequence;
    public int isPlaying;
    public int earnedXP;
    public int spawnsCount;
    public int headshots;
    public int lastKillState;
    public int killsOnLife;
    public int exp;
    public int money;
    public int gp;
    public int Score;
    public int allKills;
    public int allDeaths;
    public int allAssists;
    public int objects;
    public int BonusItemExp;
    public int BonusItemPoint;
    public int BonusEventExp;
    public int BonusEventPoint;
    public int BonusCafePoint;
    public int BonusCafeExp;
    public int unkItem;
    public int Costume = 0;
    public DateTime NextVoteDate;
    public DateTime startTime;
    public DateTime preStartDate;
    public DateTime preLoadDate;
    public ushort damageBar1;
    public ushort damageBar2;
    public List<int> armas_usadas = new List<int>();
    public bool MissionsCompleted;
    public PlayerMissions Missions;
    public TimerState timing = new TimerState();

    public void StopTiming()
    {
      if (this.timing == null)
        return;
      this.timing.Timer = (Timer) null;
    }

    public Slot(int slotIdx) => this.SetSlotId(slotIdx);

    public void SetSlotId(int slotIdx)
    {
      this._id = slotIdx;
      this._team = slotIdx % 2;
      this.Flag = 1 << slotIdx;
    }

    public void ResetSlot()
    {
      this.repeatLastState = false;
      this._deathState = DeadEnum.Alive;
      this.StopTiming();
      this.check = false;
      this.espectador = false;
      this.specGM = false;
      this.withHost = false;
      this.firstRespawn = true;
      this.failLatencyTimes = 0;
      this.latency = 0;
      this.ping = 0;
      this.passSequence = 0;
      this.allDeaths = 0;
      this.allKills = 0;
      this.allAssists = 0;
      this.bonusFlags = ResultIcon.None;
      this.killsOnLife = 0;
      this.lastKillState = 0;
      this.Score = 0;
      this.gp = 0;
      this.exp = 0;
      this.headshots = 0;
      this.objects = 0;
      this.BonusItemExp = 0;
      this.BonusItemPoint = 0;
      this.BonusCafeExp = 0;
      this.BonusCafePoint = 0;
      this.BonusEventExp = 0;
      this.BonusEventPoint = 0;
      this.spawnsCount = 0;
      this.damageBar1 = (ushort) 0;
      this.damageBar2 = (ushort) 0;
      this.earnedXP = 0;
      this.isPlaying = 0;
      this.money = 0;
      this.NextVoteDate = new DateTime();
      this.aiLevel = 0;
      this.armas_usadas.Clear();
      this.MissionsCompleted = false;
      this.Missions = (PlayerMissions) null;
    }

    public void SetMissionsClone(PlayerMissions missions)
    {
      this.Missions = missions.DeepCopy();
      this.MissionsCompleted = false;
    }

    public double inBattleTime(DateTime date) => this.startTime == new DateTime() ? 0.0 : (date - this.startTime).TotalSeconds;
  }
}
