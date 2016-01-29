using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        private const int EXIT_NUMBER = 7;
        public static void Start(List<User> usrs, List<Product> prods)
        {
            Login loginUser = new Login(usrs);
            showWelcomeMessage();

            Login:

            string username = promptUserInputForUsername();
            if (!String.IsNullOrEmpty(username))
            {
                if (loginUser.validateUsername(username))
                {
                    if (loginUser.validatePassword(promptUserInputForPassword()))
                    {
                        User currentUser = loginUser.getValidUser();

                        showMessage("Login successful! Welcome " + username + "!", ConsoleColor.Green);

                        showTusc(usrs, prods, currentUser);
                        return;
                    }
                    else
                    {
                        showMessage("You entered an invalid password.", ConsoleColor.Red);
                        goto Login;
                    }
                }
                else
                {
                    showMessage("You entered an invalid user.", ConsoleColor.Red);
                    goto Login;
                }
            }

            promptExitMessage();
        }

        private static void showTusc(List<User> usrs, List<Product> prods, User currentUser)
        {
            double balance = currentUser.Bal;
            showRemainingBalanceMessage(balance);

            while (true)
            {
                showProductList(prods);
                int productNumber = getUserInput("Enter a number:") - 1;

                if (productNumber == EXIT_NUMBER)
                {
                    currentUser.Bal = balance;
                    saveBalance(usrs);
                    saveProductQuantity(prods);
                    promptExitMessage();
                    return;
                }
                else if (productNumber < EXIT_NUMBER)
                {
                    balance = purchaseProcess(prods, balance, productNumber);
                }
            }
        }

        private static double purchaseProcess(List<Product> prods, double balance, int productNumber)
        {
            //refactor to maybe in a class
            Product currentProductToPurchase = prods[productNumber];

            showCurrentProductUserWantsToPurcahseMessage(currentProductToPurchase, balance);
            int quantityAmountToPurchase = getUserInput("Enter amount to purchase:");

            if (canUserAffordProduct(balance, currentProductToPurchase, quantityAmountToPurchase))
            {
                showMessage("You do not have enough money to buy that.", ConsoleColor.Red);
            }
            else if (isProductInStock(currentProductToPurchase, quantityAmountToPurchase))
            {
                String noProductInStockMessage = "Sorry, " + prods[productNumber].Name + " is out of stock";
                showMessage(noProductInStockMessage, ConsoleColor.Red);
            }
            else if (quantityAmountToPurchase > 0)
            {
                balance = updateBalance(balance, currentProductToPurchase, quantityAmountToPurchase);
                updateProductStock(currentProductToPurchase, quantityAmountToPurchase);
                showProductPurchasedMessage(currentProductToPurchase, balance, quantityAmountToPurchase);
            }
            else
            {
                showMessage("Purchase cancelled", ConsoleColor.Yellow);
            }
            return balance;
        }

        private static void updateProductStock(Product currentProductToPurchase, int quantityAmountToPurchase)
        {
            currentProductToPurchase.Qty = currentProductToPurchase.Qty - quantityAmountToPurchase;
        }

        private static double updateBalance(double balance, Product currentProductToPurchase, int quantityAmountToPurchase)
        {
            balance = balance - currentProductToPurchase.Price * quantityAmountToPurchase;
            return balance;
        }

        private static bool isProductInStock(Product currentProductToPurchase, int quantityAmountToPurchase)
        {
            return currentProductToPurchase.Qty <= quantityAmountToPurchase;
        }

        private static bool canUserAffordProduct(double balance, Product currentProductToPurchase, int quantityAmountToPurchase)
        {
            return balance - currentProductToPurchase.Price * quantityAmountToPurchase < 0;
        }

        private static void showProductPurchasedMessage(Product product, double balance, int quantityAmountToPurchase)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You bought " + quantityAmountToPurchase + " " + product.Name);
            Console.WriteLine("Your new balance is " + balance.ToString("C"));
            Console.ResetColor();
        }

        private static void showCurrentProductUserWantsToPurcahseMessage(Product product, double balance)
        {
            Console.WriteLine();
            Console.WriteLine("You want to buy: " + product.Name);
            Console.WriteLine("Your balance is " + balance.ToString("C"));
        }

        private static void showProductList(List<Product> prods)
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");
            int productNumber = 1;
            foreach (Product product in prods)
            {
                Console.WriteLine(productNumber + ": " + product.Name + " (" + product.Price.ToString("C") + ")");
                productNumber++;
            }

            Console.WriteLine(prods.Count + 1 + ": Exit");
        }

        private static int getUserInput(String message)
        {
            Console.WriteLine(message);
            string result = Console.ReadLine();
            return Convert.ToInt32(result);
        }


        private static void saveProductQuantity(List<Product> prods)
        {
            string json2 = JsonConvert.SerializeObject(prods, Formatting.Indented);
            File.WriteAllText(@"Data/Products.json", json2);
        }

        private static void saveBalance(List<User> usrs)
        {
            string json = JsonConvert.SerializeObject(usrs, Formatting.Indented);
            File.WriteAllText(@"Data/Users.json", json);
        }

        private static void showRemainingBalanceMessage(double balance)
        {
            Console.WriteLine();
            Console.WriteLine("Your balance is " + balance.ToString("C"));
        }

        private static string promptUserInputForPassword()
        {
            Console.WriteLine("Enter Password:");
            string password = Console.ReadLine();
            return password;
        }

        private static string promptUserInputForUsername()
        {
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            string name = Console.ReadLine();
            return name;
        }

        private static void showWelcomeMessage()
        {
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }

        private static void promptExitMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        private static void showMessage(String message, ConsoleColor color) {
            Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
