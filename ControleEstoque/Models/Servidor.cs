﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ControleEstoque.Enum;

namespace ControleEstoque.Models
{
    [Table("Servidores")]
    public class Servidor
    {

        [Key]
        [Display(Name = "ID: ")]
        public int id { get; set; }

        [Required(ErrorMessage = "Campo modelo é obrigatório")]
        [StringLength(35)]
        [Display(Name = "Modelo: ")]
        public string modelo { get; set; }

        [Required(ErrorMessage = "Campo serie é obrigatório")]
        [StringLength(35)]
        [Display(Name = "Serie do dispositivo: ")]
        public string serie { get; set; }


        [Display(Name = "Patrimonio: ")]
        public string patrimonio { get; set; }

        [Required(ErrorMessage = "Campo Quantidade é obrigatório")]

        [Display(Name = "Quantidade: ")]
        public int quantidade { get; set; }

        [Display(Name = "status: ")]
        public Status status { get; set; }
    }
}
