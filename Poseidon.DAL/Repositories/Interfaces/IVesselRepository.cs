namespace Poseidon.DAL.Repositories.Interfaces;

public interface IVesselRepository
{
    IEnumerable<VesselEntity> GetVessels();

    Task<VesselEntity> GetVesselAsync(int id);

    Task<VesselEntity> GetVesselAsync(string mmsi);

    Task<bool> InsertVesselAsync(VesselEntity vesselEntity);

    Task<bool> UpdateVesselAsync(int id, VesselEntity vesselEntity);

    Task<bool> DeleteVesselAsync(int id);
}