using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EmpDB
{
  internal class DbApp
  {
    private List<IPayable> payments = new List<IPayable>();

    public void ReadPaymentDataFromInputFileCSV()
    {
      string inputFileName = "payments.csv";

      try
      {
        var lines = File.ReadAllLines(inputFileName);

        foreach (var line in lines.Skip(1))
        {
          var fields = line.Split(',');
          string firstMidName = fields[0].Trim();
          string lastName = fields[1].Trim();
          double gradePtAvg;


          if (double.TryParse(fields[2].Trim(), out gradePtAvg))
          {
            string emailAddress = fields[3].Trim();
            Student student = new Student(firstMidName, lastName, gradePtAvg, emailAddress);
            students.Add(student);
          }
          else
          {
            Console.WriteLine($"Invalid GPA value in line : {line}");
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"ERROR reading file: {ex.Message}");
      }
    }

    public void ReadStudentDataFromInputFile()
    {
      string inputFileName = "students.json";
      try
      {
        // read the JSON file as a string and deserialize it back
        // into the Student sub-type objects and place them into
        // the students list
        string jsonFromFile = File.ReadAllText(Constants.StudentOuputJSONFile);
        // null coalesce
        students = JsonConvert.DeserializeObject<List<Student>>(jsonFromFile) ?? new List<Student>();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"ERROR reading file: {ex.Message}");
      }
    }

    public void RunDatabaseApp()
    {
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
            AddStudentRecord();
            break;
          case 'f':
            // find a record
            FindStudentRecord();
            break;
          case 'm':
            // modify a record
            ModifyStudentRecord();
            break;

          case 'd':
            // delete a record
            DeleteStudentRecord();
            break;

          case 'p':
            // print all records
            PrintAllStudentRecord();
            break;

          case 'k':
            //  print emails only
            PrintAllStudentRecordKeys();
            break;

          case 's':
            // save and exit
            SaveStudentDataAndExit();
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

    private Employee? CheckIfSsnExists(string email)
    {
      var student = students.FirstOrDefault(student => student.EmailAddress == email);

      if (student == null)
      {
        // did not find a record
        Console.WriteLine($"{email} NOT FOUND.\n");
        return null;
      }
      else
      {
        // found a record
        Console.WriteLine($"FOUND email address: {email}\n");
        return student;
      }
    }

    private void AddPaymentRecord()
    {
      // first search if user already exists
      Console.Write("ENTER the email to add: ");
      string email = Console.ReadLine();
      Student stu = CheckIfEmailExists(email);
      if (stu == null)
      {
        // Record was NOT FOUND -- go ahead and add
        Console.WriteLine($"Adding new student w/ Email: {email}");

        // Gather initial student data
        // get the first and middle name
        Console.Write("ENTER First and Middle name: ");
        string first = Console.ReadLine();

        // get the last name
        Console.Write("ENTER Last name: ");
        string last = Console.ReadLine();

        // get the GPA
        Console.Write("ENTER Grade point average: ");
        double gpa = double.Parse(Console.ReadLine());

        // We already have the email, but need to ask if the student
        // is an undergrad or grad student
        Console.Write("[U]ndergrad or [G]raduate student: ");
        string studentType = Console.ReadLine();

        // branch out to student type
        if (studentType.ToLower() == "u")
        {
          // get the rank
          Console.WriteLine("[1] Freshman, [2] Sophomore, [3] Junior, [4] Senior: ");
          Console.Write("ENTER Year rank in school from above choices: ");
          YearRank rank = (YearRank)int.Parse(Console.ReadLine());

          Console.Write("Enter major degree program: ");
          string major = Console.ReadLine();

          Undergrad undergrad = new Undergrad(firstMidName: first,
                                              lastName: last,
                                              gradePtAvg: gpa,
                                              emailAddress: email,
                                              rank: rank,
                                              degreeMajor: major);
          students.Add(undergrad);
        }

        // Graduate Student
        if (studentType.ToLower() == "g")
        {
          // get the rank
          Console.WriteLine("ENTER the tuition credit for this student (no commas): $");
          decimal credit = decimal.Parse(Console.ReadLine());

          Console.Write("Enter full name of faculty advisor: ");
          string advisor = Console.ReadLine();

          Graduate graduate = new Graduate(firstMidName: first,
                                              lastName: last,
                                              gradePtAvg: gpa,
                                              emailAddress: email,
                                              tuitionCredit: credit,
                                              facultyAdvisor: advisor);
          students.Add(graduate);
        }

      }
      else
      {
        // Record was FOUND -- do not add
        Console.WriteLine($"RECORD FOUND! Can't add student {email}.");
        Console.WriteLine($"Record already exists.");
        return;
      }
    }

    private void FindPaymentRecord()
    {
      // Ask user for email to search for
      Console.Write("Please enter the student email to search for: ");
      string email = Console.ReadLine();
      var student = CheckIfEmailExists(email);
      if (student != null)
      {
        Console.WriteLine();
        Console.WriteLine(student);
      }
    }


    private void ModifyPaymentRecord()
    {
      // first search if user already exists
      Console.Write("ENTER the email of the student to modify: ");
      string email = Console.ReadLine();
      Student stu = CheckIfEmailExists(email);


      // if student is found, ask which part of the record to change
      if (stu != null)
      {
        string response = "";

        do
        {
          // Print the menu for the user to select what to change
          PrintModifyMenu(studentType: stu.StudentType);
          response = Console.ReadLine().ToLower();

          switch (response)
          {
            case "f":
              Console.Write("ENTER new First and Middle name: ");
              string firstMidName = Console.ReadLine();
              if (firstMidName.Length == 0 || firstMidName == null)
              {
                Console.WriteLine("Invalid name. Please enter a valid name.");
                break;
              }
              else
              {
                stu.FirstMidName = firstMidName;
                Console.WriteLine($"Student's first and middle name has been updated to {stu.FirstMidName}! \n");
              }
              break;
            case "l":
              Console.Write("ENTER new Last name: ");
              string lastName = Console.ReadLine();
              if (lastName.Length == 0 || lastName == null)
              {
                Console.WriteLine("Invalid name. Please enter a valid name.");
                break;
              }
              else
              {
                stu.LastName = lastName;
                Console.WriteLine($"Student's last name has been updated to {stu.LastName}! \n");
              }
              break;
            case "g":
              Console.Write("ENTER new Grade point average: ");
              double gradePtAvg = double.Parse(Console.ReadLine());
              if (gradePtAvg < 0 || gradePtAvg > 4)
              {
                Console.WriteLine("Invalid GPA. Please enter a number between 0 and 4.");
                break;
              }
              else
              {
                stu.GradePtAvg = gradePtAvg;
                Console.WriteLine($"Student's GPA has been updated to {stu.GradePtAvg}! \n");
              }
              break;
            case "e":
              Console.Write("ENTER new Email address: ");
              string emailAddress = Console.ReadLine();
              if (emailAddress.Length == 0 || emailAddress == null)
              {
                Console.WriteLine("Invalid email. Please enter a valid email.");
                break;
              }
              else if (students.Any(student => student.EmailAddress == emailAddress))
              {
                Console.WriteLine("Email already exists. Please enter a unique email.");
                break;
              }
              else if (!emailAddress.Contains("@") || !emailAddress.Contains("."))
              {
                Console.WriteLine("Invalid email. Please enter a valid email.");
                break;
              }
              else
              {
                stu.EmailAddress = emailAddress;
                Console.WriteLine($"Student's email has been updated to {stu.EmailAddress}! \n");
              }
              break;
            case "s":
              Console.Write("ENTER new Student type, [G]raduate or [U]ndergrad: ");
              string studentType = Console.ReadLine().ToLower();
              if (studentType == "u")
              {
                if (stu is Undergrad)
                {
                  Console.WriteLine("Student is already an Undergrad student.");
                }
                else
                {
                  // get the rank
                  Console.WriteLine("[1] Freshman, [2] Sophomore, [3] Junior, [4] Senior: ");
                  Console.Write("ENTER Year rank in school from above choices: ");
                  YearRank rank = (YearRank)int.Parse(Console.ReadLine());

                  // verify the year rank is valid
                  if (rank < YearRank.Freshman || rank > YearRank.Senior)
                  {
                    Console.WriteLine("Invalid rank. Please enter a number between 1 and 4.");
                    break;
                  }

                  // get the degree program
                  Console.Write("Enter major degree program: ");
                  string major = Console.ReadLine();

                  // verify the major is valid
                  if (major.Length == 0 || major == null)
                  {
                    Console.WriteLine("Invalid major. Please enter a valid major.");
                    break;
                  }

                  //create a new undergrad student
                  Undergrad undergrad = new Undergrad(firstMidName: stu.FirstMidName,
                                                      lastName: stu.LastName,
                                                      gradePtAvg: stu.GradePtAvg,
                                                      emailAddress: stu.EmailAddress,
                                                      rank: rank,
                                                      degreeMajor: major);
                  // remove the old student record and add the new one
                  students.Remove(stu);
                  students.Add(undergrad);
                  Console.WriteLine($"Student's type has been updated to {studentType}! \n");
                }
              }
              else if (studentType == "g")
              {
                if (stu is Graduate)
                {
                  Console.WriteLine("Student is already a Graduate student.");
                }
                else
                {
                  // get the rank
                  Console.WriteLine("ENTER the tuition credit for this student (no commas): $");
                  decimal credit = decimal.Parse(Console.ReadLine());

                  // validate the credit
                  if (credit < 0)
                  {
                    Console.WriteLine("Invalid tuition credit. Please enter a positive number.");
                    break;
                  }

                  // get the advisor name
                  Console.Write("Enter full name of faculty advisor: ");
                  string advisor = Console.ReadLine();

                  if (advisor.Length == 0 || advisor == null)
                  {
                    Console.WriteLine("Invalid advisor name. Please enter a valid name.");
                    break;
                  }

                  //create the new graduate student
                  Graduate graduate = new Graduate(firstMidName: stu.FirstMidName,
                                                  lastName: stu.LastName,
                                                  gradePtAvg: stu.GradePtAvg,
                                                  emailAddress: stu.EmailAddress,
                                                  tuitionCredit: credit,
                                                  facultyAdvisor: advisor);

                  // remove the old student record and add the new one
                  students.Remove(stu);
                  students.Add(graduate);
                  Console.WriteLine($"Student's type has been updated to {studentType}! \n");
                }
              }
              else
              {
                Console.WriteLine("Invalid student type. Please enter 'G' or 'U'.");
              }
              break;
            case "r":
              if (stu is Undergrad)
              {
                Console.WriteLine("[1] Freshman, [2] Sophomore, [3] Junior, [4] Senior: ");
                Console.Write("ENTER Year rank in school from above choices: ");
                YearRank rank = (YearRank)int.Parse(Console.ReadLine());
                if (rank < YearRank.Freshman || rank > YearRank.Senior)
                {
                  Console.WriteLine("Invalid rank. Please enter a number between 1 and 4.");
                  break;
                }
                else
                {
                  ((Undergrad)stu).Rank = rank;
                  Console.WriteLine($"Student's rank has been updated to {((Undergrad)stu).Rank}! \n");
                }
              }
              else
              {
                Console.WriteLine("Invalid selection. Rank is only for Undergrad students.");
              }
              break;
            case "m":
              if (stu is Undergrad)
              {
                Console.Write("Enter major degree program: ");
                string degreeMajor = Console.ReadLine();
                if (degreeMajor.Length == 0 || degreeMajor == null)
                {
                  Console.WriteLine("Invalid major. Please enter a valid major.");
                  break;
                }
                else
                {
                  ((Undergrad)stu).DegreeMajor = degreeMajor;
                  Console.WriteLine($"Student's major has been updated to {((Undergrad)stu).DegreeMajor}! \n");
                }
              }
              else
              {
                Console.WriteLine("Invalid selection. Major is only for Undergrad students.");
              }
              break;
            case "t":
              if (stu is Graduate)
              {
                Console.WriteLine("ENTER the tuition credit for this student (no commas): $");
                decimal tuitionCredit = decimal.Parse(Console.ReadLine());
                if (tuitionCredit < 0)
                {
                  Console.WriteLine("Invalid tuition credit. Please enter a positive number.");
                  break;
                }
                else
                {
                  ((Graduate)stu).TuitionCredit = tuitionCredit;
                  Console.WriteLine($"Student's tuition credit has been updated to {((Graduate)stu).TuitionCredit}! \n");
                }
              }
              else
              {
                Console.WriteLine("Invalid selection. Tuition Credit is only for Graduate students.");
              }
              break;
            case "a":
              if (stu is Graduate)
              {
                Console.Write("Enter full name of faculty advisor: ");
                string facultyAdvisor = Console.ReadLine();
                if (facultyAdvisor.Length == 0 || facultyAdvisor == null)
                {
                  Console.WriteLine("Invalid advisor name. Please enter a valid name.");
                  break;
                }
                else
                {
                  ((Graduate)stu).FacultyAdvisor = facultyAdvisor;
                  Console.WriteLine($"Student's faculty advisor has been updated to {((Graduate)stu).FacultyAdvisor}! \n");
                }
              }
              else
              {
                Console.WriteLine("Invalid selection. Faculty Advisor is only for Graduate students.");
              }
              break;

          }
        } while (response != "q");
      }
      else
      {
        // if student is not found, report back to user
        Console.WriteLine($"Student with email {email} not found. Cannot modify.");
      }

    }

    private void PrintModifyMenu(string studentType)
    {
      string modMenu = string.Empty;
      switch (studentType)
      {
        case "Student":
          modMenu += "******************************\n";
          modMenu += "**** Modify Student Record ****\n";
          modMenu += "******************************\n";
          modMenu += "[F]irst and Middle Name\n";
          modMenu += "[L]ast Name\n";
          modMenu += "[G]rade Point Average\n";
          modMenu += "[E]mail Address\n";
          modMenu += "[S]tudent Type\n";
          modMenu += "[Q]uit\n";
          modMenu += "\n";
          modMenu += "User Key Selection: ";
          Console.Write(modMenu);
          break;
        case "Undergrad":
          modMenu += "******************************\n";
          modMenu += "**** Modify Undergrad Record ****\n";
          modMenu += "******************************\n";
          modMenu += "[F]irst and Middle Name\n";
          modMenu += "[L]ast Name\n";
          modMenu += "[G]rade Point Average\n";
          modMenu += "[E]mail Address\n";
          modMenu += "[R]ank\n";
          modMenu += "[M]ajor\n";
          modMenu += "[S]tudent Type\n";
          modMenu += "[Q]uit\n";
          modMenu += "\n";
          modMenu += "User Key Selection: ";
          Console.Write(modMenu);
          break;
        case "Graduate":
          modMenu += "******************************\n";
          modMenu += "**** Modify Graduate Record ****\n";
          modMenu += "******************************\n";
          modMenu += "[F]irst and Middle Name\n";
          modMenu += "[L]ast Name\n";
          modMenu += "[G]rade Point Average\n";
          modMenu += "[E]mail Address\n";
          modMenu += "[T]uition Credit\n";
          modMenu += "[A]dvisor\n";
          modMenu += "[S]tudent Type\n";
          modMenu += "[Q]uit\n";
          modMenu += "\n";
          modMenu += "User Key Selection: ";
          Console.Write(modMenu);
          break;
        default:
          Console.WriteLine("Invalid student type.");
          break;
      }
    }

    private void DeletePaymentRecord()
    {
      // first search if user already exists
      Console.Write("ENTER the email of the student to delete: ");
      string email = Console.ReadLine();
      Student stu = CheckIfEmailExists(email);


      if (stu != null)
      {
        // response from user to verify if record should be deleted
        string response = "";

        do
        {
          //Verify that user wants to delete the student record
          Console.Write($"Are you sure you want to delete student with email {email}? [Y/N]: ");
          response = Console.ReadLine().ToLower();

          // check if response is valid
          if (response != "y" && response != "n")
          {
            Console.WriteLine("Invalid response. Please enter 'Y' or 'N'.");
          }
        } while (response != "y" && response != "n");

        // if user responded positively, delete the record
        if (response == "y")
        {
          // Record was FOUND -- go ahead and delete
          students.Remove(stu);
          Console.WriteLine($"Student with email {email} has been deleted.");
        }
        // if the user responded negatively, do not delete the record and report back
        else
        {
          Console.WriteLine($"Student with email {email} has NOT been deleted.");
        }

      }
      else
      {
        Console.WriteLine($"Student with email {email} not found. Cannot delete.");
      }
    }

    private void PrintAllPaymentRecord()
    {
      Console.WriteLine("**** Printing all student records in file ****");
      Console.WriteLine();
      foreach (var student in students)
      {
        Console.WriteLine(student);
      }
      Console.WriteLine("Done printing all stuednt records in File ****");
      Console.WriteLine();
    }

    private void PrintAllPaymentRecordKeys()
    {

      Console.WriteLine("**** Printing all student email addresses in file ****");
      Console.WriteLine();
      foreach (var student in students)
      {
        Console.WriteLine(student.EmailAddress);

        Console.WriteLine("**** Done printing all stuednt email addresses in file ****");
        Console.WriteLine();
      }
    }

    private void SavePaymentDataAndExit()
    {
      Console.WriteLine("Saving Data and exiting");
      WriteDataToOutPutFile();
      Environment.Exit(0);
    }

    /// <summary>
    /// gets the key entered by user wihtout having to hit enter
    /// </summary>
    /// <returns></returns>
    private char GetUserSelection()
    {
      ConsoleKeyInfo key = Console.ReadKey();
      return key.KeyChar;
    }

    private void DisplayMainMenu()
    {
      string menu = string.Empty;
      menu += "******************************\n";
      menu += "**** Student Database App ****\n";
      menu += "******************************\n";
      menu += "[A]dd a student record (C in CRUD - Create)\n";
      menu += "[F]ind a student record (R in CRUD - Read)\n";
      menu += "[M]odify a student record  (U in CRUD - Update)\n";
      menu += "[D]elete a student  record  (D in CRUD - Delete)\n";
      menu += "[P]rint all records in current db storage\n";
      menu += "[S]ave data to file and exit app\n";
      menu += "[E]xit app wihthout saving chnages\n";
      menu += "\n";
      menu += "User Key Selection: ";

      Console.Write(menu);
    }

    private void WriteDataToOutputFileText()
    {
      // Create an object that attacthes toa  file on disk
      StreamWriter outFile = new StreamWriter(Constants.StudentOutPutTextFile);

      //for user to see
      Console.WriteLine("Outputting student data to the output file.");

      //use the refrence to the file above to write the file
      foreach (var student in students)
      {
        // show each student to user for now
        Console.WriteLine(student);
        outFile.WriteLine(student);
      }
      // close the resource
      outFile.Close();
    }


    private void WriteDataToOutPutFileCSV()
    {
      // create a list to hold the CSV lines
      var csvLines = new List<string>();

      // add the header line
      csvLines.Add("FirstNmae,LastName,GPA,Email");

      foreach (var student in students)
      {
        string line = student.ToCSVFormat();
        csvLines.Add(line);
      }

      File.WriteAllLines(Constants.StudentOutPutCSVFile, csvLines);
    }

    private void WriteDataToOutPutFile()
    {
      string json = JsonConvert.SerializeObject(students, Formatting.Indented);
      File.WriteAllText(Constants.StudentOuputJSONFile, json);
    }

    public void StudentDbTester()
    {
      Student stu1 = new Student();
      Student stu2 = new Student();
      Student stu3 = new Student();


      // does not scale well- should have a constructor
      stu1.FirstMidName = "Allison Amy";
      stu1.LastName = "adams";
      stu1.GradePtAvg = 3.95;
      stu1.EmailAddress = "aaadm@uw.edu";


      Student stu4 = new Student(
      firstMidName: "Jhon James",
      lastName: "jones",
      gradePtAvg: 3.86,
      emailAddress: "jjjjones@uw.edu"
      );

      stu2 = new Student(
      firstMidName: "bob bobo",
      lastName: "Bobbins",
      gradePtAvg: 2.17,
      emailAddress: "bbbobbubins@uw.edu"
      );

      stu3 = new Student(
      firstMidName: "Mary marie",
      lastName: "masterson",
      gradePtAvg: 4.0,
      emailAddress: "mmastersonj@uw.edu"
      );

      // outpout can be done individually
      // need a Tostring 
      //Console.WriteLine(stu1);
      //Console.WriteLine(stu2);
      //Console.WriteLine(stu3);
      //Console.WriteLine(stu4);

      Undergrad stu5 = new Undergrad(
      firstMidName: "Jonny John",
      lastName: "Johnson",
      gradePtAvg: 2.8,
      emailAddress: "jjjohnson@uw.edu",
      rank: YearRank.Junior,
      degreeMajor: "IT"
      );


      Graduate stu6 = new Graduate(
      firstMidName: "Tom Thomas",
      lastName: "Thompson",
      gradePtAvg: 2.8,
      emailAddress: "ttthompson@uw.edu",
      tuitionCredit: 550.75m,
      facultyAdvisor: "Derek Atwood"
      );

      //Addingit students to student instance List
      students.Add(stu1);
      students.Add(stu2);
      students.Add(stu3);
      students.Add(stu4);
      students.Add(stu5);
      students.Add(stu6);
    }
  }
}
