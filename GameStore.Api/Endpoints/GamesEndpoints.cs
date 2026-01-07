using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    
    public static class GamesEndpoints
    {
        public static void MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/games");

            //GET
            group.MapGet("/", async (GameStoreContext dbContext) => 
                await  dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => new GameSummaryDto(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate
                    )).ToListAsync());

            //GET by id
            group.MapGet("/{id:int}", async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                return game is not null ? Results.Ok(
                    new GameDetailsDto(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate
                        )
                    ) : Results.NotFound();
            })
            .WithName("GetGameById")
            .WithTags("Games");

            //POST
            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
                {
                    Game game = new()
                    {
                        Name = newGame.name,
                        GenreId = newGame.genreId,
                        Price = newGame.price,
                        ReleaseDate = newGame.releaseDate
                    };
                    
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                    GameDetailsDto gameDto = new(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate
                    );
                
                return Results.Created($"/games/{game.Id}", gameDto);
            })
            .WithName("CreateGame")
            .WithTags("Games");

            //PUT
            group.MapPut("/{id:int}", async (int id, UpdateGameDto updateGameDto, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);
                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                existingGame.Name = updateGameDto.name;
                existingGame.GenreId = updateGameDto.genreId;
                existingGame.Price = updateGameDto.price;
                existingGame.ReleaseDate = updateGameDto.releaseDate;
                
                await dbContext.SaveChangesAsync();
                
                return Results.NoContent();
            })
            .WithName("UpdateGame")
            .WithTags("Games");

            //DELETE
            group.MapDelete("/{id:int}", async (int id, GameStoreContext dbContext) =>
            {
                await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            })
            .WithName("DeleteGame")
            .WithTags("Games");           

        }
    }




}