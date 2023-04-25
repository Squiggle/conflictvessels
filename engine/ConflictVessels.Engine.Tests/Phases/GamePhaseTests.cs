using ConflictVessels.Engine.Phases;
using System;
using Xunit;

namespace ConflictVessels.Engine.Tests.Phases;

public class GamePhaseTests
{
    [Fact]
    public void GamePhase_is_complete_when_one_player_board_is_defeated()
    {
        var board1 = new TestPlayerBoard();
        var board2 = new TestPlayerBoard();
        var gamePhase = new GamePhase(board1, board2);
        var complete = false;
        gamePhase.PhaseState.Subscribe(r => complete = r);

        Assert.False(complete, "Game phase must not begin completed");

        board1.Lose();
        Assert.True(complete, "Game phase is completed when one player board is defeated");
    }
    
}