// See https://aka.ms/new-console-template for more information

using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.Services;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;

var service = new RandomInsertService(null);

//RandomNumberGenerator.GetInt32(10)

//service.AddRandomMembers(50);
//service.AddRandomMerchandiseAndSpecs(90);

//service.AddRandomCart();
//service.AddRandomOrders();
//service.AddSpecTags();
//service.AddSpecPopularity();
//service.AddRandomEvaluations();
var generator = new RandomGenerator();

for (int i = 0; i < 100; i++)
{
    int itemAmount = (int)(generator.RandomDouble().InvCSND(0.9,0.1) * 5);
    itemAmount = Math.Max(itemAmount, 1);
    Console.WriteLine(itemAmount);
}



Console.WriteLine("success");

