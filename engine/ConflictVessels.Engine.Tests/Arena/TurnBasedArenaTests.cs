// using System;
// using ConflictVessels.Engine;
// using ConflictVessels.Engine.Arena;
// using Xunit;

// public class TurnBasedArenaTests
// {
//   [Fact]
//   public void Player_one_goes_first()
//   {
    
//     var arena = new TurnBasedArena();
//   }

//   [Fact]
//   public void Arena_raises_ready_changed_event_when_all_grids_populated()
//   {
//     // creating an Arena with one AutoGrid
//     var grid = new TestGrid();
//     var arena = new Arena(grid);

//     Assert.False(arena.Ready);
//     var readyObserved = false;
//     arena.ObservableReady.Subscribe(x => readyObserved = x);
//     // act
//     grid.ReadyUp();
//   }

//   public class TestGrid : Grid
//   {
//     public TestGrid() : base(10, 10) { }
//     public bool ReadyUp() => Ready = true;
//   }
// }
