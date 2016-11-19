using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace UMTD.Classes
{
    public class uMD5
    {
        public string Value { set; get; }

        /// <summary>
        /// Create new instance of uMd5 class
        /// </summary>
        /// <param name="value">Value for processing with md5</param>
        public uMD5(string value)
        {
            if (value == null)
                value = "";    
            this.Value = value;
        }
        /// <summary>
        /// Return md5 hash for Value property
        /// </summary>
        public string GetMd5Hash()
        {
            try
            {
                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                using (MD5 md5Hash = MD5.Create())
                {
                    // Convert the input string to a byte array and compute the hash.
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(this.Value));

                    // Loop through each byte of the hashed data 
                    // and format each one as a hexadecimal string.
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                }
                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
            catch (Exception exc)
            {
                //uLog.PutException(exc, "uMD5.GetMd5Hash");
                throw exc;
            }
        }

        /// <summary>
        /// Compare hash value and md5 hash value generated for Value property 
        /// </summary>
        /// <param name="hash">Hash for comparation</param>
        public bool VerifyMd5Hash(string hash)
        {
            try
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    // Hash the input.
                    string hashOfInput = this.GetMd5Hash();

                    // Create a StringComparer an compare the hashes.
                    StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                    if (0 == comparer.Compare(hashOfInput, hash))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exc)
            {
                //uLog.PutException(exc, "uMD5.VerifyMd5Hash");
                throw exc;
            }
        }

    }
}