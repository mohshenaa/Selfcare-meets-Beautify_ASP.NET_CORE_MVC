using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Selfcare_meets_Beautify.Models;
using Selfcare_meets_Beautify.Services;

namespace Selfcare_meets_Beautify.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly SelfcareDb _context;
        private readonly IUploadService _uploadService;

        public ProductController(SelfcareDb context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Type })
                .ToListAsync();

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Update product properties
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

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction("Details", "Brand", new { id = existingProduct.BrandId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Type })
                .ToListAsync();

            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Brand)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                var brandId = product.BrandId; // Store brand ID before deletion for redirect

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Product '{product.Name}' has been deleted successfully!";
                return RedirectToAction("Details", "Brand", new { id = brandId });
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);

                TempData["ErrorMessage"] = "An error occurred while deleting the product. Please try again.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Unexpected error deleting product with ID {ProductId}", id);

                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                return RedirectToAction("Index", "Brand");
            }
        }

        // GET: Product/AllProducts
        public async Task<IActionResult> AllProducts(string searchString, string categoryFilter, string skinTypeFilter, string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSort"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSort"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["BrandSort"] = sortOrder == "Brand" ? "brand_desc" : "Brand";

            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .AsQueryable();

            // Search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    p.Name.Contains(searchString) ||
                    p.Brand.Name.Contains(searchString) ||
                    p.Description.Contains(searchString));
            }

            // Category filter
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                products = products.Where(p => p.Category.Type == categoryFilter);
            }

            // Skin type filter
            if (!string.IsNullOrEmpty(skinTypeFilter))
            {
                products = products.Where(p => p.SkinType == skinTypeFilter);
            }

            // Sorting
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "Brand":
                    products = products.OrderBy(p => p.Brand.Name);
                    break;
                case "brand_desc":
                    products = products.OrderByDescending(p => p.Brand.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            // Get filters for dropdowns
            ViewBag.Categories = await _context.Categories
                .Select(c => c.Type)
                .Distinct()
                .ToListAsync();

            ViewBag.SkinTypes = await _context.Products
                .Select(p => p.SkinType)
                .Distinct()
                .Where(st => !string.IsNullOrEmpty(st))
                .ToListAsync();

            ViewBag.Brands = await _context.Brands
                .Select(b => b.Name)
                .Distinct()
                .ToListAsync();

            var productList = await products.ToListAsync();

            // Pass search string to view
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentCategory"] = categoryFilter;
            ViewData["CurrentSkinType"] = skinTypeFilter;

            return View(productList);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}