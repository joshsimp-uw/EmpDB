using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSubTypes;

namespace EmpDB
{
  [JsonConverter(typeof(JsonSubtypes), "PaymentType")]
  [JsonSubtypes.KnownSubType(typeof(BasePlusCommissionEmployee), "BasePlusCommissionEmployee")]
  [JsonSubtypes.KnownSubType(typeof(SalariedEmployee), "SalariedEmployee")]
  [JsonSubtypes.KnownSubType(typeof(CommissionEmployee), "CommissionEmployee")]
  [JsonSubtypes.KnownSubType(typeof(HourlyEmployee), "HourlyEmployee")]
  [JsonSubtypes.KnownSubType(typeof(Employee), "Employee")]
  [JsonSubtypes.KnownSubType(typeof(Invoice), "Invoice")]
  
    public interface IPayable
  {
    public string PaymentType { get => GetPaymentType(); }
    decimal GetPaymentAmount();
    string GetPaymentType();
  }
}
