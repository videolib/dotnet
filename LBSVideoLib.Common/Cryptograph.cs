﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;


namespace LBFVideoLib.Common
{
    public class Cryptograph
    {
        private static string password = @"myKey123";

        public static void EncryptObject(Object obj , string outputFile)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                string json = JsonConvert.SerializeObject(obj);

                using (FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create))
                {

                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    using (CryptoStream cs = new CryptoStream(fsCrypt,
                         RMCrypto.CreateEncryptor(key, key),
                         CryptoStreamMode.Write))
                    {

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(memoryStream, obj);
                            // This resets the memory stream position for the following read operation
                            memoryStream.Seek(0, SeekOrigin.Begin);

                            // Get the bytes
                            var bytes = new byte[memoryStream.Length];
                            memoryStream.Read(bytes, 0, (int)memoryStream.Length);

                            cs.Write(bytes, 0, bytes.Length);
                            cs.Flush();
                            bf = null;

                        }
                        cs.Close();
                        fsCrypt.Close();
                    }
                }
            }
            catch (Exception)
            {
                //log
                throw;
            }
        }

        public static T DecryptObject<T>(string inputFile)
        {
            T cInfo;
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
            {
                RijndaelManaged RMCrypto = new RijndaelManaged();
                using (CryptoStream cs = new CryptoStream(fsCrypt,
                      RMCrypto.CreateDecryptor(key, key),
                      CryptoStreamMode.Read))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        int data;
                        while ((data = cs.ReadByte()) != -1)
                            m.WriteByte((byte)data);
                        BinaryFormatter bf = new BinaryFormatter();
                        m.Position = 0;
                        cInfo = (T)bf.Deserialize(m);
                    }
                }
            }
            return cInfo;
        }

        public static void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                // log
                throw;
            }
        }
        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Decrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        public static void DecryptFile(string inputFile, string outputFile)
        {

            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (System.Exception ex)
            {
                // log
                throw;
            }


        }
    }
}

