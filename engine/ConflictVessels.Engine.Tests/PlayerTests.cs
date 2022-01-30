using System;
using Xunit;

namespace ConflictVessels.Engine.Tests;

public class PlayerTests
{
  [Fact]
  public void Player_is_initialised_with_unique_id()
  {
    var player = new Player();
    Assert.NotEqual(Guid.Empty, player.Id);
  }
}