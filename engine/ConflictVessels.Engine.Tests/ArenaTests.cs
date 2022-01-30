using System;
using ConflictVessels.Engine;
using ConflictVessels.Engine.Grids;
using Xunit;

public class ArenaTests
{
  [Fact]
  public void Default_arena_created_with_two_grids()
  {
    var arena = Arena.Default();
    Assert.Equal(2, arena.Grids.Count);
    Assert.False(arena.Ready);
  }

  [Fact]
  public void Arena_raises_ready_changed_event_when_all_grids_populated()
  {
    // creating an Arena with one AutoGrid
    var grid = new TestGrid();
    var arena = new Arena(grid);

    Assert.False(arena.Ready);
    var readyObserved = false;
    arena.ObservableReady.Subscribe(x => readyObserved = x);
    // act
    grid.ReadyUp();
  }

  public class TestGrid : Grid
  {
    public bool ReadyUp() => Ready = true;
  }
}
