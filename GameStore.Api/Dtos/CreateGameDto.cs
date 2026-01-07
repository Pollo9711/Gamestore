using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos
{
    public record CreateGameDto(
        [Required][StringLength(50)] string name,
        [Range(1,50)] int genreId,
        [Range(1,100)] decimal price,
        DateTime releaseDate
        );
}
