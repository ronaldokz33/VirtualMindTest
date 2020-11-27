using System;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace VirtualMind.NetTest.Arquitetura.Library.Util.Security
{
    public class CryptographyTool
    {
        public static string Decifrar(string cipherText)
        {
            if (cipherText != "")
            {
                string plainText, passPhrase, saltValue, hashAlgorithm, initVector;
                int passwordIterations, keySize;
                cipherText = cipherText.Replace(" ", "+");

                passPhrase = "kD3Sk6g9"; // can be any string
                saltValue = "LA3EG7F94D"; // can be any string
                hashAlgorithm = "SHA1"; // can be "MD5"
                passwordIterations = 2; // can be any number
                initVector = "7KgDa97eo3Hd2aL1"; // must be 16 bytes
                keySize = 256;
                /*Convert strings defining encryption key characteristics into byte
                 arrays. Let us assume that strings only contain ASCII codes.
                 If strings include Unicode characters, use Unicode, UTF7, or UTF8
                 encoding.*/
                byte[] initVectorBytes, saltValueBytes, cipherTextBytes;
                initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                //Convert our ciphertext into a byte array.
                cipherTextBytes = Convert.FromBase64String(cipherText);

                /*First, we must create a password, from which the key will be 
                derived. This password will be generated from the specified 
                passphrase and salt value. The password will be created using
                the specified hash algorithm. Password creation can be done in
                several iterations. */
                PasswordDeriveBytes password;

                password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                /*Use the password to generate pseudo-random bytes for the encryption
                key. Specify the size of the key in bytes (instead of bits).*/
                byte[] keyBytes;
                keyBytes = password.GetBytes(keySize / 8);

                /*Create uninitialized Rijndael encryption object.*/
                RijndaelManaged symmetricKey;
                symmetricKey = new RijndaelManaged();

                /*It is reasonable to set encryption mode to Cipher Block Chaining
                (CBC). Use default options for other symmetric key parameters.*/
                symmetricKey.Mode = CipherMode.CBC;

                /*Generate decryptor from the existing key bytes and initialization 
                vector. Key size will be defined based on the number of the key 
                bytes.*/
                ICryptoTransform decryptor;
                decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                //Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream;
                memoryStream = new MemoryStream(cipherTextBytes);

                //Define memory stream which will be used to hold encrypted data.
                CryptoStream cryptoStream;
                cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                /*Since at this point we don't know what the size of decrypted data
                will be, allocate the buffer long enough to hold ciphertext;
                plaintext is never longer than ciphertext.*/
                byte[] plainTextBytes;
                plainTextBytes = new byte[cipherTextBytes.Length];

                //Start decrypting.
                int decryptedByteCount;
                decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                //Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                /*Convert decrypted data into a string. 
                Let us assume that the original plaintext string was UTF8-encoded.
                Dim plainText As String*/
                plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                //Return decrypted string.
                return plainText;
            }
            else
            {
                return "";
            }
        }
        public static string Cifrar(string plainText)
        {
            if ((plainText != ""))
            {
                string Cifrar;
                string cipherText;
                string passPhrase;
                string saltValue;
                string hashAlgorithm;
                int passwordIterations;
                string initVector;
                int keySize;
                passPhrase = "kD3Sk6g9";
                saltValue = "LA3EG7F94D";
                hashAlgorithm = "SHA1";
                passwordIterations = 2;
                //  can be any number
                initVector = "7KgDa97eo3Hd2aL1";
                keySize = 256;
                //  Convert strings into byte arrays.
                //  Let us assume that strings only contain ASCII codes.
                //  If strings include Unicode characters, use Unicode, UTF7, or UTF8 
                //  encoding.
                byte[] initVectorBytes;
                initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes;
                saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                //  Convert our plaintext into a byte array.
                //  Let us assume that plaintext contains UTF8-encoded characters.
                byte[] plainTextBytes;
                plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                //  First, we must create a password, from which the key will be derived.
                //  This password will be generated from the specified passphrase and 
                //  salt value. The password will be created using the specified hash 
                //  algorithm. Password creation can be done in several iterations.
                PasswordDeriveBytes password;
                password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
                //  Use the password to generate pseudo-random bytes for the encryption
                //  key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes;
                keyBytes = password.GetBytes((keySize / 8));
                //  Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey;
                symmetricKey = new RijndaelManaged();
                //  It is reasonable to set encryption mode to Cipher Block Chaining
                //  (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;
                //  Generate encryptor from the existing key bytes and initialization 
                //  vector. Key size will be defined based on the number of the key 
                //  bytes.
                ICryptoTransform encryptor;
                encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                //  Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream;
                memoryStream = new MemoryStream();
                //  Define cryptographic stream (always use Write mode for encryption).
                CryptoStream cryptoStream;
                cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                //  Start encrypting.
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                //  Finish encrypting.
                cryptoStream.FlushFinalBlock();
                //  Convert our encrypted data from a memory stream into a byte array.
                byte[] cipherTextBytes;
                cipherTextBytes = memoryStream.ToArray();
                //  Close both streams.
                memoryStream.Close();
                cryptoStream.Close();
                //  Convert encrypted data into a base64-encoded string.
                // Dim cipherText As String
                cipherText = Convert.ToBase64String(cipherTextBytes);
                //  Return encrypted string.
                Cifrar = cipherText;
                return Cifrar;
            }
            else
            {
                return "";
            }
        }

        public static void LimparXSS(ref string dados)
        {
            dados = dados.Replace("<", "&lt;");
        }

        private static string getMD5(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);

                hash = hash.Substring(2, hash.Length - 4);

                return hash;
            }
        }

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
