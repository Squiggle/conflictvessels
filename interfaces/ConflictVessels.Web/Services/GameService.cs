using ConflictVessels.Engine;
using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Web.Services;

public class GameService : IDisposable
{
    private Game? game;

    public Game? CurrentGame => game;

    public void StartNewGame()
    {
        DisposeCurrentGame();

        // Create a default game with auto-placed vessels for now
        var vessels = new Vessel[]
        {
            new Vessel(2),
            new Vessel(3),
            new Vessel(3),
            new Vessel(4),
            new Vessel(5)
        };

        var player1Grid = new AutoGrid(10, 10, vessels);
        var player2Grid = new AutoGrid(10, 10, vessels);

        var arena = new Arena(player1Grid, player2Grid);
        var player1 = new Player();
        var player2 = new Player();

        game = new Game(arena, player1, player2);
    }

    public Grid? GetPlayerGrid(int playerIndex)
    {
        if (game == null || playerIndex < 0 || playerIndex >= game.Arena.Grids.Count)
            return null;

        return game.Arena.Grids[playerIndex];
    }

    private void DisposeCurrentGame()
    {
        game?.Dispose();
        game = null;
    }

    public void Dispose()
    {
        DisposeCurrentGame();
    }
}
