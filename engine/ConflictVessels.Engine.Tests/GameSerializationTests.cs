using System;
using System.Linq;
using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Tests;

/// <summary>
/// Tests for GameSerializationExtensions using in-memory operations only (no filesystem).
/// </summary>
public class GameSerializationTests
{
    [Fact]
    public void ToJson_and_FromJson_successfully_round_trip_default_game()
    {
        // Arrange
        var game = Game.Default();

        // Act
        var json = game.ToJson();
        var deserializedGame = GameSerializationExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedGame);
        Assert.Equal(game.Id, deserializedGame.Id);
        Assert.Equal(game.Phase, deserializedGame.Phase);
        Assert.Equal(game.Players.Count, deserializedGame.Players.Count);
    }

    [Fact]
    public void ToJson_and_FromJson_preserve_game_id()
    {
        // Arrange
        var game = Game.Default();
        var originalId = game.Id;

        // Act
        var json = game.ToJson();
        var deserializedGame = GameSerializationExtensions.FromJson(json);

        // Assert
        Assert.Equal(originalId, deserializedGame.Id);
    }

    [Fact]
    public void ToJson_and_FromJson_preserve_all_players_and_their_ids()
    {
        // Arrange
        var game = Game.Default();
        var originalPlayerIds = game.Players.Select(p => p.Id).ToList();

        // Act
        var json = game.ToJson();
        var deserializedGame = GameSerializationExtensions.FromJson(json);

        // Assert
        Assert.Equal(game.Players.Count, deserializedGame.Players.Count);
        var deserializedPlayerIds = deserializedGame.Players.Select(p => p.Id).ToList();
        Assert.Equal(originalPlayerIds, deserializedPlayerIds);
    }

    [Fact]
    public void ToJson_and_FromJson_preserve_game_phase()
    {
        // Arrange
        var game = Game.Default();
        var originalPhase = game.Phase;

        // Act
        var json = game.ToJson();
        var deserializedGame = GameSerializationExtensions.FromJson(json);

        // Assert
        Assert.Equal(originalPhase, deserializedGame.Phase);
    }

    [Fact]
    public void ToJson_and_FromJson_preserve_arena_structure()
    {
        // Arrange
        var game = Game.Default();
        var originalGridCount = game.Arena.Grids.Count;
        var originalGridDimensions = game.Arena.Grids
            .Select(g => (g.Width, g.Height))
            .ToList();

        // Act
        var json = game.ToJson();
        var deserializedGame = GameSerializationExtensions.FromJson(json);

        // Assert
        Assert.Equal(originalGridCount, deserializedGame.Arena.Grids.Count);
        var deserializedGridDimensions = deserializedGame.Arena.Grids
            .Select(g => (g.Width, g.Height))
            .ToList();
        Assert.Equal(originalGridDimensions, deserializedGridDimensions);
    }

    [Fact]
    public void ToJson_and_FromJson_preserve_placed_vessels()
    {
        // Arrange
        var vessels = new[] { new Vessel(4), new Vessel(3) };
        var grid = new Grid(10, 10, vessels);

        // Place vessels manually
        grid.Vessels[0].Place(2, 3, VesselOrientation.Horizontal);
        grid.Vessels[1].Place(5, 6, VesselOrientation.Vertical);

        var arena = new Arena(new[] { grid });
        var game = new Game(arena, new[] { new Player(), new Player() });

        // Act
        var json = game.ToJson();
        var deserializedGame = GameSerializationExtensions.FromJson(json);

        // Assert
        var originalVessel1 = grid.Vessels[0];
        var deserializedVessel1 = deserializedGame.Arena.Grids[0].Vessels[0];

        Assert.Equal(originalVessel1.Vessel.Size, deserializedVessel1.Vessel.Size);
        Assert.NotNull(deserializedVessel1.Position);
        Assert.Equal(originalVessel1.Position!.Coords.X, deserializedVessel1.Position!.Coords.X);
        Assert.Equal(originalVessel1.Position.Coords.Y, deserializedVessel1.Position.Coords.Y);
        Assert.Equal(originalVessel1.Position.Orientation, deserializedVessel1.Position.Orientation);

        var originalVessel2 = grid.Vessels[1];
        var deserializedVessel2 = deserializedGame.Arena.Grids[0].Vessels[1];

        Assert.Equal(originalVessel2.Vessel.Size, deserializedVessel2.Vessel.Size);
        Assert.NotNull(deserializedVessel2.Position);
        Assert.Equal(originalVessel2.Position!.Coords.X, deserializedVessel2.Position!.Coords.X);
        Assert.Equal(originalVessel2.Position.Coords.Y, deserializedVessel2.Position.Coords.Y);
        Assert.Equal(originalVessel2.Position.Orientation, deserializedVessel2.Position.Orientation);
    }

    [Fact]
    public void ToJson_and_FromJson_preserve_unplaced_vessels_as_null_positions()
    {
        // Arrange
        var vessels = new[] { new Vessel(4), new Vessel(3) };
        var grid = new Grid(10, 10, vessels);
        // Deliberately leave vessels unplaced (Position should be null)

        var arena = new Arena(new[] { grid });
        var game = new Game(arena, new[] { new Player(), new Player() });

        // Act
        var json = game.ToJson();
        var deserializedGame = GameSerializationExtensions.FromJson(json);

        // Assert
        var deserializedVessels = deserializedGame.Arena.Grids[0].Vessels;
        Assert.Equal(2, deserializedVessels.Count);
        Assert.Null(deserializedVessels[0].Position);
        Assert.Null(deserializedVessels[1].Position);
        Assert.Equal(4, deserializedVessels[0].Vessel.Size);
        Assert.Equal(3, deserializedVessels[1].Vessel.Size);
    }

    [Fact]
    public void ToJson_and_FromJson_preserve_player_actions_history()
    {
        // Arrange
        var game = Game.Default();
        game.PlayerActions.Add("Player 1 placed vessel at (2,3)");
        game.PlayerActions.Add("Player 2 placed vessel at (5,6)");
        game.PlayerActions.Add("Player 1 attacked (7,8)");

        // Act
        var json = game.ToJson();
        var deserializedGame = GameSerializationExtensions.FromJson(json);

        // Assert
        Assert.Equal(3, deserializedGame.PlayerActions.Count);
        Assert.Equal("Player 1 placed vessel at (2,3)", deserializedGame.PlayerActions[0]);
        Assert.Equal("Player 2 placed vessel at (5,6)", deserializedGame.PlayerActions[1]);
        Assert.Equal("Player 1 attacked (7,8)", deserializedGame.PlayerActions[2]);
    }

    [Fact]
    public void ToJson_throws_ArgumentNullException_when_game_is_null()
    {
        // Arrange
        Game? nullGame = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullGame!.ToJson());
    }

    [Fact]
    public void FromJson_throws_ArgumentException_when_json_is_empty()
    {
        // Arrange
        var emptyJson = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => GameSerializationExtensions.FromJson(emptyJson));
    }

    [Fact]
    public void FromJson_throws_JsonException_when_json_is_invalid()
    {
        // Arrange
        var invalidJson = "{this is not valid json}";

        // Act & Assert
        Assert.Throws<System.Text.Json.JsonException>(() => GameSerializationExtensions.FromJson(invalidJson));
    }
}
