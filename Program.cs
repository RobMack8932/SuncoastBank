using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace SuncoastBank
{

    class Transaction
    {
        public string Type { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
    }
    class Program
    {
        private static decimal TotalAccountBalance(List<Transaction> transactions, string account, string Account)
        {
            var Deposits = transactions.Where(transaction => transaction.Account == account && transaction.Type == "Deposit");
            var TotalDeposits = Deposits.Sum(transaction => transaction.Amount);

            var Withdraws = transactions.Where(transaction => transaction.Account == account && transaction.Type == "Withdraw");
            var TotalWithdraws = Withdraws.Sum(transaction => transaction.Amount);

            var balance = TotalDeposits - TotalWithdraws;

            return balance;
        }
        static decimal AskForAmount(string prompt)
        {
            decimal result = 0.00m;
            bool positiveNumber = false;

            while (!positiveNumber)
            {
                Console.WriteLine(prompt);

                positiveNumber = decimal.TryParse(Console.ReadLine(), out result);

                // Invalid input: not a number
                if (!positiveNumber)
                {
                    Console.WriteLine("Invalid input! Not a number.");
                }

                // Invalid input: not a positive number
                if (positiveNumber && result < 0)
                {
                    Console.WriteLine($"Invalid input! Not a positive number. ");
                    positiveNumber = false;
                }
            }

            return result;
        }

        static void Main(string[] args)
        {




            TextReader reader;

            if (File.Exists("Transactions.csv"))
            {
                reader = new StreamReader("Transactions.csv");
            }
            else
            {
                reader = new StringReader("");
            }

            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var transactions = csvReader.GetRecords<Transaction>().ToList();

            reader.Close();
            var deposit = new Transaction()
            {
                Type = "Deposit",
                Amount = 100.00m,
                Account = "Checking",
            };
            var deposit2 = new Transaction()
            {
                Type = "Deposit",
                Amount = 500m,
                Account = "Saving",
            };
            var withdraw = new Transaction()
            {
                Type = "Withdraw",
                Amount = 50m,
                Account = "Checking",
            };
            var withdraw2 = new Transaction()
            {
                Type = "Withdraw",
                Amount = 200m,
                Account = "Saving",
            };

            transactions.Add(deposit);
            transactions.Add(deposit2);
            transactions.Add(withdraw);
            transactions.Add(withdraw2);
            bool usingMainMenu = true;
            while (usingMainMenu)
            {
                Console.Write(" Would you like to (d)eposit, (w)ithdraw, (v)iew, or e(x)it?  ");
                string userChoice = Console.ReadLine();
                if (userChoice == "d")
                {
                    bool depositing = true;

                    while (depositing)
                    {
                        Console.WriteLine("(c)hecking - (s)avings -or- e(x)it");
                        string userAccountChoice = Console.ReadLine();
                        if (userAccountChoice == "c")
                        {
                            var userDeposit = AskForAmount("What is the amount you'd like to deposit in your checking account today?");
                            var newDepositTransaction = new Transaction()
                            {
                                Type = "Deposit",
                                Account = "Checking",
                                Amount = userDeposit
                            };

                            transactions.Add(newDepositTransaction);

                            Console.WriteLine($"You have deposited ${userDeposit} into your Checking account.");
                        }
                        if (userAccountChoice == "s")
                        {
                            var SavingsAmount = AskForAmount("What is the amount you'd like to deposit in savings today?");
                            var newSavingsTransaction = new Transaction()
                            {
                                Type = "Deposit",
                                Account = "Savings",
                                Amount = SavingsAmount
                            };

                            transactions.Add(newSavingsTransaction);

                            Console.WriteLine($"You have deposited ${SavingsAmount} into your Savings account.");

                        }
                        if (userAccountChoice == "x")
                        {
                            Console.WriteLine($"Entering Main Menu.");
                            depositing = false;
                        }


                    }
                }
                if (userChoice == "w")
                {
                    bool withdrawing = true;

                    while (withdrawing)
                    {
                        Console.WriteLine("Withdraw from (c)hecking (s)avings or e(x)it.");
                        var userWithdrawAccountChoice = Console.ReadLine();

                        if (userWithdrawAccountChoice == "c")
                        {
                            var checkingWithdrawAmount = AskForAmount($"How much do you want to withdraw from your checking account?");
                            var newWithdrawTransaction = new Transaction()
                            {
                                Type = "Withdraw",
                                Account = "Checking",
                                Amount = checkingWithdrawAmount
                            };

                            transactions.Add(newWithdrawTransaction);

                            Console.WriteLine("Thank you, {withdrawAmount} has been withdrawn from your checking account.");
                        }

                        if (userWithdrawAccountChoice == "s")
                        {
                            var savingsWithdrawAmount = AskForAmount($"How much do you want to withdraw from your savings account?");

                            var newWithdrawTransaction = new Transaction()
                            {
                                Type = "Withdraw",
                                Account = "Saving",
                                Amount = savingsWithdrawAmount
                            };

                            transactions.Add(newWithdrawTransaction);

                            Console.WriteLine($"Thankyou, {savingsWithdrawAmount} has been withdrawn from your savings account");
                        }

                        if (userWithdrawAccountChoice == "x")
                        {
                            Console.WriteLine("Back to main menu");
                            withdrawing = false;
                        }


                    }
                }
                if (userChoice == "v")
                {
                    {
                        var checkingBalance = TotalAccountBalance(transactions, "Checking", "Saving");
                        var savingBalance = TotalAccountBalance(transactions, "Saving", "Checking");

                        Console.WriteLine($"Your checking balance is currently : ${checkingBalance}");

                        Console.WriteLine($"Your savings balance is currently : ${savingBalance}");

                    }

                }






                var fileWriter = new StreamWriter("Transactions.csv");
                var csvWriter = new CsvHelper.CsvWriter(fileWriter, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(transactions);
                fileWriter.Close();



            }
        }
    }
}


