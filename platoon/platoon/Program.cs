// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using platoon.Enum;
using platoon.Model;
using System.Text.Json.Serialization;

Console.WriteLine("Enter First Base");
string ourBase = Console.ReadLine();

Console.WriteLine("Enter Oppenent Base");
string oppBase =Console.ReadLine();

PlatoonOperation.FindWinning(ourBase, oppBase);
Console.ReadLine();
