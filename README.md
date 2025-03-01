You must be in a Cs2 group for this assignment to receive a grade
Communicate with your group and put yourselves into the same group prior to turning in a submission.
Submission from all groups should be a single zip file of the entire development tree including any testing document produced.
Design, code, and test an Employee Payroll app using the Student Database app as a template.

Use a Console App - .NET Core
Database storage must use a plain text (UTF-8) file - no database software allowed
Use the Employee inheritance hierarchy data object classes from the Deitel book example - Fig12_04_09 PayrollSystem code.
Name your project and solution EmpDB - this will create the EmpDB namespace - all code should be placed in that namespace
Create at least 2 employees of each subtype for sample data in your input file - a total of 8 various employees.
Employee objects in the database app should be stored in a List<> at runtime - not in a static sized array.
Your EmpDB app should implement all 4 CRUD operations on Employee(s) in addition to processing payroll.
Processing payroll is simply a printout of all elements in the List along with the correct amount they should be paid based on current data.
The process payroll action should also print a total for the entire payroll.
Requirements for teams of 3 and 4 students also include the following:

Additional requirements for a group of 3 or 4 students:
Integrate the IPayable interface and Invoice class from Fig12_11_15 into your EmpDB app so that when you run payroll to determine how much each employee's check should be, you can also pay invoices that are due.
Invoice data can be kept in the same input file as employees and there should be at least 4 sample invoice entries that need to be paid with the payroll.
There should no longer be a List<> of Employees, but a List<> of IPayable objects.
An IPayable object could be any kind of employee that needs to be paid or an Invoice that needs to be paid.
Processing payroll should include all payable elements in the List with correct pay information for each item (whether employee or invoice) along with subtotals for pay and total invoiced costs and a grand total.
