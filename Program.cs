using System;
using System.Security.Cryptography;
using System.IO;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)

        {
            string secretString = args[0];
            string cipherText = args[1];
            DateTime startingTime = new DateTime(2020,7,3,11,0,0);
            DateTime endingTime = new DateTime(2020,7,4,11,0,0);
            cycleThroughKeys(startingTime, endingTime,secretString, cipherText);
        }

 
    private static string Encrypt(byte[] key, string secretString)
    {
        DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms,
        csp.CreateEncryptor(key, key), CryptoStreamMode.Write);
        StreamWriter sw = new StreamWriter(cs);
        sw.Write(secretString);
        sw.Flush();
        cs.FlushFinalBlock();
        sw.Flush();
        return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
    }

    public static byte[] tryNewKey(DateTime time)
    {
        TimeSpan ts = time.Subtract(new DateTime(1970, 1, 1));
        Random rng = new Random((int)ts.TotalMinutes);
        byte[] key = BitConverter.GetBytes(rng.NextDouble());
        return key;
    }
    public static int getSeed(DateTime time)
    {
        TimeSpan ts = time.Subtract(new DateTime(1970, 1, 1));
        Random rng = new Random((int)ts.TotalMinutes);
        
        return (int)ts.TotalMinutes;
    }
    public static void cycleThroughKeys(DateTime beginTime, DateTime endTime, String plainText, String cipherText)
    {
        //Loop through dates to generate key 
        for (DateTime i = beginTime; i< endTime; i=i.AddMinutes(1))
        {
                byte[] testkey = tryNewKey(i);
                String tempCipher = Encrypt(testkey, plainText);
               // Console.WriteLine("Time is currently = "+ i.ToString());
                if(tempCipher == cipherText)
                {
                    Console.WriteLine(getSeed(i).ToString());
                    break;
                }

        }
        
    }

    }
}
