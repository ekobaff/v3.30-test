using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Core.Models.Gift
{
    public class WeaponExp
    {
        public int _Weapon;
        public int _exp;
        public WeaponExp(int weapon, int exp)
        {
            _Weapon = weapon;
            _exp = exp;
        }

    }
}
