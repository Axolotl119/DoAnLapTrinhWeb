using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Efood_Menu.Models;

namespace Efood_Menu.Repositories
{
    public class EFMenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public EFMenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await _context.Menus.ToListAsync();
        }

        public async Task<Menu?> GetByIdAsync(int id)
        {
            return await _context.Menus.FindAsync(id);
        }

        public async Task AddAsync(Menu menu)
        {
            await _context.Menus.AddAsync(menu);
        }

        public void Update(Menu menu)
        {
            _context.Menus.Update(menu);
        }

        public void Remove(Menu menu)
        {
            _context.Menus.Remove(menu);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Menu>> GetMenusWithFoodItemsAsync()
        {
            return await _context.Menus
                .Include(m => m.FoodItems)
                .ThenInclude(f => f.Category)
                .ToListAsync();
        }
    }
}