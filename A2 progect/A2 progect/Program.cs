using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankProgram
{
    public class CommonData
    {
        public static int Input { get; set; } = 0;
        public static int Attempts { get; set; } = 3;
        public static bool LoopOccurred { get; set; } = false;
        public static bool AreEqual { get; set; } = false;
        public static bool LoggedIn { get; set; } = false;
        public static bool SignedUp { get; set; } = false;
    }
    public class UserData
    {
        // These are the user data, using the brief default values.
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; } = 24;
        public decimal Balance { get; set; } = 100;

        public UserData(string Username, string UserEmail, int UserAge, string UserPhoneNumber, string UserPassword, decimal UserBalance)
        {
            Name = Username;
            Email = UserEmail;
            Age = UserAge;
            PhoneNumber = UserPhoneNumber;
            Password = UserPassword;
            Balance = UserBalance;
        }
        public bool Matches(string EnteredUsername, string EnteredPassword)
        {
            return string.Equals(Name, EnteredUsername, StringComparison.OrdinalIgnoreCase) && Password == EnteredPassword;
        }
        public decimal GetBalance() { return Balance; }

        public bool Credit(decimal amount)
        {
            if (amount < 0 || amount > 1000000000 - Balance) return false;
            Balance += amount;
            return true;
        }

        public bool Debit(decimal amount)
        {
            if (amount < 0 || amount > Balance) return false;
            Balance -= amount;
            return true;
        }
    }
    public class Program
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
    public class Bank
    {
        private readonly List<UserData> users = new List<UserData>();
        private UserData CurrentUser;
        private const int MaxAttempts = 3;
        private bool loop = false;
        public Bank()
        {
            users.Add(new UserData("Jhon Doe", "Jhon@example.com", 24, "0400000000", "Password123", 100));
        }
        public void Menus()
        {
            while (true)
            {
                Console.Clear();
                loop = true;

                if (CurrentUser == null)
                {
                    Console.WriteLine("Welcome to the Bank");
                    Console.WriteLine("Enter 1 to Login. Enter 2 to Signup. Enter 3 to Quit:");

                    while (loop == true)
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
                                Console.WriteLine("\nInvalid input. Only input 1, 2 or 3.");
                                Console.ReadLine();
                                loop = true;
                                break;
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Welcome" + CurrentUser.Name);
                    Console.WriteLine("Enter 1 to Deposit. Enter 2 to Withdraw. Enter 3 to View Balance. Enter 4 to Logout. Enter 5 to Quit:");

                    while (loop == true)
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
                                    Console.WriteLine("\nInvalid input. Only input 1, 2, 3, 4 or 5.");
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
            loop = true;

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
        public bool LoginCheck(string Username, string Password)
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

        private void LoginMenu()
        {
            Console.Clear();

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
        }
        private void Signup()
        {
            Console.Clear();
            Console.WriteLine("You have selected signup.");

            Console.WriteLine("\nEnter your name:");
            string EnterdUsername = Console.ReadLine().Trim();

            Console.WriteLine("\nEnter your email:");
            string EnterdEmail = Console.ReadLine().Trim();

            Console.WriteLine("\nEnter your age:");
            string input = Console.ReadLine();
            int.TryParse(input, out int EnterdAge);

            Console.WriteLine("\nEnter your phone number:");
            string EnterdPhoneNumber = Console.ReadLine().Trim();

            Console.WriteLine("\nEnter your password:");
            string EnterdPassword = Console.ReadLine().Trim();

            Console.Clear();
            Console.WriteLine("Confirm your password:");

            while (Console.ReadLine().Trim() != EnterdPassword)
            {
                Console.WriteLine("\nPasswords do not match. Try again.");
            }

            // New accounts start at zero; signup does not bypass login.
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
            loop = true;

            while (loop == true)
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
            }
        }
        private void DepositAmount()
        {
            while (loop == true)
            {
                Console.WriteLine("Enter the amount you wish to deposit:");
                string input = Console.ReadLine();
                decimal.TryParse(input, out decimal amount);

                if (amount <= 0)
                {
                    Console.WriteLine("\nEnter a non-negative amount grater then 0, in dollars and cents.");
                    loop = true;
                }
                else
                {
                    Console.WriteLine("\nYou have deposited $" + amount);
                    CurrentUser.Credit(amount);
                    ViewBalance();
                    loop = false;
                }
            }
        }
        private void Withdraw()
        {
            Console.Clear();
            Console.WriteLine("You've selected withdraw.");
            loop = true;

            while (loop == true)
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
            }
        }
        private void WithdrawAmount()
        {
            while (loop == true)
            {
                Console.WriteLine("Enter the amount you wish to withdraw:");
                string input = Console.ReadLine();
                decimal.TryParse(input, out decimal amount);
                if (amount <= 0)
                {
                    Console.WriteLine("\nEnter a non-negative amount grater then 0, in dollars and cents.");
                    loop = true;
                }
                else if (amount > CurrentUser.GetBalance())
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
            }
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
            loop = true;

            while (loop == true)
            {
                Console.WriteLine("Do you wish to logout? Enter 1 to go back or enter 2 to logout:");

                string input = Console.ReadLine();
                int.TryParse(input, out int choice);

                switch (choice)
                {
                    case 1:
                        loop = false;
                        break;
                    case 2:
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
            }
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