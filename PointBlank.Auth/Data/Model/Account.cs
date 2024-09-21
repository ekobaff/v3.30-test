// Decompiled with JetBrains decompiler
// Type: PointBlank.Auth.Data.Model.Account
// Assembly: PointBlank.Auth, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F2D71E3-3E87-4155-AA64-E654DA3CFF2D
// Assembly location: C:\Users\LucasRoot\Desktop\Servidor BG\PointBlank.Auth.exe

using Npgsql;
using PointBlank.Auth.Data.Configs;
using PointBlank.Auth.Data.Managers;
using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Account.Title;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;

namespace PointBlank.Auth.Data.Model
{
    public class Account
    {
        public bool _myConfigsLoaded;
        public bool _isOnline;
        public bool ICafe;
        public CouponEffects effects;
        public uint LastRankUpDate;
        public string player_name = "";
        public string password;
        public string login;
        public string token;
        public string hwid;

        public int tourneyLevel;
        public int _exp, _gp, _money, _tag;
        public int clan_id;
        public int clanAccess;
        public int pc_cafe;
        public int _rank;
        public int brooch;
        public int insignia;
        public int medal;
        public int blue_order;
        public int name_color;
        public int access;
        public int age;
        public long player_id;
        public long ban_obj_id;
        public PhysicalAddress MacAddress;
        public PlayerEquipedItems _equip = new PlayerEquipedItems();
        public PlayerInventory _inventory = new PlayerInventory();
        public AuthClient _connection;
        public PlayerBonus _bonus;
        public PlayerMissions _mission = new PlayerMissions();
        public PlayerStats _statistic = new PlayerStats();
        public PlayerConfig _config;
        public PlayerTitles _titles;
        public AccountStatus _status = new AccountStatus();
        public PlayerEvent _event;
        public FriendSystem FriendSystem = new FriendSystem();
        public PlayerDailyRecord Daily = new PlayerDailyRecord();
        public List<Account> _clanPlayers = new List<Account>();
        public List<Character> Characters = new List<Character>();
        public List<QuickStart> Quickstarts = new List<QuickStart>();
        public List<PlayerItemTopup> _topups = new List<PlayerItemTopup>();
        public List<PlayerMakeVip> _makevip = new List<PlayerMakeVip>();
        public PlayerRanked Ranked = new PlayerRanked();

        public void SimpleClear()
        {
            _config = null;
            _titles = null;
            _bonus = null;
            _event = null;
            _connection = null;
            _inventory = new PlayerInventory();
            Quickstarts = new List<QuickStart>();
            Characters = new List<Character>();
            FriendSystem = new FriendSystem();
            _clanPlayers = new List<Account>();
            _equip = new PlayerEquipedItems();
            _mission = new PlayerMissions();
            _status = new AccountStatus();
            Daily = new PlayerDailyRecord();
            Ranked = new PlayerRanked();
        }

        public void setOnlineStatus(bool online)
        {
            if (_isOnline == online || !ComDiv.updateDB("players", nameof(online), online, "player_id", player_id))
                return;
            _isOnline = online;
        }

        public void updateCacheInfo()
        {
            if (player_id == 0L)
                return;
            lock (AccountManager.getInstance()._accounts)
                AccountManager.getInstance()._accounts[player_id] = this;
        }

        public void Close(int time)
        {
            if (_connection == null)
                return;
            _connection.Close(time, true);
        }

        public void SendPacket(SendPacket sp)
        {
            if (_connection == null)
                return;
            _connection.SendPacket(sp);
        }

        public void SendPacket(byte[] data)
        {
            if (_connection == null)
                return;
            _connection.SendPacket(data);
        }

        public void SendCompletePacket(byte[] data)
        {
            if (_connection == null)
                return;
            _connection.SendCompletePacket(data);
        }

        public void LoadInventory()
        {
            lock (_inventory._items)
                _inventory._items.AddRange(PlayerManager.getInventoryItems(player_id));
        }

        public void LoadMissionList()
        {
            PlayerMissions mission = MissionManager.getInstance().getMission(player_id, _mission.mission1, _mission.mission2, _mission.mission3, _mission.mission4);
            if (mission == null)
                MissionManager.getInstance().addMissionDB(player_id);
            else
                _mission = mission;
        }

        public void LoadQuickstarts()
        {
            try
            {
                using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = npgsqlConnection.CreateCommand();
                    npgsqlConnection.Open();
                    command.Parameters.AddWithValue("@owner", player_id);
                    command.CommandText = "SELECT * FROM player_quickstart WHERE owner_id=@owner;";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
                    while (npgsqlDataReader.Read())
                    {
                        Quickstarts.Add(new QuickStart()
                        {
                            MapId = npgsqlDataReader.GetInt32(1),
                            Rule = npgsqlDataReader.GetInt32(2),
                            StageOptions = npgsqlDataReader.GetInt32(3),
                            Type = npgsqlDataReader.GetInt32(4)
                        });
                        Quickstarts.Add(new QuickStart()
                        {
                            MapId = npgsqlDataReader.GetInt32(5),
                            Rule = npgsqlDataReader.GetInt32(6),
                            StageOptions = npgsqlDataReader.GetInt32(7),
                            Type = npgsqlDataReader.GetInt32(8)
                        });
                        Quickstarts.Add(new QuickStart()
                        {
                            MapId = npgsqlDataReader.GetInt32(9),
                            Rule = npgsqlDataReader.GetInt32(10),
                            StageOptions = npgsqlDataReader.GetInt32(11),
                            Type = npgsqlDataReader.GetInt32(12)
                        });
                    }
                    command.Dispose();
                    npgsqlDataReader.Close();
                    npgsqlConnection.Dispose();
                    npgsqlConnection.Close();
                }
                if (Quickstarts.Count != 0)
                    return;
                using (NpgsqlConnection npgsqlConnection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = npgsqlConnection.CreateCommand();
                    npgsqlConnection.Open();
                    command.Parameters.AddWithValue("@owner", player_id);
                    command.CommandText = "INSERT INTO player_quickstart (owner_id) VALUES (@owner);";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                    command.Dispose();
                    npgsqlConnection.Dispose();
                    npgsqlConnection.Close();
                    Quickstarts.Add(new QuickStart()
                    {
                        MapId = 0,
                        Rule = 0,
                        StageOptions = 0,
                        Type = 0
                    });
                    Quickstarts.Add(new QuickStart()
                    {
                        MapId = 0,
                        Rule = 0,
                        StageOptions = 0,
                        Type = 0
                    });
                    Quickstarts.Add(new QuickStart()
                    {
                        MapId = 0,
                        Rule = 0,
                        StageOptions = 0,
                        Type = 0
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }

        public long Status() => !string.IsNullOrEmpty(player_name) ? 1L : 0L;

        public void SetPlayerId(long id, int LoadType)
        {
            player_id = id;
            GetAccountInfos(LoadType);
        }

        public void SetPlayerId()
        {
            _titles = new PlayerTitles();
            _bonus = new PlayerBonus();
            GetAccountInfos(8);
        }

        public void GetAccountInfos(int LoadType)
        {
            if (LoadType == 0 || player_id == 0L)
                return;
            if ((LoadType & 1) == 1)
            {
                _titles = TitleManager.getInstance().getTitleDB(player_id);
                _topups = PlayerManager.getPlayerTopups(player_id);
                _makevip = PlayerManager.getPlayerMakeVip(player_id);
                Characters = CharacterManager.getCharacters(player_id);
                Daily = PlayerManager.getPlayerDailyRecord(player_id);
                if (Daily == null)
                    PlayerManager.CreatePlayerDailyRecord(player_id);
                Ranked = PlayerManager.getPlayerRanked(player_id);
                if (Ranked == null)
                    PlayerManager.CreatePlayerRanked(player_id);

            }
            if ((LoadType & 2) == 2)
                _bonus = PlayerManager.getPlayerBonusDB(player_id);
            if ((LoadType & 4) == 4)
            {
                List<Friend> friendList = PlayerManager.getFriendList(player_id);
                if (friendList.Count > 0)
                    FriendSystem._friends = friendList;
            }
            if ((LoadType & 8) == 8)
            {
                _event = PlayerManager.getPlayerEventDB(player_id);
                if (_event == null)
                {
                    PlayerManager.addEventDB(player_id);
                    _event = new PlayerEvent();
                }
            }
            if ((LoadType & 16) != 16)
                return;
            _config = PlayerManager.getConfigDB(player_id);
            if (_config != null)
                return;
            PlayerManager.CreateConfigDB(player_id);
        }

        public Character getCharacter(int ItemId)
        {
            for (int index = 0; index < Characters.Count; ++index)
            {
                Character character = Characters[index];
                if (character.Id == ItemId)
                    return character;
            }
            return null;
        }

        public bool CompareToken(string Token) => AuthConfig.isTestMode || Token == token;

        public bool DiscountPlayerItems()
        {
            try
            {
                bool flag = false;
                int int64 = Convert.ToInt32(DateTime.Now.AddYears(-10).ToString("yyMMddHHmm"));
                List<object> objectList = new List<object>();
                int num1 = _bonus != null ? _bonus.bonuses : 0;
                int num2 = _bonus != null ? _bonus.freepass : 0;
                lock (_inventory._items)
                {
                    for (int index1 = 0; index1 < _inventory._items.Count; ++index1)
                    {
                        ItemsModel itemsModel = _inventory._items[index1];
                        if (itemsModel._count <= int64 & itemsModel._equip == 2)
                        {
                            if (itemsModel._category == 2 && ComDiv.getIdStatics(itemsModel._id, 1) == 6)
                            {
                                Character character1 = getCharacter(itemsModel._id);
                                if (character1 != null)
                                {
                                    int Slot = 0;
                                    for (int index2 = 0; index2 < Characters.Count; ++index2)
                                    {
                                        Character character2 = Characters[index2];
                                        if (character2.Slot != character1.Slot)
                                        {
                                            character2.Slot = Slot;
                                            CharacterManager.Update(Slot, character2.ObjId);
                                            ++Slot;
                                        }
                                    }
                                    if (CharacterManager.Delete(character1.ObjId, player_id))
                                        Characters.Remove(character1);
                                }
                            }
                            else if (itemsModel._category == 3)
                            {
                                if (_bonus != null)
                                {
                                    if (!_bonus.RemoveBonuses(itemsModel._id))
                                    {
                                        if (itemsModel._id == 1600014)
                                        {
                                            ComDiv.updateDB("player_bonus", "sightcolor", 4, "player_id", player_id);
                                            _bonus.sightColor = 4;
                                        }
                                        else if (itemsModel._id == 1600006)
                                        {
                                            ComDiv.updateDB("players", "name_color", 0, "player_id", player_id);
                                            name_color = 0;
                                        }
                                        else if (itemsModel._id == 1600009)
                                        {
                                            ComDiv.updateDB("player_bonus", "fakerank", 55, "player_id", player_id);
                                            _bonus.fakeRank = 55;
                                        }
                                        else if (itemsModel._id == 1600010)
                                        {
                                            if (_bonus.fakeNick.Length > 0)
                                            {
                                                ComDiv.updateDB("player_bonus", "fakenick", "", "player_id", player_id);
                                                ComDiv.updateDB("players", "player_name", _bonus.fakeNick, "player_id", player_id);
                                                player_name = _bonus.fakeNick;
                                                _bonus.fakeNick = "";
                                            }
                                        }
                                        else if (itemsModel._id == 1600187)
                                        {
                                            ComDiv.updateDB("player_bonus", "muzzle", 0, "player_id", player_id);
                                            _bonus.muzzle = 0;
                                        }
                                    }
                                    CouponFlag couponEffect = CouponEffectManager.getCouponEffect(itemsModel._id);
                                    if (couponEffect != null && couponEffect.EffectFlag > 0 && effects.HasFlag(couponEffect.EffectFlag))
                                    {
                                        effects -= couponEffect.EffectFlag;
                                        flag = true;
                                    }
                                }
                                else
                                    continue;
                            }
                            objectList.Add(itemsModel._objId);
                            _inventory._items.RemoveAt(index1--);
                        }
                        else if (itemsModel._count == 0L)
                        {
                            objectList.Add(itemsModel._objId);
                            _inventory._items.RemoveAt(index1--);
                        }
                    }
                    ComDiv.deleteDB("player_items", "object_id", objectList.ToArray(), "owner_id", player_id);
                }
                if (_bonus != null && (_bonus.bonuses != num1 || _bonus.freepass != num2))
                    PlayerManager.updatePlayerBonus(player_id, _bonus.bonuses, _bonus.freepass);
                if (effects < 0)
                    effects = 0;
                if (flag)
                    PlayerManager.updateCupomEffects(player_id, effects);

                _inventory.LoadBasicItems();

                if (pc_cafe == 1)
                {
                    _inventory.LoadVipBasic();
                }
                else if (pc_cafe == 2)
                {
                    _inventory.LoadVipPlus();
                }
                else if (pc_cafe == 3)
                {
                    _inventory.LoadVipMaster();
                }
                else if (pc_cafe == 4)
                {
                    _inventory.LoadVipCombat();
                }
                else if (pc_cafe == 5)
                {
                    _inventory.LoadVipExtreme();
                }
                else if (pc_cafe == 6)
                {
                    _inventory.LoadVipBooster();
                }

                int num3 = PlayerManager.CheckEquipedItems(_equip, _inventory._items, false);
                if (num3 > 0)
                {
                    DBQuery query = new DBQuery();
                    if ((num3 & 2) == 2)
                        PlayerManager.updateWeapons(_equip, query);
                    if ((num3 & 1) == 1)
                        PlayerManager.updateChars(_equip, query);
                    ComDiv.updateDB("players", "player_id", player_id, query.GetTables(), query.GetValues());
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.error("DiscountPlayerItems: " + ex.ToString());
                return false;
            }
        }

  

        public void CheckVIP()
        {
            try
            {
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {


                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@id", player_id);
                        command.CommandText = "SELECT \"End\" FROM player_vip_expire WHERE \"accountID\"=@id";
                        NpgsqlDataReader data = command.ExecuteReader();
                        while (data.Read())
                        {
                            DateTime expirate = data.GetDateTime(0);

                            //Se a data final do VIP for menos que a data atual realiza a remoção dele
                            if (expirate < DateTime.Now)
                            {
                                Logger.warning("Removido o VIP do jogador " + player_id + " [" + login + "].");
                                if (ComDiv.updateDB("players", "pc_cafe", 0, "player_id", player_id))
                                {
                                    pc_cafe = 0;
                                
                                if (ComDiv.updateDB("player_bonus", "fakerank", 55, "player_id", player_id))
                                     _bonus.fakeRank = 55;

                                        //Deleta a conta da tabela vip
                                ComDiv.deleteDB("player_vip_expire", "\"accountID\"", player_id);
                                    Message message = new Message(15)
                                    {
                                        sender_name = "Seu VIP expirou!",
                                        sender_id = 0,
                                        text = "Renove agora para continuar desfrutando de todos os benefícios exclusivos do VIP.\nEntre em contato conosco para obter mais informações sobre como renovar seu VIP.",
                                        state = 1
                                    };
                                    if (MessageManager.CreateMessage(player_id, message))
                                        SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(message));
                                }
                            }
                        }
                        data.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.error("[Account.CheckVIP]: " + ex.Message);
                Console.ResetColor();
            }
        }
    


        public bool IsGM() => _rank == 53 || _rank == 54 || access > 2;

        public bool HaveGMLevel() => access > 2;

        public bool HaveAcessLevel() => access > 0;


    }
}
