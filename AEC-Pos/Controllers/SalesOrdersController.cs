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
    public class SalesOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Services.IRepository _pos;

        public SalesOrdersController(ApplicationDbContext context, Services.IRepository pos)
        {
            _context = context;
            _pos = pos;
        }

        public IActionResult POS() 
        {
            return View();
        }
        // GET: SalesOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SalesOrder.Include(s => s.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SalesOrders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.SalesOrderId == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // GET: SalesOrders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name");
            ViewData["Number"] = _pos.GenerateSONumber();
            return View();
        }

        // POST: SalesOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalesOrderId,Number,Description,SalesOrderDate,CustomerId")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                salesOrder.SalesOrderId = Guid.NewGuid();
                _context.Add(salesOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = salesOrder.SalesOrderId });
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // GET: SalesOrders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", salesOrder.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name");
            return View(salesOrder);
        }

        // POST: SalesOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SalesOrderId,Number,Description,SalesOrderDate,CustomerId")] SalesOrder salesOrder)
        {
            if (id != salesOrder.SalesOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(salesOrder.SalesOrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // GET: SalesOrders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.SalesOrderId == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: SalesOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var salesOrder = await _context.SalesOrder.FindAsync(id);
            List<SalesOrderLine> line = await _context.SalesOrderLine.Where(x => x.SalesOrderId.Equals(id)).ToListAsync();
            List<InvenTran> tran = await _context.InvenTran.Where(x => x.TranSourceNumber.Equals(salesOrder.Number)).ToListAsync();
            _context.InvenTran.RemoveRange(tran);
            _context.SalesOrderLine.RemoveRange(line);
            _context.SalesOrder.Remove(salesOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderExists(Guid id)
        {
            return _context.SalesOrder.Any(e => e.SalesOrderId == id);
        }
    }
}
