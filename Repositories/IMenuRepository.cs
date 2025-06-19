using System.Collections.Generic;
using System.Threading.Tasks;
using Efood_Menu.Models;

namespace Efood_Menu.Repositories
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Menu>> GetAllAsync();
        Task<Menu?> GetByIdAsync(int id);
        Task AddAsync(Menu menu);
        void Update(Menu menu);
        void Remove(Menu menu);
        Task SaveChangesAsync();
        Task<IEnumerable<Menu>> GetMenusWithFoodItemsAsync();
    }
}