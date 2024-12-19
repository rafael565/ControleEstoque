using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Enum
{
    public enum Status
    {

        [Display(Name = "Ativo")]
        Ativo = 0,


        [Display(Name = "Morto")]
        Morto = 2,


        [Display(Name = "Sucata")]
        Sucata = 1,
    }

}
