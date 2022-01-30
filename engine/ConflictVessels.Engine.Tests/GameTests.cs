using Xunit;
using ConflictVessels.Engine;
using System;

namespace ConflictVessels.Engine.Tests;

public class GameTests
{
  [Fact]
  public void Create_game_initialises_new_game()
  {
    var game = Game.Create();

    // game should be initialised with
    // - an ID
    // - two players
    // - an 'Arena Created' event
    // - the game should be in progress
    Assert.NotNull(game.Id);
    Assert.Equal(2, game.PlayerCount);
    game.ArenaCreated += (o, a) => Console.WriteLine(o?.ToString());
    Assert.True(game.Active);
  }
}
