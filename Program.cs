namespace EmpDB
{
  class Program
  {
    static void Main(string[] args)
    {
      PaymentApp paymentApp = new PaymentApp();

     // Load sample data
     //paymentApp.PaymentDBTester();
     // OR
     // Load Data
     paymentApp.ReadPayableDataFromInputFile();


     // Start the application
     paymentApp.Run();
      
    }
  }
}
