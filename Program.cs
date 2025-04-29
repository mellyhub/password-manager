using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<Account> accounts = AccountsHandler.LoadAccounts();
        
        while (true)
        {
            Console.WriteLine("\nPassword Manager:");
            Console.WriteLine("1. Add Account");
            Console.WriteLine("2. View Accounts");
            Console.WriteLine("3. Save & Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    AccountsHandler.AddAccount(accounts); 
                    break;
                case "2": 
                    AccountsHandler.ViewAccounts(accounts); 
                    break;
                case "3": 
                    AccountsHandler.SaveAccounts(accounts); 
                    return;
                default: 
                    Console.WriteLine("Invalid option."); 
                    break;
            }
        }
    }
}