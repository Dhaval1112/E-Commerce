using E_Commerce.Areas.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Repository
{
    public interface ICategoryRepository
    {
        Task<int> CreateCategoryAsync(CategoryModel model);
        List<CategoryModel> AllCategories();
        CategoryModel GetCategory(int id);
        Task<bool> UpdateCategory(CategoryModel category);
    }
}