namespace ConflictVessels.Engine;

public class Player
{

  public Guid Id { get; init; }
  public Player()
  {
    Id = Guid.NewGuid();
  }
}