using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace PointBlank.Core.Progress
{
    public class Functions
    {

        public static string NameVip(int type)
        {


            switch (type)
            {
                case 1: return "Basic";
                case 2: return "Plus";
                case 3: return "Master";
                case 4: return "Combat";
                case 5: return "Extreme";
                case 6: return "Booster";
                default: return "Error";
            }

           
        }
        public static string NameRanked(int rank)
        {
            lock (new object())
            {
                switch (rank)
                {
                    case 1:
                        return "Ferro";
                    case 2:
                        return "Ferro I";
                    case 3:
                        return "Ferro II";
                    case 4:
                        return "Ferro III";
                    case 5:
                        return "Cobre I";
                    case 6:
                        return "Cobre II";
                    case 7:
                        return "Cobre III";
                    case 8:
                        return "Cobre IV";
                    case 9:
                        return "Cobre V";
                    case 10:
                        return "Cobre VI";
                    case 11:
                        return "Bronze I";
                    case 12:
                        return "Bronze II";
                    case 13:
                        return "Bronze III";
                    case 14:
                        return "Prata I";
                    case 15:
                        return "Prata II";
                    case 16:
                        return "Prata III";
                    case 17:
                        return "Ouro I";
                    case 18:
                        return "Ouro II";
                    case 19:
                        return "Ouro III";
                    case 20:
                        return "Mestre";
                    default:
                        return null;
                }
            }
        }   
            public static string NameRank(int rank)
        {

            lock (new object())
            {
                switch (rank)
                {
                    case 0:
                        return "Novato";

                    case 1:
                        return "Recruta";

                    case 2:
                        return "Soldado";

                    case 3:
                        return "Cabo";

                    case 4:
                        return "Sargento";

                    case 5:
                        return "Terceiro Sargento 1";

                    case 6:
                        return "Terceiro Sargento 2";

                    case 7:
                        return "Terceiro Sargento 3";

                    case 8:
                        return "Segundo Sargento 1";

                    case 9:
                        return "Segundo Sargento 2";

                    case 10:
                        return "Segundo Sargento 3";

                    case 11:
                        return "Segundo Sargento 4";

                    case 12:
                        return "Primeiro Sargento 1";

                    case 13:
                        return "Primeiro Sargento 2";

                    case 14:
                        return "Primeiro Sargento 3";

                    case 15:
                        return "Primeiro Sargento 4";

                    case 16:
                        return "Primeiro Sargento 5";

                    case 17:
                        return "Segundo Tenente 1";

                    case 18:
                        return "Segundo Tenente 2";

                    case 19:
                        return "Segundo Tenente 3";

                    case 20:
                        return "Segundo Tenente 4";

                    case 21:
                        return "Primeiro Tenente 1";

                    case 22:
                        return "Primeiro Tenente 2";

                    case 23:
                        return "Primeiro Tenente 3";

                    case 24:
                        return "Primeiro Tenente 4";

                    case 25:
                        return "Primeiro Tenente 5";

                    case 26:
                        return "Capitão 1";

                    case 27:
                        return "Capitão 2";

                    case 28:
                        return "Capitão 3";

                    case 29:
                        return "Capitão 4";

                    case 30:
                        return "Capitão 5";

                    case 31:
                        return "Major 1";

                    case 32:
                        return "Major 2";

                    case 33:
                        return "Major 3";

                    case 34:
                        return "Major 4";

                    case 35:
                        return "Major 5";

                    case 36:
                        return "Tenente Coronel 1";

                    case 37:
                        return "Tenente Coronel 2";

                    case 38:
                        return "Tenente Coronel 3";

                    case 39:
                        return "Tenente Coronel 4";

                    case 40:
                        return "Tenente Coronel 5";

                    case 41:
                        return "Coronel 1";

                    case 42:
                        return "Coronel 2";

                    case 43:
                        return "Coronel 3";

                    case 44:
                        return "Coronel 4";

                    case 45:
                        return "Coronel 5";

                    case 46:
                        return "General de Brigada";

                    case 47:
                        return "General de Divisão";

                    case 48:
                        return "General de Exército";

                    case 49:
                        return "Marechal";

                    case 50:
                        return "Herói de Guerra";

                    case 51:
                        return "Hero";

                    case 52:
                        return "Bomb";

                    case 53:
                        return "Game Master";

                    case 54:
                        return "Moderador";
                    
                    default:
                        return null;
                }

            }

        }



    }
}
