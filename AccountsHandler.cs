using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Xml.Linq;

public class Account
{
    public string Name { get; set; }
    public string Password { get; set; }

    public override string ToString()
    {
        return $"{Name}:{Password}";
    }
}

public static class AccountsHandler
{
    private const string FilePath = "accounts.txt";

    public static List<Account> LoadAccounts()
    {
        var accounts = new List<Account>();
        if (File.Exists(FilePath))
        {
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    accounts.Add(new Account { Name = parts[0], Password = parts[1] });
                }
            }
        }
        return accounts;
    }

    public static void AddAccount(List<Account> accounts)
    {
        if(!File.Exists(FilePath))
        {
            File.Create(FilePath).Close();
        }
        Console.Write("\nEnter account name:\n");
        string name = Regex.Replace(Console.ReadLine(), @"\s+", string.Empty);
        Console.Write("Enter password:\n");
        string password = Regex.Replace(Console.ReadLine(), @"\s+", string.Empty);
        accounts.Add(new Account { Name = name, Password = password });
        Console.WriteLine("Account added.");
    }

    public static void ViewAccounts(List<Account> accounts)
    {
        Console.WriteLine("\nStored Accounts:");
        foreach (var account in accounts)
        {
            Console.WriteLine(account);
        }
    }

    public static void SaveAccounts(List<Account> accounts)
    {
        var lines = new List<string>();
        foreach (var account in accounts)
        {
            lines.Add(account.ToString());
        }
        File.WriteAllLines(FilePath, lines);
        Console.WriteLine("Accounts saved to ." + FilePath);
    }
}