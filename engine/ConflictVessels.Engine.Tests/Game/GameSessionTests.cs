using Xunit;
using System;
using ConflictVessels.Engine.Game;

namespace ConflictVessels.Engine.Tests.Game;

/// <summary>
/// GameSession facilitates creation, sequencing game phases and termination of a game.
/// A game session requires a sequence of phases; each phase may signal its completion
/// wherein the game session progresses to the next phase.
/// </summary>
public partial class GameSessionTests
{
    [Fact]
    public void Create_game_with_no_phases_throws_argumentexception()
    {
        Assert.Throws<ArgumentException>(() => new GameSession());
    }

    [Fact]
    public void Create_game_session_with_phases_begins_on_first_phase()
    {
        var setup = new TestPhase("setup");
        var complete = new TestPhase("complete");

        var game = new GameSession(setup, complete);

        Assert.NotEqual(Guid.Empty, game.Id);
        Assert.Equal(setup, game.CurrentPhase);
    }

    [Fact]
    public void Game_progresses_to_next_phase_once_current_phase_is_complete()
    {
        var start = new TestPhase("start");
        var end = new TestPhase("end");
        
        var game = new GameSession(start, end);
        start.Completed();

        Assert.Equal(end, game.CurrentPhase);
    }

    [Fact]
    public void Game_session_terminates_when_all_phases_are_complete()
    {
        // arrange
        var phase = new TestPhase("phase");
        var game = new GameSession(phase);
        var active = game.Active.Observe();

        // act
        phase.Completed();

        // assert
        Assert.False(active.Value);
    }
}
