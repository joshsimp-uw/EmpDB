using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpDB
{
  internal class BasePlusCommissionEmployee : CommissionEmployee
  {
    private decimal baseSalary; // base salary per week
    public override string PaymentType;
    
    // six-parameter constructor
    public BasePlusCommissionEmployee(string firstName, string lastName,
    string socialSecurityNumber, decimal grossSales,
    decimal commissionRate, decimal baseSalary)
      : base(firstName, lastName, socialSecurityNumber, grossSales, commissionRate)
    {
      BaseSalary = baseSalary; // validate base salary
      PaymentType = this.GetType().Name; // initialize paymenttype for serialization
    }

    // property that gets and sets base-salaried commission employee's base salary
    public decimal BaseSalary
    {
      get
      {
        return baseSalary;
      }
      set
      {
        if (value < 0) // validation
        {
          throw new ArgumentOutOfRangeException(nameof(value),
          value, $"{nameof(BaseSalary)} must be >= 0");
        }
        baseSalary = value;
      }
    }
    public override decimal Earnings() => BaseSalary + base.Earnings();
    public override string ToString() => $"base-salaried {base.ToString()}\n" + $"base salary: {BaseSalary:C}";
  }
}
