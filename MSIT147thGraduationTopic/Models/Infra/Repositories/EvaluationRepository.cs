using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using MSIT147thGraduationTopic.ViewComponents;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class EvaluationRepository
    {
        private readonly GraduationTopicContext _context;
        public EvaluationRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public async Task<List<EvaluationVM>?> ShowMoreEvaliation(int id, int evaluationPageCounts)
        {           
                return await (from e in _context.EvaluationInputs
                             join s in _context.Specs on e.SpecId equals s.SpecId
                             join m in _context.Merchandises on  e.MerchandiseId equals m.MerchandiseId 
                             where e.MerchandiseId == id
                             orderby e.EvaluationId descending
                             select new EvaluationVM
                             {
                                 MerchandiseId = id,
                                 EvaluationId = e.EvaluationId,
                                 MerchandiseName = m.MerchandiseName,
                                 SpecName = s.SpecName,
                                 Comment = e.Comment,
                                 Score = e.Score,
                             }).Skip((evaluationPageCounts-1) * 5).Take(5).ToListAsync();            
        }
    }
}
