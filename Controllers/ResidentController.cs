using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareHomeInfoTracker.Data;
using CareHomeInfoTracker.Models;
using Microsoft.AspNetCore.Authorization;
using CareHomeInfoTracker.Services.ImageFiles;

namespace CareHomeInfoTracker.Controllers
{
    [Authorize]
    public class ResidentController : Controller
    {
        private readonly CareHomeInfoContext _context;
        private readonly IFileUploadService _fileUploadService;

        public ResidentController(CareHomeInfoContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
        }

        // GET: Residents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Residents
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.MiddleName)
                .ThenBy(x => x.LastName)
                .ToListAsync());
        }

        // GET: Residents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resident = await _context.Residents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            return View(resident);
        }

        public IActionResult ChartOrDetail(int? id, int chart = 0) 
        {

            if (id == null)
            {
                return NotFound();
            }

            var resident = _context.Residents
                .FirstOrDefault(m => m.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            if(chart == 0)
            {
                return PartialView("_ResDetailPartial", resident);
            }

            var weightHistory = _context.WeightHistories
                .Where(x => x.Resident == resident)
                .OrderByDescending(x => x.RecordedDate)
                .ToList();
            return PartialView("_ChartPartial", weightHistory);
        }
        public async Task<IActionResult> IndividualChart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resident = await _context.Residents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            var weightHistory = await _context.WeightHistories
                .Where(x => x.Resident == resident)
                .OrderByDescending(x => x.RecordedDate)
                .ToListAsync();
            return View(weightHistory);
        }

        // GET: Residents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Residents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,RoomNum,BedNum, ResImage, ImgLocation")] Resident resident, IFormFile resImage)
        {
            if (ModelState.IsValid)
            {
                if(resImage != null && resImage.Length > 0)
                {
                    try
                    {
                        var imagePath = await _fileUploadService.SaveImageAsync(resImage, @"D:\MeroBucket\Images", 3);
                        resident.ImgLocation = imagePath;
                    }
                    catch (Exception e) 
                    {
                        ModelState.AddModelError("ResImage", e.Message);
                        return View(resident);
                    }
                }
                _context.Add(resident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resident);
        }

        // GET: Residents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resident = await _context.Residents.FindAsync(id);
            if (resident == null)
            {
                return NotFound();
            }
            return View(resident);
        }

        // POST: Residents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,RoomNum,BedNum")] Resident resident)
        {
            if (id != resident.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResidentExists(resident.Id))
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
            return View(resident);
        }

        // GET: Residents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resident = await _context.Residents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            return View(resident);
        }

        // POST: Residents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resident = await _context.Residents.FindAsync(id);
            if (resident != null)
            {
                _context.Residents.Remove(resident);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResidentExists(int id)
        {
            return _context.Residents.Any(e => e.Id == id);
        }
    }
}


//public async Task<IActionResult> Create([Bind("Id,....")] Resident resident, IFormFile resImage)