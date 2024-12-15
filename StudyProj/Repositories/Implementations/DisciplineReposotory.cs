﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace StudyProj.Repositories.Implementations
{
    public class DisciplineRepository : BaseRepository<Discipline>
    {
        public DisciplineRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<List<Discipline>> GetAllAsync(Discipline discipline)
        {
            var Disciplines = Context.Set<Discipline>().AsQueryable();
            if (!string.IsNullOrEmpty(discipline.Name))
            {
                Disciplines = Disciplines.Where(d => d.Name.Contains(discipline.Name));
            }
            return await Disciplines.ToListAsync();  // Асинхронное получение всех записей
        }
    }
}
