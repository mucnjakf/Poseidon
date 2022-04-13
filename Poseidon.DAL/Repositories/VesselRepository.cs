namespace Poseidon.DAL.Repositories;

public class VesselRepository : IVesselRepository
{
    private readonly ApplicationDbContext _context;

    public VesselRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<VesselEntity> GetVessels() => _context.Vessels;

    public async Task<VesselEntity> GetVesselAsync(int id)
    {
        VesselEntity vesselEntity = await _context.Vessels.FirstOrDefaultAsync(vessel => vessel.Id == id);

        return vesselEntity;
    }

    public async Task<VesselEntity> GetVesselAsync(string mmsi)
    {
        VesselEntity vesselEntity = await _context.Vessels.FirstOrDefaultAsync(vessel => vessel.Mmsi == mmsi);

        return vesselEntity;
    }

    public async Task<bool> InsertVesselAsync(VesselEntity vesselEntity)
    {
        await _context.Vessels.AddAsync(vesselEntity);

        int numberOfEntriesWritten = await _context.SaveChangesAsync();

        return numberOfEntriesWritten > 0;
    }

    public async Task<bool> UpdateVesselAsync(int id, VesselEntity vesselEntity)
    {
        VesselEntity vesselEntityFromDb = await GetVesselAsync(id);

        if (vesselEntityFromDb is null)
        {
            throw new Exception($"Vessel with ID {id} was not found.");
        }

        vesselEntityFromDb.Mmsi = vesselEntity.Mmsi;
        vesselEntityFromDb.Name = vesselEntity.Name;
        vesselEntityFromDb.Callsign = vesselEntity.Callsign;
        vesselEntityFromDb.Flag = vesselEntity.Flag;
        vesselEntityFromDb.Imo = vesselEntity.Imo;

        int numberOfEntriesWritten = await _context.SaveChangesAsync();

        return numberOfEntriesWritten > 0;
    }

    public async Task<bool> DeleteVesselAsync(int id)
    {
        VesselEntity vesselEntity = await GetVesselAsync(id);

        if (vesselEntity is null)
        {
            throw new Exception($"Vessel with ID {id} was not found.");
        }

        _context.Vessels.Remove(vesselEntity);
        int numberOfEntriesWritten = await _context.SaveChangesAsync();

        return numberOfEntriesWritten > 0;
    }
}