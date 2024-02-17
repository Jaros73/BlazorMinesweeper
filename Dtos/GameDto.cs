
namespace BlazorMinesweeper.Dtos;

/// <summary>
/// Reprezentuje objekt pro přenos dat (DTO) pro entitu hry.
/// </summary>
public class GameDto
{
    /// <summary>
    /// Získá nebo nastaví identifikátor hry.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Získá nebo nastaví název hry.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Získá nebo nastaví stav hry.
    /// </summary>
    public string State { get; set; }
    /// <summary>
    /// Získá nebo nastaví datum a čas vytvoření hry.
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Získá nebo nastaví datum a čas ukončení hry (pokud je k dispozici).
    /// </summary>
    public DateTime? EndDate { get; set; }

}
