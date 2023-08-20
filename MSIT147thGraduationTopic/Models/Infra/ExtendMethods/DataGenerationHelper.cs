using MathNet.Numerics.Distributions;
using System.Security.Cryptography;
using System.Text;

namespace MSIT147thGraduationTopic.Models.Infra.ExtendMethods
{
    static public class DataGenerationHelper
    {
        /// <summary>
        /// cumulated standered normal distribution,回傳一個最小為0最大為1的double
        /// </summary>
        /// <param name="number"></param>
        /// <param name="mean">中位數</param>
        /// <param name="stddev">標準差</param>
        /// <returns></returns>
        static public double InvCSND(this double number, double mean = 0.5, double stddev = 0.15)
        {
            double result = Normal.InvCDF(mean, stddev, number);

            return Math.Min(Math.Max(result, 0), 1);
        }

        /// <summary>
        /// hashed by md5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public int GetHashedInt(this string str)
        {
            using MD5 md5Hasher = MD5.Create();
            string newStr = str.GetSaltedSha256("idn", str);
            var hashed = md5Hasher.ComputeHash(Encoding.Default.GetBytes(newStr));
            return Math.Abs(BitConverter.ToInt32(hashed, 0));
        }

    }
}
