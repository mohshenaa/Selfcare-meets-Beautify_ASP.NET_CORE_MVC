using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Selfcare_meets_Beautify.Models;
using Selfcare_meets_Beautify.Services;

namespace Selfcare_meets_Beautify.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        private readonly SelfcareDb _context;
        private readonly IUploadService _uploadService;

        public BrandController(SelfcareDb context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
            var brands = await _context.Brands
                .Include(b => b.Products)
                    .ThenInclude(p => p.Category)
                .ToListAsync();
            return View(brands);
        }

        // GET: Brand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(b => b.Products)
                    .ThenInclude(p => p.Category) // Include category for product display
                .FirstOrDefaultAsync(m => m.Id == id);

            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var brand = new Brand
            {
                Products = new List<Product> { new Product() }
            };
            ViewBag.Categories = GetCategories();
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand, string operation = "")
        {
            if (operation == "add" || operation.StartsWith("delete"))
            {
                if (operation == "add")
                {
                    brand.Products.Add(new Product());
                }
                else if (operation.StartsWith("delete-"))
                {
                    int.TryParse(operation.Replace("delete-", ""), out int index);
                    if (index >= 0 && index < brand.Products.Count)
                    {
                        brand.Products.RemoveAt(index);
                    }
                }

                ViewBag.Categories = GetCategories();
                ModelState.Clear(); // CRITICAL: Add this line
                return View(brand);
            }

            if (ModelState.IsValid)
            {
                // Handle logo upload using UploadService
                if (brand.LogoFile != null && brand.LogoFile.Length > 0)
                {
                    brand.LogoUrl = await _uploadService.FileSave(brand.LogoFile);
                }

                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();

                // Handle product image uploads and set BrandId
                foreach (var product in brand.Products.Where(p => !string.IsNullOrEmpty(p.Name)))
                {
                    if (product.ImageFile != null && product.ImageFile.Length > 0)
                    {
                        product.ImageUrl = await _uploadService.FileSave(product.ImageFile);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Brand and products created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = GetCategories();
            return View(brand);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
            {
                return NotFound();
            }

            ViewBag.Categories = GetCategories();
            ViewBag.ProductsToDelete = new List<int>(); // Initialize empty list
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand brand, string operation = "", List<int> productsToDelete = null)
        {
            Console.WriteLine($"Edit POST called - ID: {id}, Operation: {operation}");

            if (id != brand.Id)
            {
                return NotFound();
            }

            // Handle add/remove product operations
            if (operation == "add" || operation.StartsWith("delete"))
            {
                Console.WriteLine($"Handling operation: {operation}");

                if (operation == "add")
                {
                    brand.Products.Add(new Product());
                    Console.WriteLine("Added new product");
                }
                else if (operation.StartsWith("delete-"))
                {
                    int.TryParse(operation.Replace("delete-", ""), out int index);
                    if (index >= 0 && index < brand.Products.Count)
                    {
                        var productToDelete = brand.Products[index];

                        // If it's an existing product (has ID), add to deletion list
                        if (productToDelete.Id > 0)
                        {
                            if (productsToDelete == null)
                                productsToDelete = new List<int>();

                            productsToDelete.Add(productToDelete.Id);
                            ViewBag.ProductsToDelete = productsToDelete;
                        }

                        brand.Products.RemoveAt(index);
                        Console.WriteLine($"Removed product at index: {index}");
                    }
                }

                ViewBag.Categories = GetCategories();
                ModelState.Clear();
                Console.WriteLine("Returning view with updated products");
                return View(brand);
            }

            // This is the final save operation
            Console.WriteLine("Final save operation");

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState is valid, saving changes...");

                    var existingBrand = await _context.Brands
                        .Include(b => b.Products)
                        .FirstOrDefaultAsync(b => b.Id == id);

                    if (existingBrand == null)
                    {
                        return NotFound();
                    }

                    // Update brand properties
                    existingBrand.Name = brand.Name;

                    // Handle logo upload if new file is provided
                    if (brand.LogoFile != null && brand.LogoFile.Length > 0)
                    {
                        existingBrand.LogoUrl = await _uploadService.FileSave(brand.LogoFile);
                    }

                    // Handle product deletions first
                    if (productsToDelete != null && productsToDelete.Any())
                    {
                        Console.WriteLine($"Deleting products: {string.Join(",", productsToDelete)}");
                        foreach (var productId in productsToDelete)
                        {
                            var productToDelete = await _context.Products.FindAsync(productId);
                            if (productToDelete != null)
                            {
                                _context.Products.Remove(productToDelete);
                            }
                        }
                    }

                    // Update existing products and add new ones
                    foreach (var product in brand.Products)
                    {
                        if (product.Id > 0)
                        {
                            // Update existing product
                            var existingProduct = await _context.Products.FindAsync(product.Id);
                            if (existingProduct != null)
                            {
                                existingProduct.Name = product.Name;
                                existingProduct.Price = product.Price;
                                existingProduct.Size = product.Size;
                                existingProduct.SkinType = product.SkinType;
                                existingProduct.Origin = product.Origin;
                                existingProduct.CategoryId = product.CategoryId;
                                existingProduct.ProductionDate = product.ProductionDate;
                                existingProduct.ExpiryDate = product.ExpiryDate;
                                existingProduct.Description = product.Description;

                                // Handle product image upload
                                if (product.ImageFile != null && product.ImageFile.Length > 0)
                                {
                                    existingProduct.ImageUrl = await _uploadService.FileSave(product.ImageFile);
                                }
                            }
                        }
                        else
                        {
                            // Add new product
                            product.BrandId = brand.Id;

                            // Handle product image upload
                            if (product.ImageFile != null && product.ImageFile.Length > 0)
                            {
                                product.ImageUrl = await _uploadService.FileSave(product.ImageFile);
                            }

                            _context.Products.Add(product);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Brand and products updated successfully!";
                    Console.WriteLine("Save successful, redirecting to index");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Concurrency exception: {ex.Message}");
                    if (!BrandExists(brand.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during save: {ex.Message}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
            }

            ViewBag.Categories = GetCategories();
            return View(brand);
        }

        // GET: Brand/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(b => b.Products)
                    .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var brand = await _context.Brands
                    .Include(b => b.Products)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (brand == null)
                {
                    return NotFound();
                }

                // Remove all products associated with this brand first
                if (brand.Products.Any())
                {
                    _context.Products.RemoveRange(brand.Products);
                }

                // Then remove the brand
                _context.Brands.Remove(brand);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Brand '{brand.Name}' and all associated products have been deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error deleting brand with ID {BrandId}", id);

                TempData["ErrorMessage"] = "An error occurred while deleting the brand. Please try again.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Unexpected error deleting brand with ID {BrandId}", id);

                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }


        public IActionResult GetProductPartial(int index)
        {
            ViewData["Index"] = index;
            ViewBag.Categories = GetCategories(); 
            return PartialView("_ProductPartial", new Brand());
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }

        private List<SelectListItem> GetCategories()
        {
            return _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Type })
                .ToList();
        }
    }
}