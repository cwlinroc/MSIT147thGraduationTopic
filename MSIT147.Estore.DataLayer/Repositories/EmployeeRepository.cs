using MSIT147.Estore.DataLayer.Dtos;
using MSIT147.Estore.DataLayer.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIT147.Estore.DataLayer.Repositories
{
    public class EmployeeRepository
    {
        public int Create(EmployeeDto dto)
        {
            var db = new GraduationTopicContext();
            var obj = dto.ToEF();
            db.Employees.Add(obj);
            db.SaveChanges();
            return obj.EmployeeId;
        }
    }
}
