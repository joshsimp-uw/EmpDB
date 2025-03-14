using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmpDB
{
    internal class PaymentApp
    {
        // Stores all payable items (employees + invoices)
        private List<IPayable> payments = new List<IPayable>();

        public void ReadPayableDataFromInputFile()
        {
            if (File.Exists("payments.json"))
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                payments = JsonConvert.DeserializeObject<List<IPayable>>(
                    File.ReadAllText("payments.json"), settings) ?? new List<IPayable>();
            }
        }

        public void Run()
        {

            while (true)
            {
                // First, display the main menu
                DisplayMainMenu();
                // Get the user selection
                char selection = GetUserSelection();
                Console.Clear(); // Clear console after selection

                // Switch for user selection
                switch (char.ToLower(selection))
                {
                    
                    case 'a':
                        // Add a record
                        AddPayableRecord(); 
                        break;
                    case 'f':
                        // Find a record
                        FindPayableRecord(); 
                        break;
                    case 'm':
                        // Modify a record
                        ModifyPayableRecord();
                        break;
                    case 'd':
                        // Delete a record
                        DeletePayableRecord(); 
                        break;
                    case 'p':
                        // Print all records
                        PrintAllPayableRecord(); 
                        break;
                    case 'k':
                        // Print SSN/Part Numbers only
                        PrintAllPayableRecordKeys();
                        break;
                    case 's':
                        // Save and exit
                        SavePayableDataAndExit(); 
                        break;
                    case 'e':
                        // Exit without saving
                        Environment.Exit(0); 
                        break;
                    default: 
                        Console.WriteLine("Invalid option"); 
                        break;
                }

                // Once a user selects a key, they will get their operation output on a seperate screen
                // Then the user can select any key to return to the main menu after viewing what they need
                if (char.ToLower(selection) != 's' && char.ToLower(selection) != 'e')
                {
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private void AddPayableRecord()
        {
            Console.WriteLine("\nAdd: [E]mployee/[I]nvoice");
            var choice = char.ToLower(Console.ReadKey().KeyChar);

            try
            {
                if (choice == 'e') AddEmployee();
                else if (choice == 'i') AddInvoice();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        private void AddEmployee()
        {
            // User selects which employee type they want to add
            Console.WriteLine("\nEmployee Type: [H]ourly/[S]alaried/[C]ommission/[B]ase+Commission");
            var type = char.ToLower(Console.ReadKey().KeyChar);

            // User adds First, Last Name, and SSN
            Console.Write("\nFirst Name: ");
            string first = Console.ReadLine();
            Console.Write("Last Name: ");
            string last = Console.ReadLine();
            Console.Write("SSN: ");
            string ssn = Console.ReadLine();

            switch (type)
            {
                // Hourly employee
                case 'h':
                    Console.Write("Hourly Wage: ");
                    decimal wage = decimal.Parse(Console.ReadLine());
                    Console.Write("Hours Worked: ");
                    decimal hours = decimal.Parse(Console.ReadLine());
                    payments.Add(new HourlyEmployee(first, last, ssn, wage, hours));
                    break;

                // Salaried employee
                case 's':
                    Console.Write("Weekly Salary: ");
                    decimal salary = decimal.Parse(Console.ReadLine());
                    payments.Add(new SalariedEmployee(first, last, ssn, salary));
                    break;

                // Commission employee
                case 'c':
                    Console.Write("Gross Sales: ");
                    decimal sales = decimal.Parse(Console.ReadLine());
                    Console.Write("Commission Rate: ");
                    decimal rate = decimal.Parse(Console.ReadLine());
                    payments.Add(new CommissionEmployee(first, last, ssn, sales, rate));
                    break;

                // BasePlusCommission employee
                case 'b':
                    Console.Write("Gross Sales: ");
                    decimal bSales = decimal.Parse(Console.ReadLine());
                    Console.Write("Commission Rate: ");
                    decimal bRate = decimal.Parse(Console.ReadLine());
                    Console.Write("Base Salary: ");
                    decimal bSalary = decimal.Parse(Console.ReadLine());
                    payments.Add(new BasePlusCommissionEmployee(first, last, ssn, bSales, bRate, bSalary));
                    break;
            }
        }

        private void AddInvoice()
        {
            // Part Number
            Console.Write("\nPart Number: ");
            string num = Console.ReadLine();
            // Description 
            Console.Write("Description: ");
            string desc = Console.ReadLine();
            // Number of items
            Console.Write("Quantity: ");
            int qty = int.Parse(Console.ReadLine());
            // Price per Items
            Console.Write("Price per Item: ");
            decimal price = decimal.Parse(Console.ReadLine());

            payments.Add(new Invoice(num, desc, qty, price));
        }

        private void FindPayableRecord()
        {
            // User searches for SSN or Invoice
            Console.Write("\nEnter SSN or Invoice #: ");
            var id = Console.ReadLine();

            // Serach both employees and invoices
            var found = payments.Find(p =>
                (p is Employee e && e.SocialSecurityNumber == id) ||
                (p is Invoice i && i.PartNumber == id));

            if (found != null)
            {
                // Employee or Invoice information is shown plus Amount Due
                Console.WriteLine(found);
                Console.WriteLine($"Amount Due: {found.GetPaymentAmount():C}");
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }

        private void ModifyPayableRecord()
        {
            // User selects SSN or Invoice to modify
            Console.Write("Enter SSN (employees) or Part# (invoices) to modify: ");
            string identifier = Console.ReadLine();

            IPayable payable = payments.FirstOrDefault(p =>
                (p is Employee e && e.SocialSecurityNumber == identifier) ||
                (p is Invoice i && i.PartNumber == identifier));

            if (payable == null)
            {
                Console.WriteLine("Record not found.");
                return;
            }

            try
            {
                switch (payable)
                {
                    case BasePlusCommissionEmployee basePlus:
                        ModifyBasePlusCommissionEmployee(basePlus);
                        break;

                    case HourlyEmployee hourly:
                        ModifyHourlyEmployee(hourly);
                        break;

                    case SalariedEmployee salaried:
                        ModifySalariedEmployee(salaried);
                        break;

                    case CommissionEmployee commission:
                        ModifyCommissionEmployee(commission);
                        break;

                    case Invoice invoice:
                        ModifyInvoice(invoice);
                        break;

                    default:
                        Console.WriteLine("Modification not supported for this type.");
                        break;
                }
                Console.WriteLine("Record updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Modification failed: {ex.Message}");
            }
        }

        private void ModifyHourlyEmployee(HourlyEmployee emp)
        {
            Console.Write($"Current wage ({emp.Wage:C}/hr) - New wage (Leave blank and press 'Enter' to keep): ");
            if (Decimal.TryParse(Console.ReadLine(), out decimal newWage) && newWage > 0)
                emp.Wage = newWage;

            Console.Write($"Current hours ({emp.Hours}) - New hours (Leave blank and press 'Enter' to keep): ");
            if (Decimal.TryParse(Console.ReadLine(), out decimal newHours) && newHours >= 0)
                emp.Hours = newHours;
        }

        private void ModifySalariedEmployee(SalariedEmployee emp)
        {
            Console.Write($"Current salary ({emp.WeeklySalary:C}) - New salary (Leave blank and press 'Enter' to keep): ");
            if (Decimal.TryParse(Console.ReadLine(), out decimal newSalary) && newSalary > 0)
                emp.WeeklySalary = newSalary;
        }

        private void ModifyCommissionEmployee(CommissionEmployee emp)
        {
            Console.Write($"Current sales ({emp.GrossSales:C}) - New sales (Leave blank and press 'Enter' to keep): ");
            if (Decimal.TryParse(Console.ReadLine(), out decimal newSales) && newSales >= 0)
                emp.GrossSales = newSales;

            Console.Write($"Current rate ({emp.CommissionRate:P}) - New rate (Leave blank and press 'Enter' to keep): ");
            if (Decimal.TryParse(Console.ReadLine(), out decimal newRate) && newRate > 0 && newRate < 1)
                emp.CommissionRate = newRate;
        }

        private void ModifyBasePlusCommissionEmployee(BasePlusCommissionEmployee emp)
        {
            Console.Write($"Current base salary ({emp.BaseSalary:C}) - New base (Leave blank and press 'Enter' to keep): ");
            if (Decimal.TryParse(Console.ReadLine(), out decimal newBase) && newBase > 0)
                emp.BaseSalary = newBase;

            ModifyCommissionEmployee(emp); // Reuse commission employee modification
        }

        private void ModifyInvoice(Invoice inv)
        {
            Console.Write($"Current quantity ({inv.Quantity}) - New quantity (Leave blank and press 'Enter' to keep): ");
            if (Int32.TryParse(Console.ReadLine(), out int newQty) && newQty >= 0)
                inv.Quantity = newQty;

            Console.Write($"Current price ({inv.PricePerItem:C}/item) - New price (Leave blank and press 'Enter' to keep): ");
            if (Decimal.TryParse(Console.ReadLine(), out decimal newPrice) && newPrice >= 0)
                inv.PricePerItem = newPrice;
        }

        private void DeletePayableRecord()
        {
            // User selects SSN or Invoice # to delete
            Console.Write("\nEnter SSN or Invoice Part Number # to delete: ");
            var id = Console.ReadLine();

            var item = payments.Find(p =>
                (p is Employee e && e.SocialSecurityNumber == id) ||
                (p is Invoice i && i.PartNumber == id));

            if (item != null)
            {
                payments.Remove(item);
                Console.WriteLine("Item removed");
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }

        private void PrintAllPayableRecord()
        {
           // Print all Employee Types/Invoices and also shows the total

            decimal empTotal = 0, invTotal = 0;

            Console.WriteLine("\nPAYROLL PROCESSING");
            Console.WriteLine("==================");

            foreach (var payable in payments)
            {
                Console.WriteLine(payable);
                Console.WriteLine($"Amount Due: {payable.GetPaymentAmount():C}\n");

                if (payable is Employee) empTotal += payable.GetPaymentAmount();
                else if (payable is Invoice) invTotal += payable.GetPaymentAmount();
            }

            Console.WriteLine($"Employee Total: {empTotal:C}");
            Console.WriteLine($"Invoice Total: {invTotal:C}");
            Console.WriteLine($"GRAND TOTAL: {empTotal + invTotal:C}");
        }

        private void PrintAllPayableRecordKeys()
        {
            // Prints all SSN and Part Numbers
            Console.WriteLine("\nAll Payment Keys:");
            Console.WriteLine("-----------------");

            foreach (var payable in payments)
            {
                if (payable is Employee e)
                {
                    Console.WriteLine($"Employee SSN: {e.SocialSecurityNumber}");
                }
                else if (payable is Invoice i)
                {
                    Console.WriteLine($"Invoice Part#: {i.PartNumber}");
                }
            }
        }

        private void SavePayableDataAndExit()
        {
            // Saves Data to JSON then exits
            Console.WriteLine("\n Saving data and exiting.");
            WriteDataToOutputFile();
            Environment.Exit(0);
        }

        private char GetUserSelection()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            return key.KeyChar;
        }

        private void DisplayMainMenu()
        {
            string menu = string.Empty;
            menu += "\n";
            menu += "******************************************\n";
            menu += "*********** Payment Database App *********\n";
            menu += "******************************************\n";
            menu += "[A]dd a payable record        (C in CRUD - Create)\n";
            menu += "[F]ind a payable record       (R in CRUD - Read)\n";
            menu += "[M]odify a payable record     (U in CRUD - Update)\n";
            menu += "[D]elete a payable record     (D in CRUD - Delete)\n";
            menu += "[P]rint all payable in current db storage\n";
            menu += "Print all primary [K]eys (SSN/Part Numbers)\n";
            menu += "[S]ave data to file and exit app\n";
            menu += "[E]xit app without saving changes\n";
            menu += "\n";
            menu += "User Key Selection: ";

            Console.Write(menu);
        }

        private void WriteDataToOutputFile()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            File.WriteAllText("payments.json", JsonConvert.SerializeObject(payments, settings));
        }

        public void PaymentDBTester()
        {
            payments.AddRange(new IPayable[] {
                // 2 of each employee type
                  new HourlyEmployee("Bruce", "Springsteen", "123-12-3123", 22.30M, 28.15M),
                  new HourlyEmployee("Tim", "Reynolds", "141-14-1414", 40.00M, 40.00M),
                  new CommissionEmployee("Dave", "Matthews", "212-21-2121", 10000M, .06M),
                  new CommissionEmployee("Nita", "Strauss", "888-88-8888", 4891M, .18M),
                  new SalariedEmployee("Jimi", "Hendrix", "202-20-2020", 5000M),
                  new SalariedEmployee("Carlos", "Santana", "666-66-6666", 3800M),
                  new BasePlusCommissionEmployee("Jimmy", "Page", "848-84-8484", 1818M, .07M, 1800M),
                  new BasePlusCommissionEmployee("Jeff", "Beck", "1122-11-2211", 48984M, .02M, 3200M),
                
                // 4 invoices
                new Invoice("44811", "Gibson Guitar", 8, 4418.81M),
                new Invoice("99112", "Mini Party Hats", 16, 12.41M),
                new Invoice("84811", "Jack Daniels", 22, 21.84M),
                new Invoice("18811", "Party Cake", 1, 62.99M)
            });

            Console.WriteLine("Test data loaded successfully");
        }
    }
}
