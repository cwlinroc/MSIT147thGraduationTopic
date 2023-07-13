// See https://aka.ms/new-console-template for more information

using MSIT147.Estore.DataLayer.Dtos;
using MSIT147.Estore.DataLayer.Repositories;



var repo = new EmployeeRepository();
repo.Create(new EmployeeDto
{
    EmployeeAccount = "test",
    EmployeeEmail = "test",
    EmployeeName = "test",
    EmployeeAvatarPath = "test",
    EmployeePassword = "test",
    EmployeePhone = "test",
    Permission = 1
});
Console.WriteLine("success");

