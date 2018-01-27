using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EStatics
{
    public enum EProjectile {
        Mole = 0,
        Coakroach,
        None
    };

    public static class StringToEProjectile {
        public static Dictionary<string, EProjectile> Get = new Dictionary<string, EProjectile>() {
        { "Mole", EProjectile.Mole },
        { "Coakroach", EProjectile.Coakroach }
    };
    }

    public enum ETag {
        Mole = 0,
        Coakroach = 1,
    }

    public enum ELayer {
        Projectile = 8,
        BankPart = 9
    }
}
