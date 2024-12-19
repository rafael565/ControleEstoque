using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Models
{
    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public DbSet<Celular> Celulares { get; set; }

        public DbSet<Chip> Chips { get; set; }
        public DbSet<Computador> Computadores { get; set; }

        public DbSet<Dispositivo> Dispositivos { get; set; }

        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<MonitorEquipamento> MonitorEquipamentos { get; set; }
        public DbSet<Rede> Redes { get; set; }
        public DbSet<Servidor> Servidores { get; set; }
    }
}
