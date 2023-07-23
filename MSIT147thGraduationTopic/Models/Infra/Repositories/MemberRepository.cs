using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class MemberRepository
    {
        public int Create(MemberDto dto)
        {
            var db = new GraduationTopicContext();
            var obj = dto.ToEF();
            db.Members.Add(obj);
            db.SaveChanges();
            return obj.MemberId;
        }
    }
}
