using System.Security.Cryptography;
using System.Text;

namespace SelfModifyingCode.Common;

public static class SHA256Helper
{

    public static string GetFileSHA256Hex(string filePath)
    {
        using var hash = SHA256.Create();
        using var fileStream = File.Open(filePath, FileMode.Open);
        var result = hash.ComputeHash(fileStream);
        return ByteToHexString(result);
    }

    private static string ByteToHexString(byte[] bytes)
    {
        var builder = new StringBuilder();
        foreach (var byteCell in bytes)
        {
            builder.Append(byteCell.ToString("x2"));
        }
        return builder.ToString();  
    }
    
}