namespace GameStore.Api.Dtos
{

    public record GameDetailsDto(
        int Id,
        string name,
        int genreId,
        decimal price,
        DateTime releaseDate
    );
}   