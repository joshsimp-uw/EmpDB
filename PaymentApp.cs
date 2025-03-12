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
    private List<IPayable> payments = new List<IPayable>();


    public void Run()
    {
      /// Start the main loop user interface
      while (true)
      {
        // first, display the mian menue
        DisplayMainMenu();
        // Get the user selection 
        char selection = GetUserSelection();
        Console.WriteLine();
        // switch for user election
        switch (char.ToLower(selection))
        {
          case 'a':
            // add a record
            AddPayableRecord();
            break;
          case 'f':
            // find a record
            FindPayableRecord();
            break;
          case 'm':
            // modify a record
            ModifyPayableRecord();
            break;

          case 'd':
            // delete a record
            DeletePayableRecord();
            break;

          case 'p':
            // print all records
            PrintAllPayableRecord();
            break;

          case 'k':
            //  print emails only
            PrintAllPayableRecordKeys();
            break;

          case 's':
            // save and exit
            SavePayableDataAndExit();
            break;
          case 'e':
            // exit wihtout saving
            // todo add confimation of exit
            Environment.Exit(0);
            break;
          default:
            Console.WriteLine($"\nERROR: {selection} is not valid input. Select again.\n");
            break;
        }
      }
    }

    private void SavePayableDataAndExit()
    {
      
    }

    private void PrintAllPayableRecordKeys()
    {
      
    }

    private void PrintAllPayableRecord()
    {
      
    }

    private void DeletePayableRecord()
    {
      
    }

    private void ModifyPayableRecord()
    {
      
    }

    private void FindPayableRecord()
    {
      
    }

    private void AddPayableRecord()
    {
      
    }

    private char GetUserSelection()
    {
      ConsoleKeyInfo key = Console.ReadKey();
      return key.KeyChar;
    }

    private void DisplayMainMenu()
    {
      
    }
    private void WriteDataToOutPutFile()
    {
      string json = JsonConvert.SerializeObject(payments, Formatting.Indented);
      File.WriteAllText(Constants.PaymentsOuputJSONFile, json);
    }

    public void PaymentDBTester()
    {
            //Populate the DB with test info and then read that info into a new List<IPayable>
            var _employees = new List<Employee>
      {
          new HourlyEmployee("Bruce", "Springsteen", "123-12-3123", 22.30M, 28.15M),
          new HourlyEmployee("Tim", "Reynolds", "141-14-1414", 40.00M, 40.00M),
          new CommissionEmployee("Dave", "Matthews", "212-21-2121", 10000M, .06M),
          new CommissionEmployee("Nita", "Strauss", "888-88-8888", 4891M, .18M),
          new SalariedEmployee("Jimi", "Hendrix", "202-20-2020", 5000M),
          new SalariedEmployee("Carlos", "Santana", "666-66-6666", 3800M),
          new BasePlusCommissionEmployee("Jimmy", "Page", "848-84-8484", 1818M, .07M, 1800M),
          new BasePlusCommissionEmployee("Jeff", "Beck", "1122-11-2211", 48984M, .02M, 3200M)
      };

            var _Invoices = new List<Invoice>
            {
                new Invoice("44811", "Gibson Guitar", 8, 4418.81M),
                new Invoice("99112", "Mini Party Hats", 16, 12.41M),
                new Invoice("84811", "Jack Daniels", 22, 21.84M),
                new Invoice("18811", "Party Cake", 1, 62.99M)
            };

    }
  }
}
