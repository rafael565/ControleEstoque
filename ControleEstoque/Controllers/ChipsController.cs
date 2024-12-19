using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleEstoque.Models;
using ClosedXML.Excel;
using System.Data;
using System.IO;

namespace ControleEstoque.Controllers
{
    public class ChipsController : Controller
    {
        private readonly Contexto _context;

        public ChipsController(Contexto context)
        {
            _context = context;
        }

        // GET: Chips
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chips.ToListAsync());
        }

        [HttpGet]
        public async Task<FileResult> ExportarChipsAExcel() // Corrigido o nome da ação
        {
            var chips = await _context.Chips.ToListAsync(); // Corrigido de "Celulares" para "Chips"
            var nomeArchivo = $"Chips.xlsx";
            return GenerarExcel(nomeArchivo, chips); // Corrigido para passar a lista de chips
        }

        private FileResult GenerarExcel(string nomeArchivo, IEnumerable<Chip> chips)
        {
            DataTable dataTable = new DataTable("Chips");
            dataTable.Columns.AddRange(new DataColumn[] {
                new DataColumn("id"),
                new DataColumn("imei"),
                new DataColumn("nome"),
                new DataColumn("setor"),
                new DataColumn("status"),
            });

            // Corrigido de "celulares" para "chips" e os campos foram ajustados
            foreach (var chip in chips)
            {
                dataTable.Rows.Add(chip.id,
                    chip.imei,
                    chip.nome,
                    chip.setor,
                    chip.status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dataTable);  // Corrigido: definindo 'ws' como a worksheet

                // Adiciona uma linha no final com o total de dispositivos
                var lastRow = dataTable.Rows.Count + 2;  // Fica 2 linhas abaixo do último item
                ws.Cell(lastRow, 1).Value = "Total de Dispositivos:";
                ws.Cell(lastRow, 2).Value = chips.Count();  // Conta o número total de celulares

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArchivo);
                }
            }
        }

        // GET: Chips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chip = await _context.Chips
                .FirstOrDefaultAsync(m => m.id == id);
            if (chip == null)
            {
                return NotFound();
            }

            return View(chip);
        }

        // GET: Chips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chips/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,imei,nome,setor,status")] Chip chip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chip);
        }

        // GET: Chips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chip = await _context.Chips.FindAsync(id);
            if (chip == null)
            {
                return NotFound();
            }
            return View(chip);
        }

        // POST: Chips/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,imei,nome,setor,status")] Chip chip)
        {
            if (id != chip.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChipExists(chip.id))
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
            return View(chip);
        }

        // GET: Chips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chip = await _context.Chips
                .FirstOrDefaultAsync(m => m.id == id);
            if (chip == null)
            {
                return NotFound();
            }

            return View(chip);
        }

        // POST: Chips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chip = await _context.Chips.FindAsync(id);
            if (chip != null)
            {
                _context.Chips.Remove(chip);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChipExists(int id)
        {
            return _context.Chips.Any(e => e.id == id);
        }
    }
}
