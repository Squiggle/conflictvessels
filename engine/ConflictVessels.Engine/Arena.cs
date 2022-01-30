namespace ConflictVessels.Engine;

/// <summary>Stateful</summary>
public class Arena
{
  /// <summary>
  /// Grids are presented as a Dictionary
  /// allowing them to be referenced by key
  /// </summary>
  public Dictionary<string, Grid> Grids { get; init; }

  internal Arena()
  {
    Grids = new Dictionary<string, Grid> {
      { "1", new Grid() },
      { "2", new Grid() }
    };
  }
}