namespace EmpDB
{
  internal class Program
  {
    static void Main(string[] args)
    {
      PaymentApp paymentApp = new PaymentApp();
      paymentApp.Run();
      paymentApp.PaymentDBTester();
    }
  }
}
