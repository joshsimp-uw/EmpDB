using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpDB
{
  internal class CommissionEmployee : Employee
  {
    private decimal grossSales; // gross weekly sales
    private decimal commissionRate; // commission percentage
    public override string PaymentType;


        // five-parameter constructor
        public CommissionEmployee(string firstName, string lastName,
    string socialSecurityNumber, decimal grossSales,
    decimal commissionRate) : base(firstName, lastName, socialSecurityNumber)
    {
      GrossSales = grossSales; // validate gross sales
      CommissionRate = commissionRate; // validate commission rate
      PaymentType = this.GetType().Name; // initialize paymenttype for serialization
        }

    public decimal GrossSales
    {
      get
      {
        return grossSales;
      }

      set
      {
        if (value < 0) // validation
        {
          throw new ArgumentOutOfRangeException(nameof(value),
          value, $"{nameof(GrossSales)} must be >= 0");
        }
        grossSales = value;
      }
    }

    public decimal CommissionRate
    {
      get
      {
        return commissionRate;
      }
      set
      {
        if (value <= 0 || value >= 1) // validation
        {
          throw new ArgumentOutOfRangeException(nameof(value),
          value, $"{nameof(CommissionRate)} must be > 0 and < 1");
        }
        commissionRate = value;
      }
    }

    public override decimal Earnings() => CommissionRate * GrossSales;

    public override string ToString() => $"commission employee: {base.ToString()}\n" + $"gross sales: {GrossSales:C}\n" + $"commission rate: {CommissionRate:F2}";
  }
}
