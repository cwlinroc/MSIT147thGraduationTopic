// See https://aka.ms/new-console-template for more information

using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.Services;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;

//////RandomNumberGenerator.GetInt32(10)

var service = new SimulationDataInsertService(null);

service.AddRandomMembers(500);
Console.WriteLine("AddRandomMembers Done!");
/**
service.AddRandomMerchandiseAndSpecs(90);
Console.WriteLine("AddRandomMerchandiseAndSpecs Done!");
**/
service.AddRandomCart();
Console.WriteLine("AddRandomCart Done!");

await service.AddRandomOrders();
Console.WriteLine("AddRandomOrders Done!");
/**
service.AddSpecTags();
Console.WriteLine("AddSpecTags Done!");

service.AddSpecPopularity();
Console.WriteLine("AddSpecPopularity Done!");
**/
service.AddRandomEvaluations();
Console.WriteLine("AddRandomEvaluations Done!");

await service.AddSpecifySpecOrders(false, 15, 20);
await service.AddSpecifySpecOrders(true, 30, 30);

//var generator = new RandomGenerator();

//for (int i = 0; i < 100; i++)
//{
//    int itemAmount = (int)(generator.RandomDouble().InvCSND(0.9,0.1) * 5);
//    itemAmount = Math.Max(itemAmount, 1);
//    Console.WriteLine(itemAmount);
//}


/**test**/


/**test**/

Console.WriteLine("success");

