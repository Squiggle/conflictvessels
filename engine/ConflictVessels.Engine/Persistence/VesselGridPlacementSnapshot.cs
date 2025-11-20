using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Persistence;

/// <summary>
/// Snapshot DTO for VesselGridPlacement persistence.
/// Separates serialization concerns from the domain model.
/// </summary>
public record VesselGridPlacementSnapshot
{
    public Vessel Vessel { get; init; } = null!;
    public VesselPositionSnapshot? Position { get; init; }
}

/// <summary>
/// Extension methods for VesselGridPlacement snapshot creation and restoration.
/// </summary>
public static class VesselGridPlacementPersistence
{
    public static VesselGridPlacementSnapshot CreateSnapshot(this VesselGridPlacement placement)
    {
        if (placement == null) throw new ArgumentNullException(nameof(placement));

        return new VesselGridPlacementSnapshot
        {
            Vessel = placement.Vessel,
            Position = placement.Position?.CreateSnapshot()
        };
    }

    public static VesselGridPlacement RestoreFromSnapshot(VesselGridPlacementSnapshot snapshot)
    {
        if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));

        var position = snapshot.Position != null
            ? VesselPositionPersistence.RestoreFromSnapshot(snapshot.Position)
            : null;

        return new VesselGridPlacement(snapshot.Vessel, position);
    }
}
