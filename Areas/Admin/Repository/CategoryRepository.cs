using E_Commerce.Areas.Admin.Data;
using E_Commerce.Areas.Admin.Models;
using E_Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ECommerceContext _context;

        public CategoryRepository(ECommerceContext context)
        {
            this._context = context;
        }

        // for creating repository
        public async Task<int> CreateCategoryAsync(CategoryModel model)
        {
            var category = new Category()
            {
                CategoryName = model.CategoryName,
                CetegoryDescription = model.CetegoryDescription,
                CoverImageUrl = model.CoverImageUrl
            };
            await _context.Categories.AddAsync(category);
            var result = await _context.SaveChangesAsync();
            return result;


        }

        public List<CategoryModel> AllCategories()
        {
            return _context.Categories.Select(category => new CategoryModel()
            {
                CategoryName = category.CategoryName,
                Id = category.Id,
            }).ToList();
        }
    }
}
