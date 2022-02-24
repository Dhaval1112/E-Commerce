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
                CetegoryDescription = category.CetegoryDescription,
                CoverImageUrl = category.CoverImageUrl,
                Id = category.Id,
            }).ToList();
        }

        
        public CategoryModel GetCategory(int id)
        {
            var findCategory = _context.Categories.Select(category => new CategoryModel()
            {
                CategoryName = category.CategoryName,
                CetegoryDescription = category.CetegoryDescription,
                CoverImageUrl = category.CoverImageUrl,
                Id = category.Id
            }).Where(md=>md.Id ==id).FirstOrDefault() ;

            return findCategory;
        }

        public async Task<bool> UpdateCategory(CategoryModel category)
        {
            try
            {

                var oldCategory = await _context.Categories.FindAsync(category.Id);
                oldCategory.CategoryName = category.CategoryName;
                oldCategory.CetegoryDescription = category.CetegoryDescription;
                oldCategory.CoverImageUrl = category.CoverImageUrl;
                _context.Categories.Update(oldCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            

        }
    }
}
