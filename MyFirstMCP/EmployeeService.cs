using System.Text.Json;
using System.Linq;

namespace MyFirstMCP;

public class EmployeeService
{
    List<Employee> employeeList = new();

    public List<Employee> GetEmployeeLists()
    {
        if (employeeList?.Count > 0)
            return employeeList;
        var mockData = File.ReadAllText("D:\\employee.json");
        employeeList = JsonSerializer.Deserialize<List<Employee>>(mockData, JsonSerializerOptions.Web) ?? new List<Employee>();
        return employeeList;
    }

    public Employee? GetEmployee(string name)
    {
        var employees = GetEmployeeLists();
        return employees.FirstOrDefault(m => m.FIRST_NAME?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
    }
}

public record Employee
{
    public int id { get; set; }
    public string? FIRST_NAME { get; set; }
    public string? LAST_NAME { get; set; }
    public string? EMAIL { get; set; }
    public string? PHONE_NUMBER { get; set; }
    public DateTime HIRE_DATE { get; set; }
    public double SALARY { get; set; }
    public int DEPARTMENT_ID { get; set; }
    public string? Image { get; set; }
}