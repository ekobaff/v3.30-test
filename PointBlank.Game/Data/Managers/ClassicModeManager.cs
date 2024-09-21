
// Type: Game.data.managers.ClassicModeManager
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe


using Npgsql;
using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Sql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Game.data.managers
{
    public static class ClassicModeManager
    {
        public static List<int> _camp = new List<int>();
        public static List<int> _cnpb = new List<int>();
        public static List<int> _rush = new List<int>();
        public static List<int> _combat = new List<int>();
        public static List<int> _gold = new List<int>();
        public static List<int> _cbp = new List<int>();

        public static void RealodCamp()
        {
            _camp.Clear();
            _cnpb.Clear();
            _rush.Clear();
            _combat.Clear();
            _gold.Clear();
            _cbp.Clear();
            LoadList();
        }
        public static void LoadList()
        {
            try
            {
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "SELECT * FROM tournament_rules";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        string tournament1 = data.GetString(0);
                        string filter = data.GetString(1);
                        if (tournament1 == "camp")
                        { ShopManager.IsBlocked(filter, _camp); }
                        if (tournament1 == "cnpb")
                        { ShopManager.IsBlocked(filter, _cnpb); }
                        if (tournament1 == "rush")
                        { ShopManager.IsBlocked(filter, _rush); }
                        if (tournament1 == "combat")
                        { ShopManager.IsBlocked(filter, _combat); }
                        if (tournament1 == "gold")
                        { ShopManager.IsBlocked(filter, _gold); }
                        if (tournament1 == "cbp")
                        { ShopManager.IsBlocked(filter, _cbp); }

                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                   
                }
                //if (itemscamp.Count > 0)
                //{
                //    Logger.console($" [System] @CAMP: '{itemscamp.Count}'");
                //}


            }
            catch (Exception ex)
            {
                Logger.error("Ocorreu um problema ao carregar os Tournament Rules!\r\n" + ex.ToString());
            }
        }

        public static bool IsBlocked(int listid, int id)
        {
            if (listid == id)
                return true;
            return false;
        }
        public static bool IsBlocked(int listid, int id, ref List<string> list, string category)
        {
            if (listid == id)
            {
                list.Add(category);
                return true;
            }
            return false;
        }

    }
}
