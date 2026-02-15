using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using MyFirstMCP;
using System.Text.Json;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<MonkeyService>();
builder.Services.AddSingleton<EmployeeService>();
builder.Services.AddSingleton<ProductService>();

await builder.Build().RunAsync();

[McpServerToolType]
public static class EchoTool
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"Hello from C#: {message}";

    [McpServerTool, Description("Echoes in reverse the message sent by the client.")]
    public static string ReverseEcho(string message) => new string(message.Reverse().ToArray());
}

[McpServerToolType]
public static class ProductTools
{
    [McpServerTool, Description("Get a list of products.")]
    public static async Task<string> GetProducts(ProductService productService)
    {
        var list = await productService.GetProductsAsync();
        return JsonSerializer.Serialize(list);
    }

    [McpServerTool, Description("Get a product by id.")]
    public static async Task<string> GetProduct(ProductService productService, [Description("The product id")] string id)
    {
        var p = await productService.GetProductByIdAsync(id);
        return JsonSerializer.Serialize(p);
    }

    [McpServerTool, Description("Create a product (pass JSON for the product).")]
    public static async Task<string> CreateProduct(ProductService productService, [Description("Product JSON")] string productJson)
    {
        var prod = JsonSerializer.Deserialize<Product>(productJson, JsonSerializerOptions.Web);
        if (prod == null) return "null";
        var created = await productService.CreateProductAsync(prod);
        return JsonSerializer.Serialize(created);
    }

    [McpServerTool, Description("Delete a product by id.")]
    public static async Task<bool> DeleteProduct(ProductService productService, [Description("The product id")] string id)
    {
        return await productService.DeleteProductAsync(id);
    }
}

[McpServerToolType]
public static class MonkeyTools
{
    [McpServerTool, Description("Get a list of monkeys.")]
    public static async Task<string> GetMonkeys(MonkeyService monkeyService)
    {
        var monkeys = await monkeyService.GetMonkeys();
        return JsonSerializer.Serialize(monkeys);
    }

    [McpServerTool, Description("Get a monkey by name.")]
    public static async Task<string> GetMonkey(MonkeyService monkeyService, [Description("The name of the monkey to get details for")] string name)
    {
        var monkey = await monkeyService.GetMonkey(name);
        return JsonSerializer.Serialize(monkey);
    }
}

[McpServerToolType]
public static class EmployeeTools
{
    [McpServerTool, Description("Get a list of employees.")]
    public static string GetEmployees(EmployeeService employeeService)
    {
        var employees = employeeService.GetEmployeeLists();
        return JsonSerializer.Serialize(employees);
    }

    [McpServerTool, Description("Get an employee by name.")]
    public static string GetEmployee(EmployeeService employeeService, [Description("The name of the employee to get details for")] string name)
    {
        var employee = employeeService.GetEmployee(name);
        return JsonSerializer.Serialize(employee);
    }
}