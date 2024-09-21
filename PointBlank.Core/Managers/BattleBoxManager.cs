using Newtonsoft.Json;
using PointBlank.Core.Models.Account.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Core.Managers
{
    public static class BattleBoxManager
    {
        private static List<BattleBoxInfo> AllItens = new List<BattleBoxInfo>();

        public static void Load()
        {
            try
            {
                List<BattleBoxInfo> itens = new List<BattleBoxInfo>();

                DirectoryInfo directoryInfo = new DirectoryInfo($"{Environment.CurrentDirectory}\\Data\\BattleBox");
                if (!directoryInfo.Exists)
                    return;
                FileInfo[] files = directoryInfo.GetFiles("*.json");
                for (int i = 0; i < files.Length; i++)
                {
                    try
                    {
                        BattleBoxInfo info = JsonConvert.DeserializeObject<BattleBoxInfo>(files[i].OpenText().ReadToEnd());
                        info.ID = int.Parse(files[i].Name.Substring(0, 7));
                        itens.Add(info);
                    }
                    catch (Exception ex)
                    {
                        Logger.error($"[BattleBoxManager.Load] Ocorreu um erro na programação {files[i].Name}\n{ex.Message}");
                    }
                }

                lock (AllItens)
                    AllItens = itens;
                Logger.warning($"[BattleBoxManager.Load] {AllItens.Count} carregados.");

            }
            catch (Exception ex)
            {
                Logger.error("[BattleBoxManager.Load]:" + ex.ToString());
            }
        }

        public static BattleBoxInfo Get(int id) => id <= 2800000 ? null : AllItens.SingleOrDefault(x => x.ID == id);

        public static Tuple<int, short>[] GetListACK() => AllItens.Any() ? AllItens.Select(x => new Tuple<int, short>(x.ID, x.Price)).ToArray() : Array.Empty<Tuple<int, short>>();

        public static BattleBoxInfo Sort()
        {
            BattleBoxInfo[] list = AllItens.Where(x => x.Enable).ToArray();

            return list.Any() ? list[new Random().Next(0, list.Length)] : null;
        }
    }

    public class BattleBoxInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public short Price { get; set; }
        public bool Enable { get; set; }
        public List<BattleBoxItem> Itens { get; set; }

        public BattleBoxItem GetSortedItem(int percent)
        {
            BattleBoxItem[] itens = Itens.Where(x => x.Percent >= percent).ToArray();
            return itens.Any() ? itens[new Random().Next(0, itens.Length)] : null;
        }
        public ItemsModel ToItemModel()
            => new ItemsModel(ID)
            {
                _name = Name,
                _equip = 1,
                _count = 1
            };
    }

    public class BattleBoxItem
    {
        public int Item { get; set; }
        public string ItemName { get; set; }
        public int Count { get; set; }
        public int Equip { get; set; }
        public int Percent { get; set; }
        public bool Special { get; set; } = false;

        public ItemsModel ToItemModel()
            => new ItemsModel(Item)
            {
                _name = ItemName,
                _equip = Equip,
                _count = Count
            };
    }
}
