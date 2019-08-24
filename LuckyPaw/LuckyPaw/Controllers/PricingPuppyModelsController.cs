using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuckyPaw.Data;
using LuckyPaw.Models;

namespace LuckyPaw.Controllers
{
    public class PricingPuppyModelsController : Controller
    {
        private readonly LuckyPawContext _context;

        public PricingPuppyModelsController(LuckyPawContext context)
        {
            _context = context;
        }

        // GET: PricingPuppyModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.PricingPuppyModel.ToListAsync());
        }

        // GET: PricingPuppyModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingPuppyModel = await _context.PricingPuppyModel
                .FirstOrDefaultAsync(m => m.PricePuppyID == id);
            if (pricingPuppyModel == null)
            {
                return NotFound();
            }

            return View(pricingPuppyModel);
        }

        // GET: PricingPuppyModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PricingPuppyModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PricePuppyID,PricePuppyDesc,PricePuppy,PricePuppyImageUrl,PuppyQty")] PricingPuppyModel pricingPuppyModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pricingPuppyModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pricingPuppyModel);
        }

        // GET: PricingPuppyModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingPuppyModel = await _context.PricingPuppyModel.FindAsync(id);
            if (pricingPuppyModel == null)
            {
                return NotFound();
            }
            return View(pricingPuppyModel);
        }

        // POST: PricingModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PricePuppyID,PricePuppyDesc,PricePuppy,PricePuppyImageUrl,PuppyQty")] PricingPuppyModel pricingPuppyModel)
        {
            if (id != pricingPuppyModel.PricePuppyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricingPuppyModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricingModelExists(pricingPuppyModel.PricePuppyID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pricingPuppyModel);
        }

        // GET: PricingModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingPuppyModel = await _context.PricingPuppyModel
                .FirstOrDefaultAsync(m => m.PricePuppyID == id);
            if (pricingPuppyModel == null)
            {
                return NotFound();
            }

            return View(pricingPuppyModel);
        }

        // POST: PricingModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pricingPuppyModel = await _context.PricingPuppyModel.FindAsync(id);
            _context.PricingPuppyModel.Remove(pricingPuppyModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PricingModelExists(int id)
        {
            return _context.PricingPuppyModel.Any(e => e.PricePuppyID == id);
        }
    }
}
