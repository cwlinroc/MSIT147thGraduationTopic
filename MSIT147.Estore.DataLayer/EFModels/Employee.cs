using System;
using System.Collections.Generic;

namespace MSIT147.Estore.DataLayer.EFModels;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string EmployeeAccount { get; set; } = null!;

    public string EmployeePassword { get; set; } = null!;

    public int Permission { get; set; }

    public string EmployeeEmail { get; set; } = null!;

    public string EmployeePhone { get; set; } = null!;

    public string? EmployeeAvatarPath { get; set; }
}
