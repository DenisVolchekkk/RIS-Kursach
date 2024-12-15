using Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace StudyProj.Repositories.Implementations
{
    public class FacilityRepository : BaseRepository<Facility>
    {
        public FacilityRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<List<Facility>> GetAllAsync(Facility facility)
        {
            var Facilities = Context.Set<Facility>().AsQueryable();
            if (!string.IsNullOrEmpty(facility.Name))
            {
                Facilities = Facilities.Where(d => d.Name.Contains(facility.Name));
            }
            return await Facilities.ToListAsync();  
        }
    }
}
