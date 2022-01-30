using System.Linq;
using ConflictVessels.Engine;
using Xunit;

public class ArenaTests
{
  [Fact]
  public void Arena_created_with_two_grids_of_equal_size()
  {
    var arena = new Arena();
    Assert.Equal(2, arena.Grids.Count);

    var gridOne = arena.Grids.First().Value;
    Assert.Equal(10, gridOne.Width);
    Assert.Equal(10, gridOne.Height);
    var gridTwo = arena.Grids.Skip(1).First().Value;
    Assert.Equal(10, gridTwo.Width);
    Assert.Equal(10, gridTwo.Height);
  }

}