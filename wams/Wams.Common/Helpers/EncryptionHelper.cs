namespace Wams.Common.Helpers
{
    using System;
    using System.IO;
    using System.Text;

    public class EncryptionHelper
    {
        #region Hashing

        /*
        You could generate a salt from a Guid converted into a base 64 string, 
        then save that in the database as char. I use nvarchar to maximise my options using a .NET string.

        Then you can implement something like this for generating the original password hash, 
        and comparing the hash when the user logs in:
        */
        
        //public static byte[] GetHash(string password, string salt)
        //{
        //    byte[] unhashedBytes = Encoding.Unicode.GetBytes(String.Concat(salt, password));

        //    SHA256Managed sha256 = new SHA256Managed();
        //    byte[] hashedBytes = sha256.ComputeHash(unhashedBytes);

        //    return hashedBytes;
        //}

        //public static bool CompareHash(string attemptedPassword, byte[] hash, string salt)
        //{
        //    string base64Hash = Convert.ToBase64String(hash);
        //    string base64AttemptedHash = Convert.ToBase64String(GetHash(attemptedPassword, salt));

        //    return base64Hash == base64AttemptedHash;
        //}

        #endregion

        //public static string Encrypt(string clearText, string encryptionKey)
        //{
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}

        //public static string Decrypt(string cipherText, string encryptionKey)
        //{
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}
    }
}
