// Decompiled with JetBrains decompiler
// Type: PointBlank.Battle.Data.Models.Player
// Assembly: PointBlank.Battle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D3C6437-0433-43F1-9377-D9705A2C09C8
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Battle.exe

using PointBlank.Battle.Data.Configs;
using PointBlank.Battle.Data.Enums;
using SharpDX;
using System;
using System.Net;

namespace PointBlank.Battle.Data.Models
{
  public class Player
  {
    public int Slot = -1;
    public int Team;
    public int Life = 100;
    public int MaxLife = 100;
    public int PlayerIdByUser = -2;
    public int PlayerIdByServer = -1;
    public int WeaponId;
    public int RespawnByUser = -2;
    public int RespawnByLogic;
    public int RespawnByServer = -1;
    public float PlantDuration;
    public float DefuseDuration;
    public float C4Time;
    public byte Extensions;
    public Half3 Position;
    public IPEndPoint Client;
    public DateTime Date;
    public DateTime LastPing;
    public DateTime C4First;
    public CLASS_TYPE WeaponClass;
    public bool Dead = true;
    public bool NeverRespawn = true;
    public bool Integrity = true;
    public bool Immortal;
    public int Primary = 0;
    public int Secondary = 0;
    public int Knife = 0;
    public int Grenade = 0;
    public int Special = 0;

    public Player(int Slot)
    {
      this.Slot = Slot;
      this.Team = Slot % 2;
    }

    public void LogPlayerPos(Half3 EndBullet)
    {
      Logger.warning("Player Position X: " + this.Position.X.ToString() + " Y: " + this.Position.Y.ToString() + " Z: " + this.Position.Z.ToString());
      Logger.warning("End Bullet Position X: " + EndBullet.X.ToString() + " Y: " + EndBullet.Y.ToString() + " Z: " + EndBullet.Z.ToString());
    }

    public bool CompareIp(IPEndPoint Ip) => this.Client != null && Ip != null && this.Client.Address.Equals((object) Ip.Address) && this.Client.Port == Ip.Port;

    public bool RespawnIsValid() => this.RespawnByServer == this.RespawnByUser;

    public bool RespawnLogicIsValid() => this.RespawnByServer == this.RespawnByLogic;

    public bool AccountIdIsValid() => this.PlayerIdByServer == this.PlayerIdByUser;

    public bool AccountIdIsValid(int Number) => this.PlayerIdByServer == Number;

    public void CheckLifeValue()
    {
      if (this.Life <= this.MaxLife)
        return;
      this.Life = this.MaxLife;
    }

    public void ResetAllInfos()
    {
      this.Client = (IPEndPoint) null;
      this.Date = new DateTime();
      this.PlayerIdByUser = -2;
      this.PlayerIdByServer = -1;
      this.Integrity = true;
      this.ResetBattleInfos();
    }

    public void ResetBattleInfos()
    {
      this.RespawnByServer = -1;
      this.RespawnByUser = -2;
      this.RespawnByLogic = 0;
      this.Immortal = false;
      this.Dead = true;
      this.NeverRespawn = true;
      this.WeaponId = 0;
      this.LastPing = new DateTime();
      this.C4First = new DateTime();
      this.C4Time = 0.0f;
      this.Position = new Half3();
      this.Life = 100;
      this.MaxLife = 100;
      this.PlantDuration = BattleConfig.plantDuration;
      this.DefuseDuration = BattleConfig.defuseDuration;
    }

    public void ResetLife() => this.Life = this.MaxLife;
  }
}
