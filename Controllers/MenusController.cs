using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Efood_Menu.Models;
using Efood_Menu.Repositories;

namespace Efood_Menu.Controllers
{
    public class MenusController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly ApplicationDbContext _context; // Only for FoodItems SelectList

        public MenusController(IMenuRepository menuRepository, ApplicationDbContext context)
        {
            _menuRepository = menuRepository;
            _context = context;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            var menus = await _menuRepository.GetAllAsync();
            return View(menus);
        }

        // GET: Menus/FoodMenu
        public async Task<IActionResult> FoodMenu()
        {
            var menus = await _menuRepository.GetMenusWithFoodItemsAsync();
            return View(menus);
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var menu = await _menuRepository.GetByIdAsync(id.Value);
            if (menu == null)
                return NotFound();

            return View(menu);
        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            ViewBag.FoodItems = new MultiSelectList(_context.FoodItems, "Id", "Name");
            return View();
        }

        // POST: Menus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Menu menu, int[] selectedFoodItems)
        {
            if (ModelState.IsValid)
            {
                menu.CreatedAt = DateTime.Now;
                menu.FoodItems = new List<FoodItem>();
                if (selectedFoodItems != null && selectedFoodItems.Length > 0)
                {
                    var foodItems = _context.FoodItems.Where(f => selectedFoodItems.Contains(f.Id)).ToList();
                    foreach (var item in foodItems)
                    {
                        menu.FoodItems.Add(item);
                    }
                }
                await _menuRepository.AddAsync(menu);
                await _menuRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.FoodItems = new MultiSelectList(_context.FoodItems, "Id", "Name", selectedFoodItems);
            return View(menu);
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var menu = await _menuRepository.GetByIdAsync(id.Value);
            if (menu == null)
                return NotFound();

            return View(menu);
        }
    }
}
