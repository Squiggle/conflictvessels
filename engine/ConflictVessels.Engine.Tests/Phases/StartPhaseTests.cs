using ConflictVessels.Engine.Phases;
using System;
using Xunit;

namespace ConflictVessels.Engine.Tests.Phases;

public class StartPhaseTests
{
    [Fact]
    public void StartPhase_is_complete_when_all_player_boards_are_ready()
    {
        var board1 = new TestPlayerBoard();
        var board2 = new TestPlayerBoard();
        var startPhase = new StartPhase(board1, board2);
        var ready = false;
        startPhase.PhaseState.Subscribe(r => {
            ready = r;
            Console.WriteLine($"Ready: {r}");
        });

        Assert.False(ready, "Start phase should begin unready");

        board1.ReadyUp();
        Assert.False(ready, "Start phase should not be ready until all boards are ready");

        board2.ReadyUp();
        Assert.True(ready, "Start phase should be ready when all boards are ready");
    }
}