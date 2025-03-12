namespace EmpDB
{
  public abstract class Employee
  {
    public string FirstName { get; }
    public string LastName { get; }
    public string SocialSecurityNumber { get; }

    
    public Employee(string firstName, string lastName, string socialSecurityNumber)
    {
      FirstName = firstName;
      LastName = lastName;
      SocialSecurityNumber = socialSecurityNumber;
    }

    // return string representation of Employee object, using properties
    public override string ToString() => $"{FirstName} {LastName}\n" + $"social security number: {SocialSecurityNumber}";

    // abstract method overridden by derived classes
    // no implementation here
    public abstract decimal Earnings();


    
    
  }


}
