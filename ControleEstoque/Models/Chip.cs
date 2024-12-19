using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ControleEstoque.Enum;

namespace ControleEstoque.Models
{
    [Table("Chips")]
    public class Chip
    {

        [Key]
        [Display(Name = "ID: ")]
        public int id { get; set; }

        [Required(ErrorMessage = "Campo IMEI é obrigatório")]
        [StringLength(35)]
        [Display(Name = "IMEI: ")]
        public string imei { get; set; }


        [Required(ErrorMessage = "Campo nome do responsavel é obrigatório")]
        [StringLength(35)]
        [Display(Name = "Nome responsavel: ")]
        public string nome { get; set; }

        [Display(Name = "Setor: ")]
        public Setor setor { get; set; }

        [Display(Name = "status: ")]
        public Status status { get; set; }
    }

}
