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
    public class TrainersModelsController : Controller
    {
        private readonly LuckyPawContext _context;

        public TrainersModelsController(LuckyPawContext context)
        {
            _context = context;
        }

        // GET: TrainersModels
        public async Task<IActionResult> Index()
        {
            // Get the trainers
            var trainersModel = await _context.TrainersModel.ToListAsync();

            // Create an updatedTrainersModel array
            var updatedTrainersModel = new TrainersModel[trainersModel.Count];

            int i = 0;

            // For each trainer in trainersModel
            foreach (TrainersModel trainer in trainersModel)
            {
                // Get all the dogs that have been trained by trainer.TrainerId
                var trainingDogCount = await _context.TrainingDogModel.CountAsync(m => m.TrainerId == trainer.TrainerId);

                // Set the training dog count to the trainer dog number
                trainer.DogNumber = trainingDogCount;

                // Set the updated trainer to the updatedTrainersModel
                updatedTrainersModel[i] = trainer;

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(trainer);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                       
                        throw;
                       
                    }
                }

                i++;
            }

            return View(updatedTrainersModel);
        }

        // GET: TrainersModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainersModel = await _context.TrainersModel
                .FirstOrDefaultAsync(m => m.TrainerId == id);
            if (trainersModel == null)
            {
                return NotFound();
            }

            return View(trainersModel);
        }

        // GET: TrainersModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainersModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainerId,TrainerName,TrainerArea,DogNumber")] TrainersModel trainersModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainersModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainersModel);
        }

        // GET: TrainersModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainersModel = await _context.TrainersModel.FindAsync(id);
            if (trainersModel == null)
            {
                return NotFound();
            }
            return View(trainersModel);
        }

        // POST: TrainersModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TrainerId,TrainerName,TrainerArea,DogNumber")] TrainersModel trainersModel)
        {
            if (id != trainersModel.TrainerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainersModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainersModelExists(trainersModel.TrainerId))
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
            return View(trainersModel);
        }

        // GET: TrainersModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainersModel = await _context.TrainersModel
                .FirstOrDefaultAsync(m => m.TrainerId == id);
            if (trainersModel == null)
            {
                return NotFound();
            }

            return View(trainersModel);
        }

        // POST: TrainersModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trainersModel = await _context.TrainersModel.FindAsync(id);
            _context.TrainersModel.Remove(trainersModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainersModelExists(string id)
        {
            return _context.TrainersModel.Any(e => e.TrainerId == id);
        }
    }
}
