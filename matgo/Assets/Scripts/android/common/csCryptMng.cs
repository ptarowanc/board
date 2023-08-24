using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;

public class csCryptMng {

	/// <summary>AES256 암호화 키값.</summary>
	/// <remarks>
	/// </remarks> 
	public static string KEY_AES256
	{
		get { return "ip2KtvDhTkLe8oq4L6a1AzOUT71matgo"; }
	}

    public static byte[] StringToByte(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static string ByteToString(byte[] strByte)
    {
        return Encoding.UTF8.GetString(strByte);
    }

	public static byte[] HexStringToBytes(string s)
	{
		const string HEX_CHARS = "0123456789ABCDEF";

		//형식 : 12-34-56 경우 아래의 로직.
		if (s.Length == 0)
			return new byte[0];
		
		s = s.TrimStart('-');
		
		if ((s.Length + 1) % 3 != 0)
			throw new FormatException();
		
		byte[] bytes = new byte[(s.Length + 1) / 3];
		
		int state = 0; // 0 = expect first digit, 1 = expect second digit, 2 = expect hyphen
		int currentByte = 0;
		int x;
		int value = 0;
		
		foreach (char c in s)
		{
			switch (state)
			{
			case 0:
				x = HEX_CHARS.IndexOf(Char.ToUpperInvariant(c));
				if (x == -1)
					throw new FormatException();
				value = x << 4;
				state = 1;
				break;
			case 1:
				x = HEX_CHARS.IndexOf(Char.ToUpperInvariant(c));
				if (x == -1)
					throw new FormatException();
				bytes[currentByte++] = (byte)(value + x);
				state = 2;
				break;
			case 2:
				if (c != '-')
					throw new FormatException();
				state = 0;
				break;
			}
		}

	
		return bytes;
	}
	
	public static String EncryptString_AES256(String PlainText ,string Password)  
	{  
		RijndaelManaged aes = new RijndaelManaged();
		aes.KeySize = 256;
		aes.BlockSize = 128;
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;
		aes.Key = Encoding.UTF8.GetBytes(Password);        
		aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		
		var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
		byte[] xBuff = null;
		using (var ms = new MemoryStream())
		{
			using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
			{
				byte[] xXml = Encoding.UTF8.GetBytes(PlainText);
				cs.Write(xXml, 0, xXml.Length);
			}
			
			xBuff = ms.ToArray();
		}
		String Output = Convert.ToBase64String (xBuff);
		
		Output = "-" +  BitConverter.ToString(Encoding.UTF8.GetBytes(Output));
		return Output;
	}  
	public static String EncryptString_AES256(String PlainText ,string Password,bool bHex)  
	{  
		RijndaelManaged aes = new RijndaelManaged();
		aes.KeySize = 256;
		aes.BlockSize = 128;
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;
		aes.Key = Encoding.UTF8.GetBytes(Password);        
		aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		
		var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
		byte[] xBuff = null;
		using (var ms = new MemoryStream())
		{
			using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
			{
				byte[] xXml = Encoding.UTF8.GetBytes(PlainText);
				cs.Write(xXml, 0, xXml.Length);
			}
			
			xBuff = ms.ToArray();
		}
		String Output = Convert.ToBase64String (xBuff);
		if(bHex)
		{
			Output = "-" +  BitConverter.ToString(Encoding.UTF8.GetBytes(Output));
		}
		return Output;
	}  
	
	//AES_256 복호화  
	public static String DecryptString_AES256(string CipherText, string key)
	{  
		CipherText = CipherText.Replace ("\r", "").Replace ("\n", "").Replace ("\r\n", "").Replace (" ", "");
		//Debug.Log("CipherText : [" + CipherText + "]");
        byte[] bytes = HexStringToBytes (CipherText);
		CipherText = Encoding.UTF8.GetString(bytes);
		
		RijndaelManaged aes = new RijndaelManaged();
		aes.KeySize = 256;
		aes.BlockSize = 128;
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;
		aes.Key = Encoding.UTF8.GetBytes(key);
		aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		
		var decrypt = aes.CreateDecryptor();
		byte[] xBuff = null;
		using (var ms = new MemoryStream())
		{
			using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
			{
				byte[] xXml = Convert.FromBase64String(CipherText);
				cs.Write(xXml, 0, xXml.Length);
			}
			
			xBuff = ms.ToArray();
		}
		
		String Output = Encoding.UTF8.GetString(xBuff);
		return Output;
	}  

    public static String DecryptString_AES256(string CipherText, string key,bool bHex)
    {  
        
        CipherText = CipherText.Replace ("\r", "").Replace ("\n", "").Replace ("\r\n", "").Replace (" ", "");

        CipherText = bHex ? ByteToString(HexStringToBytes (CipherText)) : CipherText;

        RijndaelManaged aes = new RijndaelManaged();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        var decrypt = aes.CreateDecryptor();
        byte[] xBuff = null;
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
            {
                byte[] xXml = Convert.FromBase64String(CipherText);
                cs.Write(xXml, 0, xXml.Length);
            }

            xBuff = ms.ToArray();
        }

        String Output = Encoding.UTF8.GetString(xBuff);
        return Output;
    }  

	
	public static string EncryptString(string value)  
	{  
		string key = "matgosoftqwer778899";  
		MD5 md5Hash = new MD5CryptoServiceProvider();  
		byte[] _secret = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));  
		
		// Encrypt '_value' into a byte array  
		byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);  
		
		// Eecrypt '_value' with 3DES.  
		TripleDES des = new TripleDESCryptoServiceProvider();  
		des.Key = _secret;  
		des.Mode = CipherMode.ECB;  
		ICryptoTransform xform = des.CreateEncryptor();  
		byte[] encrypted = xform.TransformFinalBlock(bytes, 0, bytes.Length);  
		
		// Convert encrypted array into a readable string.  
		string encryptedString = Convert.ToBase64String(encrypted);  
		
		return encryptedString;
	}
	public static string DecryptString(string value)  
	{  
		if(value == null)
		{
			return "";
		}
		
		string key = "matgosoftqwer778899";  
		
		MD5 md5Hash = new MD5CryptoServiceProvider();  
		byte[] _secret = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key)); 
		
		
		byte[] bytes = Convert.FromBase64String(value);  
		
		// Decrypt '_value' with 3DES.  
		TripleDES des = new TripleDESCryptoServiceProvider();  
		des.Key = _secret;  
		des.Mode = CipherMode.ECB;  
		ICryptoTransform xform = des.CreateDecryptor();  
		byte[] decrypted = xform.TransformFinalBlock(bytes, 0, bytes.Length);  
		
		// decrypte_value as a proper string.  
		string decryptedString = System.Text.Encoding.UTF8.GetString(decrypted);  
		
		return decryptedString;  
	}
}
