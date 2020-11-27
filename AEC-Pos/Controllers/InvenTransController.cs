using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AEC_Pos.Data;
using AEC_Pos.Models;

namespace AEC_Pos.Controllers
{
    public class InvenTransController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvenTransController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InvenTrans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InvenTran.Include(i => i.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: InvenTrans/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invenTran = await _context.InvenTran
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.InvenTranId == id);
            if (invenTran == null)
            {
                return NotFound();
            }

            return View(invenTran);
        }

        // GET: InvenTrans/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name");
            return View();
        }

        // POST: InvenTrans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvenTranId,Number,Description,ProductId,TranSourceId,TranSourceNumber,TranSourceType,Quantity,InvenTranDate")] InvenTran invenTran)
        {
            if (ModelState.IsValid)
            {
                invenTran.InvenTranId = Guid.NewGuid();
                _context.Add(invenTran);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", invenTran.ProductId);
            return View(invenTran);
        }

        // GET: InvenTrans/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invenTran = await _context.InvenTran.FindAsync(id);
            if (invenTran == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", invenTran.ProductId);
            return View(invenTran);
        }

        // POST: InvenTrans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InvenTranId,Number,Description,ProductId,TranSourceId,TranSourceNumber,TranSourceType,Quantity,InvenTranDate")] InvenTran invenTran)
        {
            if (id != invenTran.InvenTranId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invenTran);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvenTranExists(invenTran.InvenTranId))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", invenTran.ProductId);
            return View(invenTran);
        }

        // GET: InvenTrans/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invenTran = await _context.InvenTran
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.InvenTranId == id);
            if (invenTran == null)
            {
                return NotFound();
            }

            return View(invenTran);
        }

        // POST: InvenTrans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var invenTran = await _context.InvenTran.FindAsync(id);
            _context.InvenTran.Remove(invenTran);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvenTranExists(Guid id)
        {
            return _context.InvenTran.Any(e => e.InvenTranId == id);
        }
    }
}
