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
    }
    public class Bank
    {
        private readonly List<UserData> users = new List<UserData>();
        private UserData CurrentUser;
        private const int MaxAttempts = 3;
        private bool loop = false;
        public Bank()
        {
            users.Add(new UserData("Joe.Doe", "joe@example.com", 24, "0400000000", "Password123", 100));
        }
        public void Menus()
        {
            Console.Clear();

            while (true)
            {
                if (CurrentUser == null)
                {
                    Console.WriteLine("Welcome to the Bank");
                    Console.WriteLine("Enter 1 to Login. Enter 2 to Signup. Enter 3 to Quit:");
                    string input = Console.ReadLine();

                    int.TryParse(input, out int choice);
                    switch (choice)
                    {
                        case 1: Login(); break;
                        case 2: Signup(); break;
                        case 3: Quit(); break;
                        default: Console.WriteLine("\nInvalid input. Only input 1, 2 or 3."); break;
                    }
                }
                else
                {
                    Console.WriteLine("Welcome" + CurrentUser.Name);
                    Console.WriteLine("Enter 1 to Deposit. Enter 2 to Withdraw. Enter 3 to View Balance. Enter 4 to Logout. Enter 5 to Quit:");
                    string input = Console.ReadLine();

                    int.TryParse(input, out int choice);
                    switch (choice)
                    {
                       case 1:
                       case 2:
                       case 3:
                       case 4: Logout(); break;

                       case 5: Quit(); break;

                       default: Console.WriteLine("\nInvalid input. Only input 1, 2, 3, 4 or 5."); break;
                    }
                }
            }
        }

        private void Login()
        {
            Console.Clear();

            Console.WriteLine("You've selected login.");

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
                        LoginMenu();
                        loop = false;
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

            for (int Attempt = 1; Attempt <= MaxAttempts; Attempt++)
            {
                Console.WriteLine("Enter your username:");
                string AttemptUsername = Console.ReadLine().Trim();

                Console.WriteLine("\nEnter your password:");
                string AttemptPassword = Console.ReadLine().Trim();

                if (LoginCheck(AttemptUsername, AttemptPassword))
                {
                    Console.WriteLine("\nLogin successful.");
                }
                else
                {
                    Console.WriteLine("\nInvalid username or password. Attempts left: " + (MaxAttempts - Attempt));
                }
            }
        }
        private void Signup()
        {
            Console.Clear();

            Console.WriteLine("Enter your name:");
            string EnterdUsername = Console.ReadLine().Trim();

            Console.WriteLine("Enter your email:");
            string EnterdEmail = Console.ReadLine().Trim();

            Console.WriteLine("Enter your age:");
            string input = Console.ReadLine();
            int.TryParse(input, out int EnterdAge);

            Console.WriteLine("Enter your phone number:");
            string EnterdPhoneNumber = Console.ReadLine().Trim();

            Console.WriteLine("Enter your password:");
            string EnterdPassword = Console.ReadLine().Trim();

            while (Console.ReadLine().Trim() != EnterdPassword)
            {

            }
                Console.WriteLine("Passwords do not match. Try again.");

            // New accounts start at zero; signup does not bypass login.
            users.Add(new UserData(EnterdUsername, EnterdEmail, EnterdAge, EnterdPhoneNumber, EnterdPassword, 0));
            LoginCheck(EnterdUsername, EnterdPassword);

            Console.WriteLine("You have successfully created an account. Press enter to proceed:");
            Console.ReadLine();
        }
        private void Deposit()
        {
            Console.Clear();

        }
        private void Withdraw()
        {
            Console.Clear();

        }
        private void ViewBalance()
        {
            Console.Clear();

        }
        private void Logout()
        {
            Console.Clear();

            Console.WriteLine("You've selected logout.");

            while(loop == true)
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
                    System.Environment.Exit(0);
                    break;
                case 2:
                    break;
                default:
                    Console.WriteLine("\nInvalid input. Only input 1 or 2.");
                    loop = true;
                    break;
            }
        }

        LogIn:      // Section by Brandon Hooper

        do
        {
            Console.Clear();                                                                                                // This clears all previous text.
            input = 0;

            if (LogedIn == true)                                                                                            // This checks if the user has already logedin.
            {
                Console.WriteLine("You have already loged in. Press enter to go back to Menu:");
                Console.ReadLine();                                                                                         // This gives the user time to read with out a time limit, while
                                                                                                                            // leting them progress when thay wish.

                goto LogedInMenu;                                                                                           // this sends the user back to the LogedInMenu.
            }
            else if (attempts < 0)                                                                                          // If the user has exhausted ther attempts.
            {
                Console.WriteLine("You do not have any attempts left:");
                Console.ReadLine();
                goto LogedOutMenu;
            }
            else;

            if (!LoopOccured)
            {
                Console.WriteLine("You've selected Login. You have " + attempts + " attempts to login.");                   // This gives feed back to show the user what option thay picked.
                Console.WriteLine("");
            }
            else if (attempts > 0)
            {
                Console.WriteLine("Your attempt to login has falled. You have " + attempts + " attempts remaining.");       // This gives feed back to show the user how meny attempts
                Console.WriteLine("");                                                                                      // thay have remaining.
            }
            else;

            if (LoopOccured == true && InputEmail.SequenceEqual(UserEmail))                                                 // These if else statments shows what part of the attempt
            {                                                                                                               // was invalid.
                Console.WriteLine("Invalid Email.");
                Console.WriteLine("");
            }
            else;                                                                                                           // I do know that these else; statments are redundant but it is
                                                                                                                            // there redundancy which is why thay are here.
            if (LoopOccured == true && InputName.SequenceEqual(UserName))
            {
                Console.WriteLine("Invalid Name.");
                Console.WriteLine("");
            }
            else;

            if (LoopOccured == true && InputPassword.SequenceEqual(UserPassword))
            {
                Console.WriteLine("Invalid Password.");
                Console.WriteLine("");
            }
            else;

        inputEmail:                                                                                                         // This is a flag to retern to if needed.

            input = 0;                                                                                                      // This resets input

            Console.WriteLine("Enter any number to processed or 1 to go back:");                                            // This is a section here to give the option to go back. While I
                                                                                                                            // wanted it to be so the user presses enter to processed
                                                                                                                            // aperantly that conflicts with the int used.
            input = int.Parse(Console.ReadLine());

            if (input == 1)
            {
                goto LogedOutMenu;                                                                                          // When 1 is inputed it uses this goto to send the uses to
            }                                                                                                               // the manu.
            else;

            Console.WriteLine("");
            Console.WriteLine("Enter user email:");                                                                         // I personaly dont belive that the email is nesasery to login
                                                                                                                            // but the bref says other wise so here it is.

            InputEmail = (Console.ReadLine().ToCharArray());                                                                // This is where the user inputs there Email

            //AreEqual = InputEmail.SequenceEqual(UserEmail);                                                               // This is code turnd to coments ment for debuging
            //Console.WriteLine(AreEqual);
            //Console.ReadLine();

        inputName:

            input = 0;                                                                                                      // This is where the go back code is repeated with a new flag.

            Console.WriteLine("");
            Console.WriteLine("Enter any number to processed or 1 to go back:");
            input = int.Parse(Console.ReadLine());

            if (input == 1)
            {
                goto inputEmail;
            }
            else;

            Console.WriteLine("");
            Console.WriteLine("Enter user Neme:");                                                                          // This is where the user inputs there Neme. not to necessary
            InputName = (Console.ReadLine().ToCharArray());                                                                 // but not to unnecessary.

            //AreEqual = InputName.SequenceEqual(UserName);
            //Console.WriteLine(AreEqual);
            //Console.ReadLine();

            input = 0;                                                                                                      // This is where the go back code is repeated with a new flag.

            Console.WriteLine("");
            Console.WriteLine("Enter any number to processed or 1 to go back:");
            input = int.Parse(Console.ReadLine());

            if (input == 1)
            {
                goto inputName;
            }
            else;

        InputPassword:

            Console.WriteLine("");
            Console.WriteLine("Enter user password:");                                                                      // This is where the user inputs there Password.
            InputPassword = (Console.ReadLine().ToCharArray());

            //AreEqual = InputPassword.SequenceEqual(UserPassword);
            //Console.WriteLine(AreEqual);
            //Console.ReadLine();

            input = 0;

            Console.WriteLine("");
            Console.WriteLine("Enter any number to processed or 1 to go back.");
            input = int.Parse(Console.ReadLine());

            if (input == 1)
            {
                goto InputPassword;
            }
            else;

            AreEqual = (InputEmail.SequenceEqual(UserEmail) && InputName.SequenceEqual(UserName) && InputPassword.SequenceEqual(UserPassword)); // This checks if all of the inputs
                                                                                                                                                // are valid.
            LoopOccured = true;

            --attempts;                                                                                                     // This incraments the attempts down at the end.

        } while (!AreEqual);                                                                                                // If all of the inputs are not valid.

        LoopOccured = false;                                                                                                // these reset the values here.
        AreEqual = false;
        LogedIn = true;

        Console.WriteLine("");
        Console.WriteLine("You have sucsesfuly loged in. Press enter to return to the menu:");                              // This gives feed back telling the user thay have logedin.

        Console.ReadLine();                                                                                                 // This lets the user contenue only when thay want to.
        goto LogedInMenu;

    SignUp:     // Section by Brandon Hooper

        Console.Clear();                                                                                                    // This clears the preveuse text.
        input = 0;

        if (LogedIn == true)                                                                                                // This checks if the user has loged in.
        {
            Console.WriteLine("You have already Signed up. Press enter to return to the menu:");
            Console.ReadLine();                                                                                             // This gives the user time to read with out a time limit, while
                                                                                                                            // leting them progress when thay wish.
            goto LogedOutMenu;
        }
        else;

        Console.WriteLine("You've selected signup.");

        NewEmail:

        input = 0;                                                                                                          // This is where the go back code is repeated with a new flag.

        Console.WriteLine("");
        Console.WriteLine("Enter any number to processed or 1 to go back:");
        input = int.Parse(Console.ReadLine());

        if (input == 1)
        {
            goto LogedOutMenu;
        }
        else;

        Console.WriteLine("");
        Console.WriteLine("Enter user email:");
        UserEmail = (Console.ReadLine().ToCharArray());                                                                     // This is the input for email.

        NewPhoneNumber:

        input = 0;                                                                                                          // This is where the go back code is repeated with a new flag.

        Console.WriteLine("");
        Console.WriteLine("Enter any number to processed or 1 to go back:");
        input = int.Parse(Console.ReadLine());

        if (input == 1)
        {
            goto NewEmail;
        }
        else;

        Console.WriteLine("");
        Console.WriteLine("Enter user PhoneNumber:");
        UserPhoneNumber = (Console.ReadLine().ToCharArray());

        NewName:

        input = 0;                                                                                                          // This is where the go back code is repeated with a new flag.

        Console.WriteLine("");
        Console.WriteLine("Enter any number to processed or 1 to go back:");
        input = int.Parse(Console.ReadLine());

        if (input == 1)
        {
            goto NewPhoneNumber;
        }
        else;

        Console.WriteLine("");
        Console.WriteLine("Enter UserNeme:");
        UserName = (Console.ReadLine().ToCharArray());                                                                      // This is the input for Neme.

        NewAge:

        input = 0;                                                                                                          // This is where the go back code is repeated with a new flag.

        Console.WriteLine("");
        Console.WriteLine("Enter any number to processed or 1 to go back:");
        input = int.Parse(Console.ReadLine());

        if (input == 1)
        {
            goto NewName;
        }
        else;

        Console.WriteLine("");
        Console.WriteLine("Enter user Age:");
        UserAge = int.Parse(Console.ReadLine());

        NewPasswaod:                                                                                                        // This is where the go back code is repeated with a new flag.

        input = 0;

        Console.WriteLine("");
        Console.WriteLine("Enter any number to processed or 1 to go back:");
        input = int.Parse(Console.ReadLine());

        if (input == 1)
        {
            goto NewAge;
        }
        else;

        Console.WriteLine("");
        Console.WriteLine("Enter user password:");
        UserPassword = (Console.ReadLine().ToCharArray());                                                                  // This is the input for password.

        input = 0;                                                                                                          // This is where the go back code is repeated with a new flag.

        Console.WriteLine("");
        Console.WriteLine("Enter any number to processed or 1 to go back:");
        input = int.Parse(Console.ReadLine());

        if (input == 1)
        {
            goto NewPasswaod;
        }
        else;

        do                                                                                                                  // This is the password check section
        {
            Console.Clear();

            if (LoopOccured == true)
            {
            Console.WriteLine("Invalid.");
            Console.WriteLine("");
            }
            else;

            input = 0;                                                                                                      // This is where the go back code is repeated with a new flag.

            Console.WriteLine("Enter any number to processed or 1 to go back:");
            input = int.Parse(Console.ReadLine());

            if (input == 1)
            {
                goto NewPasswaod;
            }
            else;

            Console.WriteLine("");
            Console.WriteLine("Conform user password:");
            char[] PasswordCheck = (Console.ReadLine().ToCharArray());                                                      // This is to check the wether the user can remaber there password.

            AreEqual = PasswordCheck.SequenceEqual(UserPassword);                                                           // This checks if the user enterd the exact.
            LoopOccured = true;

        } while (AreEqual == false);                                                                                        // This loops if the password does not match.

        attempts = 3;
        LoopOccured = false;
        LogedIn = true;

        Console.WriteLine("");
        Console.WriteLine("You have sucsesfuly Signed up. Press enter to return to the menu:");
        Console.ReadLine();                                                                                                 // This lets the user contenue only when thay want to.
        goto LogedInMenu;
        


    Deposit:    // Section by Brandon Hooper

        do
        {
            Console.Clear();                                                                                                // This clears all previous text.
            input = 0;

            if (LoopOccured)
            {
                Console.WriteLine("Invalid");                                                                               // This wrights when a loop has occured.
                Console.WriteLine("");
            }
            else;
            {
                Console.WriteLine("You've selected deposit.");
                Console.WriteLine("");
            }

            Console.WriteLine("Enter any number to processed or 1 to go back:");                                            // This lets the user go back to the main menu with out needing
            input = int.Parse(Console.ReadLine());                                                                          // to processed.

            if (input == 1)
            {
                goto LogedInMenu;                                                                                           // This sends the user back to the menu when read.
            }
            else;

            input = 0;

            Console.WriteLine("");
            Console.WriteLine("Enter the amount you want to deposit:");
            input = int.Parse(Console.ReadLine());                                                                          // This is the input for how much the user wishes to deposit.

            LoopOccured = true;

        } while (input <= 0);                                                                                                // This loops this section

        BankBalance = BankBalance + input;                                                                                  // This adds the input to the bank balance.

        Console.WriteLine("You have deposited $" + input + ". Your bank balance is $" + BankBalance + ".");                 // This desplays the user input and bank balance.
        Console.WriteLine("Press enter to return to the menu:");
        Console.ReadLine();

        goto LogedInMenu;

    Withdraw:   // Section by Brandon Hooper

        do
        {
            Console.Clear();                                                                                                // This clears all previous text.
            input = 0;

            if (LoopOccured)                                                                                                
            {
                Console.WriteLine("Invalid");                                                                               // This wrights when a loop has occured.
                Console.WriteLine("");
            }
            else;
            {
                Console.WriteLine("You've selected withdraw.");
                Console.WriteLine("");
            }

            Console.WriteLine("Enter any number to processed or 1 to go back:");                                            // This lets the user go back to the main menu with out needing
            input = int.Parse(Console.ReadLine());                                                                          // to processed.

            if (input == 1)
            {
                goto LogedInMenu;                                                                                           // This sends the user back to the menu when read.
            }
            else;

            input = 0;

            Console.WriteLine("");
            Console.WriteLine("Enter the amount you want to withdraw:");
            input = int.Parse(Console.ReadLine());                                                                          // This is the input for how much the user wishes to deposit.

            LoopOccured = true;

        } while (input <= 0 || input > BankBalance);                                                                        // This loops the section if input is an invalis value.

        BankBalance = BankBalance - input;                                                                                  // This adds the input to the bank balance.

        Console.WriteLine("You have withdraw $" + input + ". Your bank balance is $" + BankBalance + ".");
        Console.WriteLine("Press enter to return to the menu:");
        Console.ReadLine();

        goto LogedInMenu;

    ViewBalance:    // Section by Brandon Hooper

        Console.WriteLine("You've selected view balance.");
        Console.WriteLine("");

        Console.WriteLine("Your account balance is " + BankBalance + ". Press enter to return to the menu:");               // This show the user there Bank Balance with out needing to go thrught the other options
        Console.ReadLine();

        goto LogedInMenu;
    }
}