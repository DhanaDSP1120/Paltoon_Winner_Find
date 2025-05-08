using Newtonsoft.Json;
using platoon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace platoon.Model
{
    public class BattleArea
    {
        public Platoon ourBase { get; set; }
        public Platoon oppBase { get; set; }

        public int isWeHaveAdvantage { get; set; }
        public int winningStatus { get; set; }
        public int remaningMemCount { get; set; } = 0;

        public static Dictionary<string, List<string>> advantageRule = new Dictionary<string, List<string>>();


        static BattleArea()
        {
            advantageRule.Add("Militia" ,new List<string>() { "Spearmen", "LightCavalry" });
            advantageRule.Add("Spearmen", new List<string>() { "LightCavalry" , "HeavyCavalry" });
            advantageRule.Add("LightCavalry", new List<string>() { "FootArcher", "CavalryArcher" });
            advantageRule.Add("HeavyCavalry", new List<string>() { "Militia", "FootArcher" , "LightCavalry" });
            advantageRule.Add("CavalryArcher", new List<string>() { "Spearmen", "HeavyCavalry" });
            advantageRule.Add("FootArcher", new List<string>() { "Militia", "CavalryArcher" });
        }


        public BattleArea(Platoon our , Platoon opp)
        {
            ourBase = our;
            oppBase = opp;
            isWeHaveAdvantage = checkAdvantage();

            int ourmemCount, oppmemCount;
            if(isWeHaveAdvantage==(int)HaveAdvandage.yes)
            {
                ourmemCount = ourBase.memCount * 2;
                oppmemCount = oppBase.memCount;
            }
            else if(isWeHaveAdvantage==(int)HaveAdvandage.no)
            {
                ourmemCount = ourBase.memCount;
                oppmemCount = oppBase.memCount *2;
            }
            else
            {//No one have power.
                ourmemCount = ourBase.memCount;
                oppmemCount = oppBase.memCount;
            }

            if (ourmemCount == oppmemCount)
            {
                winningStatus = (int)WinningStatus.draw;
            }
            else if (ourmemCount > oppmemCount)
            {
                winningStatus = (int)WinningStatus.yes;
                remaningMemCount = ourmemCount - oppmemCount;
            }
            else
            {
                winningStatus = (int)WinningStatus.no;
            }
        }

        private int checkAdvantage()
        {
            List<string> ourAdavntages = advantageRule[ourBase.type];
            List<string> oppAdvantages = advantageRule[oppBase.type];

            if(ourAdavntages.Contains(oppBase.type))
            {
                return (int)HaveAdvandage.yes;
            }
            else if(oppAdvantages.Contains(ourBase.type))
            {
                return (int)HaveAdvandage.no;
            }
            else
            {
                return (int)HaveAdvandage.none;
            }
        }

    }
}
