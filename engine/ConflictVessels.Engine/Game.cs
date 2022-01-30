namespace ConflictVessels.Engine;

using System;

public class Game
{
  /// <summary>Identifier</summary>
  public Guid Id { get; init; }

  /// <summary>Game is Active</summary>
  public bool Active { get; init; }

  /// <summary>Total player count</summary>
  public int PlayerCount { get; init; }

  /// <summary>Monitor changes to the Game's Arena</summary>
  public event EventHandler? ArenaCreated;

  /// <summary>Create a new Game with defaults</summary>
  public static Game Create()
  {
    return new Game
    {
      Id = Guid.NewGuid(),
      Active = true,
      PlayerCount = 2
    };
  }
}