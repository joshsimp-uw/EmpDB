namespace EmpDB
{
  internal class SalariedEmployee : Employee
  {
    private decimal weeklySalary;
    public string PaymentType;


        // four-parameter constructor
        public SalariedEmployee(string firstName, string lastName,
        string socialSecurityNumber, decimal weeklySalary)
      : base(firstName, lastName, socialSecurityNumber)
    {
      WeeklySalary = weeklySalary; // validate salary
      PaymentType = this.GetType().Name; // initialize paymenttype for serialization
    }

    // property that gets and sets salaried employee's salary
    public decimal WeeklySalary
    {
      get
      {
        return weeklySalary;
      }
      set
      {
        if (value < 0) // validation
        {
          throw new ArgumentOutOfRangeException(nameof(value),
          value, $"{nameof(WeeklySalary)} must be >= 0");
        }

        weeklySalary = value;
      }
    }

    // calculate earnings; override abstract method Earnings in Employee
    public override decimal Earnings() => WeeklySalary;

    // return string representation of SalariedEmployee object
    public override string ToString() => $"salaried employee: {base.ToString()}\n" + $"weekly salary: {WeeklySalary:C}";
  }
}
