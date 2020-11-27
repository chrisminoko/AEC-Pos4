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
    public class GoodsReceives1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoodsReceives1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GoodsReceives1
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GoodsReceive.Include(g => g.PurchaseOrder);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GoodsReceives1/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceive = await _context.GoodsReceive
                .Include(g => g.PurchaseOrder)
                .FirstOrDefaultAsync(m => m.GoodsReceiveId == id);
            if (goodsReceive == null)
            {
                return NotFound();
            }

            return View(goodsReceive);
        }

        // GET: GoodsReceives1/Create
        public IActionResult Create()
        {
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "Number");
            return View();
        }

        // POST: GoodsReceives1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsReceiveId,Number,Description,GoodsReceiveDate,PurchaseOrderId")] GoodsReceive goodsReceive)
        {
            if (ModelState.IsValid)
            {
                goodsReceive.GoodsReceiveId = Guid.NewGuid();
                _context.Add(goodsReceive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "Number", goodsReceive.PurchaseOrderId);
            return View(goodsReceive);
        }

        // GET: GoodsReceives1/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceive = await _context.GoodsReceive.FindAsync(id);
            if (goodsReceive == null)
            {
                return NotFound();
            }
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "Number", goodsReceive.PurchaseOrderId);
            return View(goodsReceive);
        }

        // POST: GoodsReceives1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GoodsReceiveId,Number,Description,GoodsReceiveDate,PurchaseOrderId")] GoodsReceive goodsReceive)
        {
            if (id != goodsReceive.GoodsReceiveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goodsReceive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsReceiveExists(goodsReceive.GoodsReceiveId))
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
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "Number", goodsReceive.PurchaseOrderId);
            return View(goodsReceive);
        }

        // GET: GoodsReceives1/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceive = await _context.GoodsReceive
                .Include(g => g.PurchaseOrder)
                .FirstOrDefaultAsync(m => m.GoodsReceiveId == id);
            if (goodsReceive == null)
            {
                return NotFound();
            }

            return View(goodsReceive);
        }

        // POST: GoodsReceives1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var goodsReceive = await _context.GoodsReceive.FindAsync(id);
            _context.GoodsReceive.Remove(goodsReceive);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsReceiveExists(Guid id)
        {
            return _context.GoodsReceive.Any(e => e.GoodsReceiveId == id);
        }
    }
}
