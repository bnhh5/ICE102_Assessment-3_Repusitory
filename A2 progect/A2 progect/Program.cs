# nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;

namespace BankProgram
{
    public class UserData   // User Data stores the account information and balance of the user.
    {
        // Most default values are from the brief.
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; } = 24;
        public decimal Balance { get; set; } = 100;
        public UserData(string Username, string UserEmail, int UserAge, string UserPhoneNumber, string UserPassword, decimal UserBalance)   // This contaner holds the users data to ba called or used to ceate a
        {                                                                                                                                   // new account.
            Name = Username;
            Email = UserEmail;
            Age = UserAge;
            PhoneNumber = UserPhoneNumber;
            Password = UserPassword;
            Balance = UserBalance;
        }
        public bool Matches(string EnteredUsername, string EnteredPassword) //Compares usernames without case sensitivity while keeping passwords case-sensitive.
        {
            return string.Equals(Name, EnteredUsername, StringComparison.OrdinalIgnoreCase) && Password == EnteredPassword;
        }
        public decimal GetBalance() { return Balance; }

        public bool Credit(decimal amount)  // Boolean tells the caller wether the balance will updating the balance.
        {
            if (amount < 0 || decimal.Round(amount, 2) != amount || amount > 1000000000 - Balance)
            {
                return false;
            }
            else
            {
                Balance += amount;
                return true;
            }
        }
        public bool Debit(decimal amount)   // Boolean tells the caller wether the balance will updating the balance.
        {
            if (amount < 0 || decimal.Round(amount, 2) != amount || amount > Balance)
            {
                return false;
            }
            else
            {
                Balance -= amount;
                return true;
            }
        }
    }
    public class Program    // Acts as an entry point for the program, creating a bank instance, will handaling closed input so the program can exit without an unhandled exception.
    {
        public static void Main()
        {
            try { new Bank().Menus(); }
            catch (System.IO.EndOfStreamException)
            {
                Console.WriteLine("Input closed. Exiting safely.");
            }
            Console.WriteLine("Thank you for using the bank.");
        }
    }
    public class Bank   // Manages the account authentication, creation and banking operations.
    {
        private readonly List<UserData> users = new List<UserData>();
        private UserData? CurrentUser; // A null CurrentUser represents a signed-out session.
        private const int MaxAttempts = 3;
        private bool loop = false;
        public Bank()
        {
            users.Add(new UserData("Jhon Doe", "Jhon@example.com", 24, "0400000000", "Password123", 100)); //default demonstration account
        }
        public void Menus()
        {
            while (true)
            {
                Console.Clear();
                bool loop = true;
                                            // Selects the menu according to whether a user is authenticated.
                if (CurrentUser == null)    // Uses logged out menu when there is no authenticated user.
                {
                    Console.WriteLine("Welcome to the Bank");
                    Console.WriteLine("Enter 1 to Login. Enter 2 to Signup. Enter 3 to Quit:");

                    while (loop == true)    // Local menu loops keep input handling separate from other operations.
                    {
                        string input = Console.ReadLine();
                        int.TryParse(input, out int choice);
                        switch (choice)
                        {
                            case 1:
                                Login();
                                loop = false;
                                break;
                            case 2:
                                Signup();
                                loop = false;
                                break;
                            case 3:
                                Quit();
                                loop = false;
                                break;
                            default:
                                Console.WriteLine("\nInvalid input. Only input 1, 2 or 3. Try again.");
                                Console.ReadLine();
                                loop = true;
                                break;
                        }
                    }

                }
                else // Uses logged in menu when there is a authenticated user.
                {
                    Console.WriteLine("Welcome " + CurrentUser.Name);
                    Console.WriteLine("Enter 1 to Deposit. Enter 2 to Withdraw. Enter 3 to View Balance. Enter 4 to Logout. Enter 5 to Quit:");

                    while (loop == true)    // Local menu loops keep input handling separate from other operations.
                    {
                        string input = Console.ReadLine();
                        int.TryParse(input, out int choice);
                        switch (choice)
                        {
                            case 1:
                                Deposit();
                                loop = false;
                                break;
                            case 2:
                                Withdraw();
                                loop = false;
                                break;
                            case 3:
                                ViewBalance();
                                loop = false;
                                break;
                            case 4:
                                Logout();
                                loop = false;
                                break;
                            case 5:
                                Quit();
                                loop = false;
                                break;
                            default:
                                {
                                    Console.WriteLine("\nInvalid input. Only input 1, 2, 3, 4 or 5. Try again.");
                                    Console.WriteLine("\nPress enter to proceed");
                                    Console.ReadLine();
                                    loop = true;
                                    break;
                                }
                        }
                    }
                }
            }
        }

        private void Login()
        {
            Console.Clear();
            Console.WriteLine("You've selected login.");
            bool loop = true;

            while (loop == true)
            {
                Console.WriteLine("Do you wish to login? Enter 1 to go back or enter 2 to login:");

                string input = Console.ReadLine();
                int.TryParse(input, out int choice);

                switch (choice)
                {
                    case 1:
                        loop = false;
                        break;
                    case 2:
                        loop = false;
                        LoginMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Only input 1 or 2.");
                        loop = true;
                        break;
                }
            }
        }
        public bool LoginCheck(string Username, string Password)    // Match credentials against the stored accounts.
        {
            CurrentUser = null;
            foreach (UserData user in users)
            {
                if (user.Matches(Username, Password))
                {
                    CurrentUser = user;
                    return true;
                }
            }
            return false;
        }

        private void LoginMenu()    // Limits attempts in this login session prevent unauthenticated access.
        {
            int Attempt = 1;

            while ( Attempt <= MaxAttempts)
            {
                Console.WriteLine("Enter your username:");
                string AttemptUsername = Console.ReadLine().Trim();

                Console.WriteLine("\nEnter your password:");
                string AttemptPassword = Console.ReadLine().Trim();

                if (LoginCheck(AttemptUsername, AttemptPassword))
                {
                    Console.WriteLine("\nLogin successful.");
                    Console.WriteLine("\nPress enter to proceed");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Console.WriteLine("\nInvalid username or password. Attempts left: " + (MaxAttempts - Attempt));
                }
                Attempt++;
            }
            Console.WriteLine("\nMaximum login attempts reached. Press enter to return to the menu.");
            Console.ReadLine();
        }
        private void Signup()   // Creates a new account.
        {
            Console.Clear();
            bool loop = false;
            string EnterdUsername;

            Console.WriteLine("You have selected signup.");
            Console.WriteLine("\nEnter your name:");

            do
            {
                if (loop == true)
                {
                    Console.WriteLine("\nUsername already exists or is null. Choose another username.");
                }
                else;

                EnterdUsername = Console.ReadLine().Trim();

                if (!users.Exists(user => string.Equals(user.Name, EnterdUsername, StringComparison.OrdinalIgnoreCase)) || EnterdUsername == null)
                {
                    loop = true;
                }
                else
                {
                    loop = false;
                }
            } while (loop == true); // Prevents two accounts from sharing a Username.
            loop = false;

            Console.WriteLine("\nEnter your email:");
            string EnterdEmail = Console.ReadLine().Trim();

            int EnterdAge;

            do
            {
                if (loop == true)
                {
                    Console.WriteLine("\nInvalid age");
                }

                Console.WriteLine("\nEnter your age:");
                string input = Console.ReadLine();
                int.TryParse(input, out EnterdAge);

                if (EnterdAge < 0)
                {
                    loop = true;
                }
                else
                {
                    loop = false;
                }
            } while (loop == true); // Prevents accounts an age grater then 0.
            loop = false;

            Console.WriteLine("\nEnter your phone number:");
            string EnterdPhoneNumber = Console.ReadLine().Trim();

            Console.WriteLine("\nEnter your password:");
            string EnterdPassword = Console.ReadLine().Trim();

            Console.Clear();
            Console.WriteLine("Confirm your password:");

            while (Console.ReadLine().Trim() != EnterdPassword) // Password confirmation helps prevent creating an account with a mistyped password.
            {
                Console.WriteLine("\nPasswords do not match. Try again.");
            }

            // New accounts start at zero and bypasses login.
            users.Add(new UserData(EnterdUsername, EnterdEmail, EnterdAge, EnterdPhoneNumber, EnterdPassword, 0));
            LoginCheck(EnterdUsername, EnterdPassword);

            Console.WriteLine("\nYou have successfully created an account. Press enter to proceed:");
            Console.WriteLine("\nPress enter to proceed");
            Console.ReadLine();
        }
        private void Deposit()
        {
            Console.Clear();
            Console.WriteLine("You've selected deposit.");
            bool loop = false;

            do
            {
                Console.WriteLine("Do you wish to deposit? Enter 1 to go back or enter 2 to deposit:");

                string input = Console.ReadLine();
                int.TryParse(input, out int choice);

                switch (choice)
                {
                    case 1:
                        loop = false;
                        break;
                    case 2:
                        DepositAmount();
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Only input 1 or 2.");
                        loop = true;
                        break;
                }
            } while (loop == true);
        }
        private void DepositAmount()
        {
            bool loop = false;

            do
            {
                Console.WriteLine("Enter the amount you wish to deposit:");
                string input = Console.ReadLine();
                decimal.TryParse(input, out decimal amount);

                if (amount < 0 || decimal.Round(amount, 2) != amount)
                {
                    Console.WriteLine("\nEnter a non-negative amount, in dollars and cents.");
                    loop = true;
                }
                else if (!CurrentUser.Credit(amount))
                {
                    Console.WriteLine("\nDeposit exceeds the maximum account balance of $1,000,000,000.");
                    loop = true;
                }
                else
                {
                    Console.WriteLine("\nYou have deposited $" + amount);
                    CurrentUser.Credit(amount);
                    ViewBalance();
                    loop = false;
                }
            } while (loop == true);
        }
        private void Withdraw()
        {
            Console.Clear();
            Console.WriteLine("You've selected withdraw.");
            bool loop = false;

            do
            {
                Console.WriteLine("Do you wish to withdraw? Enter 1 to go back or enter 2 to withdraw:");

                string input = Console.ReadLine();
                int.TryParse(input, out int choice);

                switch (choice)
                {
                    case 1:
                        loop = false;
                        break;
                    case 2:
                        WithdrawAmount();
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Only input 1 or 2.");
                        loop = true;
                        break;
                }
            } while (loop == true);
        }
        private void WithdrawAmount()
        {
            bool loop = false;

            do
            {
                Console.WriteLine("Enter the amount you wish to withdraw:");
                string input = Console.ReadLine();
                decimal.TryParse(input, out decimal amount);
                if (amount < 0 || decimal.Round(amount, 2) != amount)
                {
                    Console.WriteLine("\nEnter a non-negative, in dollars and cents.");
                    loop = true;
                }
                else if (!CurrentUser.Debit(amount))
                {
                    Console.WriteLine("\nEnter a number that is not grater then your balance.");
                    loop = true;
                }
                else
                {
                    Console.WriteLine("\nYou have withdrawn $" + amount);
                    CurrentUser.Debit(amount);
                    ViewBalance();
                    loop = false;
                }
            } while (loop == true);
        }
        private void ViewBalance()
        {
            Console.WriteLine("\nYour Current balance is $" + CurrentUser.GetBalance());
            Console.WriteLine("\nPress enter to proceed");
            Console.ReadLine();
        }
        private void Logout()
        {
            Console.Clear();
            Console.WriteLine("You've selected logout.");
            loop = false;

            do
            {
                Console.WriteLine("Do you wish to logout? Enter 1 to go back or enter 2 to logout:");

                string input = Console.ReadLine();
                int.TryParse(input, out int choice);

                switch (choice)
                {
                    case 1:
                        loop = false;
                        break;
                    case 2: // Resetting the user to null makes Menus desplay the signed-out options.
                        CurrentUser = null;
                        Console.WriteLine("\nYeu have been signed out. Press enter to return to the menu:");
                        Console.WriteLine("\nPress enter to proceed");
                        Console.ReadLine();
                        loop = false;
                        break;

                    default:
                        Console.WriteLine("\nInvalid input. Only input 1 or 2.");
                        loop = true;
                        break;
                }
            } while (loop == true);
        }
        private void Quit()
        {
            Console.Clear();

            Console.WriteLine("You've selected quit.");
            Console.WriteLine("Do you wish to quit? Enter 1 to go back or enter 2 to quit:");
            string input = Console.ReadLine();
            int.TryParse(input, out int choice);

            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInvalid input. Only input 1 or 2.");
                    loop = true;
                    break;
            }
        }
    }
}