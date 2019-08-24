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
    public class TrainingDogModelsController : Controller
    {
        private readonly LuckyPawContext _context;

        public TrainingDogModelsController(LuckyPawContext context)
        {
            _context = context;
        }

        // GET: TrainingDogModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingDogModel.ToListAsync());
        }

        // GET: TrainingDogModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingDogModel = await _context.TrainingDogModel
                .FirstOrDefaultAsync(m => m.TrainingId == id);
            if (trainingDogModel == null)
            {
                return NotFound();
            }

            return View(trainingDogModel);
        }

        // GET: TrainingDogModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingDogModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingId,DogName,TrainerId,GraduationDate,TrainingType")] TrainingDogModel trainingDogModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingDogModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingDogModel);
        }

        // GET: TrainingDogModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingDogModel = await _context.TrainingDogModel.FindAsync(id);
            if (trainingDogModel == null)
            {
                return NotFound();
            }
            return View(trainingDogModel);
        }

        // POST: TrainingDogModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingId,DogName,TrainerId,GraduationDate,TrainingType")] TrainingDogModel trainingDogModel)
        {
            if (id != trainingDogModel.TrainingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingDogModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingDogModelExists(trainingDogModel.TrainingId))
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
            return View(trainingDogModel);
        }

        // GET: TrainingDogModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingDogModel = await _context.TrainingDogModel
                .FirstOrDefaultAsync(m => m.TrainingId == id);
            if (trainingDogModel == null)
            {
                return NotFound();
            }

            return View(trainingDogModel);
        }

        // POST: TrainingDogModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingDogModel = await _context.TrainingDogModel.FindAsync(id);
            _context.TrainingDogModel.Remove(trainingDogModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingDogModelExists(int id)
        {
            return _context.TrainingDogModel.Any(e => e.TrainingId == id);
        }
    }
}
