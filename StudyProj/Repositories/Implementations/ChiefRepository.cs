﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace StudyProj.Repositories.Implementations
{
    public class ChiefRepository : BaseRepository<Chief>
    {
        public ChiefRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<List<Chief>> GetAllAsync(Chief chief)
        {
            var Chiefs = Context.Set<Chief>().AsQueryable();
            if (!string.IsNullOrEmpty(chief.Name))
            {
                Chiefs = Chiefs.Where(d => d.Name.Contains(chief.Name));
            }
            return await Chiefs.ToListAsync();  // Асинхронное получение всех записей
        }
    }
}
