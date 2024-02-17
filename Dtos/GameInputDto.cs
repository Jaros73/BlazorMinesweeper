using System.ComponentModel.DataAnnotations;

namespace BlazorMinesweeper.Dtos;

/// <summary>
/// Data objektu (DTO) pro vstupní data pro vytvoření nové hry.
/// </summary>
public class GameInputDto
{
    /// <summary>
    /// Název hry.
    /// </summary>
    [Required(ErrorMessage = "Název hry je povinný")]
    [MaxLength(25, ErrorMessage = "Název hry nesmí být delší než 25 znaků")]
    public string Name { get; set; }

    /// <summary>
    /// Počet min v herním poli.
    /// </summary>
    [Required(ErrorMessage = "Počet min je povinný")]
    [Range(1, 99, ErrorMessage = "Počet min musí být kladné číslo menší než 100")]
    public int MinesCount { get; set; }
}
