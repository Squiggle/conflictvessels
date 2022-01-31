using Checker;
using ConflictVessels.Engine;
using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;

var vessels = new Vessel[] {
  new Vessel(2),
  new Vessel(3),
  new Vessel(3),
  new Vessel(4),
  new Vessel(5)
};

var game = new Game(
  new Arena(new AutoGrid(10, 10, vessels))
);

foreach (var grid in game.Arena.Grids)
{
  grid.Print();
}