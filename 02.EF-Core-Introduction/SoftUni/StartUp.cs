using System;

namespace SoftUni
{
    using SoftUni.Data;
    using SoftUni.Models;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext db = new SoftUniContext();

            string result = RemoveTown(db);

            Console.WriteLine(result);
        }


        // 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new 
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .ToArray();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} from {item.DepartmentName} - ${item.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        // 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Employee[] employee = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .ToArray();

            foreach (Employee e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        

        // 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                //.ToArray()
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} - {item.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        // 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAdres = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAdres);

            Employee nakovEmployee = context
                .Employees
                .First(e => e.LastName == "Nakov");

            nakovEmployee.Address = newAdres;

            context.SaveChanges();

            var all = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToArray();

            return string.Join(Environment.NewLine, all);
        }


        // 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            IQueryable<Employee> employeeToIncrease = context
                .Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services");

            foreach (var e in employeeToIncrease)
            {
                e.Salary *= 1.12m;
            }

            context.SaveChanges();

            var employees = employeeToIncrease
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            StringBuilder result = new StringBuilder();

            foreach (var item in employees)
            {
                result.AppendLine( $"{item.FirstName} {item.LastName} (${item.Salary:f2})");
            }

            return result.ToString().TrimEnd();
        }


        // 15
        public static string RemoveTown(SoftUniContext context)
        {
            Address[] siatleAdresses = context
                .Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToArray();

            Employee[] employeesInSeattle = context
                .Employees
                .ToArray()
                .Where(e => siatleAdresses.Any(a => a.AddressId == e.AddressId))
                .ToArray();

            foreach (Employee employee in employeesInSeattle)
            {
                employee.AddressId = null;
            }

            context.Addresses.RemoveRange(siatleAdresses);

            Town seattleTown = context
                .Towns
                .First(t => t.Name == "Seattle");

            context.Towns.Remove(seattleTown);

            context.SaveChanges();

            return $"{siatleAdresses.Length} addresses in Seattle were deleted";
        }
    }
}
