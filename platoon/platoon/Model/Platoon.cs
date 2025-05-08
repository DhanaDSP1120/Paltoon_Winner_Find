using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace platoon.Model
{
    public class Platoon
    { 
        public int id { get; set; }
        public string type { get; set; }
        public int memCount { get; set; }

        public string actualname { get; set; }

        public static int objCounter = 1;

        public Platoon(string platoon)
        {
            var str = platoon.Split("#");
            id=objCounter++;
            actualname = platoon;
            type = str[0];
            memCount = int.Parse(str[1]);
        }

        public static List<Platoon> CovertStringToPlatoon(string platoonInput) {
            return platoonInput.Split(";").Select(x=>new Platoon(x)).ToList();
        }
    }

    public class WinningPlatoonCombination
    {
        public int oppbaseObjId { get; set; }
        public List<int> platoonIdsList { get; set; }
    }
}
