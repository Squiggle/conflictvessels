using Xunit;
using ConflictVessels.Engine;
using System;

namespace ConflictVessels.Engine.Tests;

public class GameTests
{
  [Fact]
  public void Create_game_initialises_new_game_with_players()
  {
    var player1 = new Player();
    var player2 = new Player();
    var game = Game.Default();

    // game should be initialised with
    // - an ID
    // - two players
    // - an Arena
    // - the game should be in progress
    // - no gameplay actions can yet be taken
    Assert.NotEqual(Guid.Empty, game.Id);
    Assert.Equal(2, game.Players.Count);
    Assert.NotNull(game.Arena);
    Assert.True(game.Active);
    Assert.Empty(game.PlayerActions);
  }

  [Fact]
  public void Game_can_be_abandoned_by_player()
  {
    var game = Game.Default();
    Assert.True(game.Active);

    // player one abandons the game
    var byPlayer = game.Players[0];
    game.Abandon(byPlayer);

    // game is now inactive
    Assert.False(game.Active);
  }

  [Fact]
  public void Game_commenses_battle_when_all_vessels_have_been_placed()
  {
    // arrange
    var testArena = new TestArena();
    var game = new Game(testArena, new Player());

    GamePhase? phaseObserved = null;
    game.ObservablePhase.Subscribe(phase => phaseObserved = phase);

    // act
    testArena.ReadyUp();

    // assert
    Assert.Equal(GamePhase.Action, game.Phase);
    Assert.Equal(GamePhase.Action, phaseObserved);
    Assert.True(game.Active);
  }

  internal class TestArena : Arena
  {
    public void ReadyUp() => Ready = true;

    public void NotReady() => Ready = false;
  }
}
