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
using System.IO;  // Adicionando o 'using' para usar 'MemoryStream'

namespace ControleEstoque.Controllers
{
    public class DispositivosController : Controller
    {
        private readonly Contexto _context;

        public DispositivosController(Contexto context)
        {
            _context = context;
        }

        // GET: Dispositivos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dispositivos.ToListAsync());
        }

        [HttpGet]
        public async Task<FileResult> ExportarDispositivosAExcel()
        {
            var dispositivos = await _context.Dispositivos.ToListAsync();
            var nomeArquivo = $"Dispositivos.xlsx";
            return GenerarExcel(nomeArquivo, dispositivos);
        }

        private FileResult GenerarExcel(string nomeArquivo, IEnumerable<Dispositivo> dispositivos)
        {
            DataTable dataTable = new DataTable("Dispositivos");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("id"),
                new DataColumn("modelo"),
                new DataColumn("serie"),
                new DataColumn("patrimonio"),
                new DataColumn("quantidade"),
                new DataColumn("nome"),
                new DataColumn("setor"),
                new DataColumn("status"),
            });

            // Corrigindo o nome da variável de 'celulares' para 'dispositivos'
            foreach (var dispositivo in dispositivos)
            {
                dataTable.Rows.Add(dispositivo.id,
                    dispositivo.modelo,
                    dispositivo.serie,
                    dispositivo.patrimonio,
                    dispositivo.quantidade,
                    dispositivo.nome,
                    dispositivo.setor,
                    dispositivo.status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dataTable);  // Corrigido: definindo 'ws' como a worksheet

                // Adiciona uma linha no final com o total de dispositivos
                var lastRow = dataTable.Rows.Count + 2;  // Fica 2 linhas abaixo do último item
                ws.Cell(lastRow, 1).Value = "Total de Dispositivos:";
                ws.Cell(lastRow, 2).Value = dispositivos.Count();  // Conta o número total de computadores

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArquivo);
                }
            }
        }

        // GET: Dispositivos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivos
                .FirstOrDefaultAsync(m => m.id == id);
            if (dispositivo == null)
            {
                return NotFound();
            }

            return View(dispositivo);
        }

        // GET: Dispositivos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dispositivos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispositivo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispositivo);
        }

        // GET: Dispositivos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivos.FindAsync(id);
            if (dispositivo == null)
            {
                return NotFound();
            }
            return View(dispositivo);
        }

        // POST: Dispositivos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] Dispositivo dispositivo)
        {
            if (id != dispositivo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispositivo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispositivoExists(dispositivo.id))
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
            return View(dispositivo);
        }

        // GET: Dispositivos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivos
                .FirstOrDefaultAsync(m => m.id == id);
            if (dispositivo == null)
            {
                return NotFound();
            }

            return View(dispositivo);
        }

        // POST: Dispositivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dispositivo = await _context.Dispositivos.FindAsync(id);
            if (dispositivo != null)
            {
                _context.Dispositivos.Remove(dispositivo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DispositivoExists(int id)
        {
            return _context.Dispositivos.Any(e => e.id == id);
        }
    }
}
