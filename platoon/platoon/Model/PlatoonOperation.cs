using Newtonsoft.Json;
using platoon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace platoon.Model
{
    public class PlatoonOperation
    {
        public static List<Platoon> ourBaseList = new List<Platoon>();
        public static List<Platoon> oppBaseList = new List<Platoon>();

        public static Dictionary<int, List<BattleArea>> possibleWinningObj = new Dictionary<int, List<BattleArea>>();
        public static List<WinningPlatoonCombination> winningcombination = new List<WinningPlatoonCombination>();
        public static List<Dictionary<int, int>> OverAllCombinations = new List<Dictionary<int, int>>();


        public static void FindWinning(string platoons1 , string platoons2)
        {
            ourBaseList = Platoon.CovertStringToPlatoon(platoons1);
            oppBaseList = Platoon.CovertStringToPlatoon(platoons2);

            FindWinningPossiblities();
            var allCombination = PlatoonOperation.FindAllCombination(winningcombination);
            AnalystResult(allCombination);
        }

        private static void FindWinningPossiblities()
        {
            foreach (var item in oppBaseList)
            {
                List<BattleArea> winningList = new List<BattleArea>();
                List<int> winningIdslist = new List<int>();
                foreach (var ourBase in ourBaseList)
                {
                    BattleArea platoonOperation = new BattleArea(ourBase, item);

                    //Separate those who are winning alone.
                    if (platoonOperation.winningStatus == (int)WinningStatus.yes)
                    {
                        winningList.Add(platoonOperation);
                        winningIdslist.Add(platoonOperation.ourBase.id);
                    }
                }

                //make of dist like oppbase1-> wins which bases
                possibleWinningObj.Add(item.id, winningList);//Full obj - temp 

                winningcombination.Add(new WinningPlatoonCombination() { oppbaseObjId = item.id, platoonIdsList = winningIdslist });//names alone
            }
        }

        private static void AnalystResult(List<Dictionary<int,int>> allCombination) 
        {
            var finalList = allCombination.Where(x => x.Count > 2).ToList();

            if (finalList.Count > 0)
            {
                var first = finalList.First();

                Stack<int> notPossibleWinners = new Stack<int>(ourBaseList.Where(item => first.Values.Contains(item.id) == false).Select(item => item.id).ToList());

                List<string> endResult = new List<string>();
                foreach (var item in oppBaseList)
                {
                    int searchResult;
                    if (first.ContainsKey(item.id))
                    {
                        searchResult = first[item.id];
                    }
                    else
                    {
                        searchResult = notPossibleWinners.Pop();
                    }
                    endResult.Add(ourBaseList.Where(x => x.id == searchResult).Select(x => x.actualname).ToList().First());
                }

                Console.WriteLine("winning order");
                Console.WriteLine(String.Join(";", endResult));
                Console.WriteLine($"By this order we can achive {first.Count} success.");
            }
            else
            {
                Console.WriteLine("There is no chance of winning");
            }

            //Console.WriteLine(JsonConvert.SerializeObject(possibleWinnerNames));
        }

        public static List<Dictionary<int, int>> FindAllCombination(List<WinningPlatoonCombination> winningcombination)
        {
            OverAllCombinations = new List<Dictionary<int, int>>();

            Dictionary<int, int> avaliCombination = new Dictionary<int, int>();
            List<int> usedPlatoon = new List<int>();

            var first = winningcombination.First();
            foreach (var objId in first.platoonIdsList)
            {
                avaliCombination.Add(first.oppbaseObjId, objId);
                usedPlatoon.Add(objId);
                FindCombination(avaliCombination, winningcombination.Skip(1).ToList(), usedPlatoon);
                avaliCombination = new Dictionary<int, int>();
                usedPlatoon = new List<int>();
            }

            //Console.WriteLine(JsonConvert.SerializeObject(OverAllCombinations));
            //Console.WriteLine(JsonConvert.SerializeObject(OverAllCombinations.Where(x => x.Count > 2).ToList()));

            return OverAllCombinations;
        }

        private static Dictionary<int, int> FindCombination(Dictionary<int, int> avaliCombination, List<WinningPlatoonCombination> winningcombination, List<int> usedPlatoon)
        {
            foreach (var obj in winningcombination)
            {
                foreach (var objId in obj.platoonIdsList)
                {
                    if (usedPlatoon.Contains(objId) == false)
                    {
                        avaliCombination.Add(obj.oppbaseObjId, objId);
                        usedPlatoon.Add(objId);
                        if (winningcombination.Count > 1)
                        {
                            winningcombination = winningcombination.Skip(1).ToList();
                            avaliCombination = FindCombination(new Dictionary<int, int>(avaliCombination), winningcombination, usedPlatoon);
                            OverAllCombinations.Add(JsonConvert.DeserializeObject<Dictionary<int, int>>(JsonConvert.SerializeObject(avaliCombination)));
                            //Console.WriteLine("overAll -" + JsonConvert.SerializeObject(OverAllCombinations));
                            avaliCombination.Remove(obj.oppbaseObjId);
                            usedPlatoon.Remove(objId);
                        }
                    }
                }
            }
            return avaliCombination;
        }
    }
}
