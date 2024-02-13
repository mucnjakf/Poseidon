namespace Poseidon.Server.Services;

public class VesselService : IVesselService
{
    private readonly IMapper _mapper;
    private readonly IVesselValidationService _vesselValidationService;
    private readonly IVesselRepository _vesselRepository;

    public VesselService(IMapper mapper, IVesselValidationService vesselValidationService, IVesselRepository vesselRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _vesselValidationService = vesselValidationService ?? throw new ArgumentNullException(nameof(vesselValidationService));
        _vesselRepository = vesselRepository ?? throw new ArgumentNullException(nameof(vesselRepository));
    }

    public GetVesselsResponseDto GetVessels()
    {
        IEnumerable<VesselModel> vessels = _vesselRepository.GetVessels().Select(vessel => _mapper.Map<VesselModel>(vessel));

        return new GetVesselsResponseDto { Result = vessels };
    }

    public async Task<GetVesselResponseDto> GetVesselAsync(int id)
    {
        if (id <= 0)
        {
            return new GetVesselResponseDto { Successful = false, Error = "ID is not valid." };
        }

        VesselEntity vesselEntity = await _vesselRepository.GetVesselAsync(id);

        if (vesselEntity is null)
        {
            return new GetVesselResponseDto { Successful = false, Error = $"Vessel with ID {id} was not found." };
        }

        VesselModel vesselModel = _mapper.Map<VesselModel>(vesselEntity);

        return new GetVesselResponseDto { Successful = true, VesselModel = vesselModel };
    }

    public async Task<GetVesselResponseDto> GetVesselAsync(string mmsi)
    {
        if (string.IsNullOrWhiteSpace(mmsi))
        {
            return new GetVesselResponseDto { Successful = false, Error = "MMSI is not valid." };
        }

        VesselEntity vesselEntity = await _vesselRepository.GetVesselAsync(mmsi);

        if (vesselEntity is null)
        {
            return new GetVesselResponseDto { Successful = false, Error = $"Vessel with MMSI {mmsi} was not found." };
        }

        VesselModel vesselModel = _mapper.Map<VesselModel>(vesselEntity);

        return new GetVesselResponseDto { Successful = true, VesselModel = vesselModel };
    }

    public async Task<InsertVesselResponseDto> InsertVesselAsync(InsertVesselRequestDto insertVesselRequestDto)
    {
        (bool successful, IEnumerable<string> errors) = await _vesselValidationService.ValidateInsertVesselRequestDto(insertVesselRequestDto);

        if (!successful)
        {
            return new InsertVesselResponseDto { Successful = false, Errors = errors };
        }

        try
        {
            VesselEntity vesselEntity = _mapper.Map<VesselEntity>(insertVesselRequestDto);

            bool succeeded = await _vesselRepository.InsertVesselAsync(vesselEntity);

            return succeeded
                ? new InsertVesselResponseDto { Successful = true }
                : new InsertVesselResponseDto { Successful = false };
        }
        catch (Exception e)
        {
            return new InsertVesselResponseDto { Successful = false, Errors = new List<string> { e.Message } };
        }
    }

    public async Task<UpdateVesselResponseDto> UpdateVesselAsync(int id, UpdateVesselRequestDto updateVesselRequestDto)
    {
        (bool successful, IEnumerable<string> errors) = await _vesselValidationService.ValidateUpdateVesselRequestDto(updateVesselRequestDto);

        if (!successful)
        {
            return new UpdateVesselResponseDto { Successful = false, Errors = errors };
        }

        try
        {
            VesselEntity vesselEntity = _mapper.Map<VesselEntity>(updateVesselRequestDto);

            bool succeeded = await _vesselRepository.UpdateVesselAsync(id, vesselEntity);

            return succeeded
                ? new UpdateVesselResponseDto { Successful = true }
                : new UpdateVesselResponseDto { Successful = false };
        }
        catch (Exception e)
        {
            return new UpdateVesselResponseDto { Successful = false, Errors = new List<string> { e.Message } };
        }
    }

    public async Task<DeleteVesselResponseDto> DeleteVesselAsync(int id)
    {
        if (id <= 0)
        {
            return new DeleteVesselResponseDto { Successful = false, Errors = new List<string> { "ID is not valid." } };
        }

        try
        {
            bool succeeded = await _vesselRepository.DeleteVesselAsync(id);

            return succeeded
                ? new DeleteVesselResponseDto { Successful = true }
                : new DeleteVesselResponseDto { Successful = false };
        }
        catch (Exception e)
        {
            return new DeleteVesselResponseDto { Successful = false, Errors = new List<string> { e.Message } };
        }
    }
}