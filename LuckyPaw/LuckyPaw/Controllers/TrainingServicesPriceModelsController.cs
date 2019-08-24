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
    public class TrainingServicesPriceModelsController : Controller
    {
        private readonly LuckyPawContext _context;

        public TrainingServicesPriceModelsController(LuckyPawContext context)
        {
            _context = context;
        }

        // GET: TrainingServicesPriceModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingServicesPriceModel.ToListAsync());
        }

        // GET: TrainingServicesPriceModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingServicesPriceModel = await _context.TrainingServicesPriceModel
                .FirstOrDefaultAsync(m => m.TrainingServicesPriceID == id);
            if (trainingServicesPriceModel == null)
            {
                return NotFound();
            }

            return View(trainingServicesPriceModel);
        }

        // GET: TrainingServicesPriceModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingServicesPriceModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingServicesPriceID,TrainingName,PriceTraining,TrainingDesc")] TrainingServicesPriceModel trainingServicesPriceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingServicesPriceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingServicesPriceModel);
        }

        // GET: TrainingServicesPriceModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingServicesPriceModel = await _context.TrainingServicesPriceModel.FindAsync(id);
            if (trainingServicesPriceModel == null)
            {
                return NotFound();
            }
            return View(trainingServicesPriceModel);
        }

        // POST: TrainingServicesPriceModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingServicesPriceID,TrainingName,PriceTraining,TrainingDesc")] TrainingServicesPriceModel trainingServicesPriceModel)
        {
            if (id != trainingServicesPriceModel.TrainingServicesPriceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingServicesPriceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingServicesPriceModelExists(trainingServicesPriceModel.TrainingServicesPriceID))
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
            return View(trainingServicesPriceModel);
        }

        // GET: TrainingServicesPriceModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingServicesPriceModel = await _context.TrainingServicesPriceModel
                .FirstOrDefaultAsync(m => m.TrainingServicesPriceID == id);
            if (trainingServicesPriceModel == null)
            {
                return NotFound();
            }

            return View(trainingServicesPriceModel);
        }

        // POST: TrainingServicesPriceModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingServicesPriceModel = await _context.TrainingServicesPriceModel.FindAsync(id);
            _context.TrainingServicesPriceModel.Remove(trainingServicesPriceModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingServicesPriceModelExists(int id)
        {
            return _context.TrainingServicesPriceModel.Any(e => e.TrainingServicesPriceID == id);
        }
    }
}
