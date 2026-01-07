namespace GameStore.Api.Dtos
{
    public record GameSummaryDto
    (
        int Id,
            string name,
            string genre,
            decimal price,
            DateTime releaseDate
    );
}
