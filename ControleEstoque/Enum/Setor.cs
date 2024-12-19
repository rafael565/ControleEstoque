using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Enum
{
    public enum Setor
    {
        [Display(Name = "ALMOXARIFADO")]
        ALMOXARIFADO = 0,

        [Display(Name = "BALANCA")]
        BALANCA = 1,

        [Display(Name = "CEO")]
        CEO = 2,

        [Display(Name = "COMERCIAL")]
        COMERCIAL = 3,

        [Display(Name = "COMPRAS")]
        COMPRAS = 4,

        [Display(Name = "CONTABILIDADE")]
        CONTABILIDADE = 5,

        [Display(Name = "COA")]
        COA = 6,

        [Display(Name = "CUSTOS")]
        CUSTOS = 7,

        [Display(Name = "DIRETORIA")]
        DIRETORIA = 8,

        [Display(Name = "ELETRICA")]
        ELETRICA = 9,

        [Display(Name = "ESCRITÓRIO AGRÍCOLA")]
        ESCRITORIO_AGRICOLA = 10,

        [Display(Name = "FATURAMENTO")]
        FATURAMENTO = 11,

        [Display(Name = "FISCAL")]
        FISCAL = 12,

        [Display(Name = "FINANCEIRO")]
        FINANCEIRO = 13,

        [Display(Name = "GERENTES INDUSTRIAIS")]
        GERENTES_INDUSTRIAIS = 14,

        [Display(Name = "JURÍDICO")]
        JURIDICO = 15,

        [Display(Name = "LABORATÓRIO")]
        LABORATORIO = 16,

        [Display(Name = "OFICINA AGRÍCOLA")]
        OFICINA_AGRICOLA = 17,

        [Display(Name = "PCTS")]
        PCTS = 18,

        [Display(Name = "PIT STOP")]
        PITSTOP = 19,

        [Display(Name = "PROJETOS")]
        PROJETOS = 20,

        [Display(Name = "QUALIDADE")]
        QUALIDADE = 21,

        [Display(Name = "RH")]
        RH = 22,

        [Display(Name = "SELECAO")]
        SELECAO = 23,

        [Display(Name = "TECNOLOGIA")]
        TECNOLOGIA = 24,

        [Display(Name = "PORTARIA")]
        PORTARIA = 25
    }
}
