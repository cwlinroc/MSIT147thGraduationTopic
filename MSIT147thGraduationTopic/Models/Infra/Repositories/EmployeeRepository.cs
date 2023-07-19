using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
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
