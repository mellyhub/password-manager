using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

public static class AccountsHandler
{
    private const string FilePath = "accounts.txt";

    // Configuration object to load settings from appsettings.json
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    // AES encryption key loaded from appsettings.json
    private static readonly byte[] Key = Encoding.UTF8.GetBytes(Configuration["Encryption:Key"]);

    // AES initialization vector (IV) loaded from appsettings.json
    private static readonly byte[] IV = Encoding.UTF8.GetBytes(Configuration["Encryption:IV"]);

    // Static constructor to validate the encryption key and IV
    static AccountsHandler()
    {
        ValidateKeyAndIV();
    }

    private static void ValidateKeyAndIV()
    {
        // Validates the encryption key and IV
        if (Key.Length != 16 && Key.Length != 24 && Key.Length != 32)
        {
            throw new CryptographicException("Invalid AES key size. Key must be 16, 24, or 32 bytes.");
        }

        if (IV.Length != 16)
        {
            throw new CryptographicException("Invalid AES IV size. IV must be 16 bytes.");
        }
    }

    public static List<Account> LoadAccounts()
    {
        var accounts = new List<Account>();

        // Check if the file exists. If not, return an empty list
        if (!File.Exists(FilePath))
        {
            return accounts;
        }

        // Read and decrypt each line from the file
        foreach (var line in File.ReadAllLines(FilePath))
        {
            try
            {
                var decryptedLine = Decrypt(line);

                // Validate and parse the decrypted line
                if (TryParseAccount(decryptedLine, out var account))
                {
                    accounts.Add(account);
                }
                else
                {
                    Console.WriteLine($"Skipping malformed line: {line}");
                }
            }
            catch (Exception ex) when (ex is FormatException || ex is CryptographicException)
            {
                Console.WriteLine($"Skipping invalid or corrupted line: {line}");
            }
        }

        return accounts;
    }

    private static bool TryParseAccount(string decryptedLine, out Account account)
    {
        account = null;

        if (string.IsNullOrWhiteSpace(decryptedLine))
        {
            return false;
        }

        // Split the line into parts (username and password)
        var parts = decryptedLine.Split(':');

        // Ensure the line contains exactly two parts
        if (parts.Length != 2)
        {
            return false;
        }

        account = new Account(parts[0], parts[1]);
        return true;
    }

    public static void AddAccount(List<Account> accounts)
    {
        Console.Write("\nEnter account name: ");
        string username = Console.ReadLine();

        Console.Write("Enter account password: ");
        string password = Console.ReadLine();

        accounts.Add(new Account(username, password));
        Console.WriteLine("\nAccount added successfully.");
    }

    public static void ViewAccounts(List<Account> accounts)
    {
        Console.WriteLine("\nStored Accounts:");
        foreach (var account in accounts)
        {
            Console.WriteLine($"Username: {account.Username}, Password: {account.Password}");
        }
    }

    public static void SaveAccounts(List<Account> accounts)
    {
        var lines = new List<string>();

        foreach (var account in accounts)
        {
            var encryptedLine = Encrypt($"{account.Username}:{account.Password}");
            lines.Add(encryptedLine);
        }

        File.WriteAllLines(FilePath, lines);
        Console.WriteLine("Accounts saved to " + FilePath);
    }
    private static string Encrypt(string plainText)
    {
        // Encrypts plaintext using AES encryption
        using (var aes = CreateAes())
        {
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cs))
                {
                    writer.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private static string Decrypt(string cipherText)
    {
        // Decrypts Base64-encoded string using AES encryption
        using (var aes = CreateAes())
        {
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cs))
            {
                return reader.ReadToEnd();
            }
        }
    }

    private static Aes CreateAes()
    {
        // Create and configure an AES encryption object
        var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;
        return aes;
    }
}