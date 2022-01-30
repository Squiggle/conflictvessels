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

  /// <summary>Arena for the Game</summary>
  public Arena Arena { get; init; }

  private Game()
  {
    // defaults can be overridden via params if needed
    Id = Guid.NewGuid();
    Active = true;
    PlayerCount = 2;
    Arena = new Arena();
  }

  /// <summary>Create a new Game with defaults</summary>
  public static Game Create()
  {
    // factory method
    // load and set defaults here
    return new Game();
  }
}