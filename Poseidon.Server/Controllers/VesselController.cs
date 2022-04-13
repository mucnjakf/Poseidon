namespace Poseidon.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/vessel")]
public class VesselController : ControllerBase
{
    private readonly IVesselService _vesselService;

    public VesselController(IVesselService vesselService)
    {
        _vesselService = vesselService ?? throw new ArgumentNullException(nameof(vesselService));
    }

    [HttpGet]
    public ActionResult<GetVesselsResponseDto> GetVessels()
    {
        GetVesselsResponseDto getVesselsResponseDto = _vesselService.GetVessels();

        return Ok(getVesselsResponseDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetVesselResponseDto>> GetVesselAsync([FromRoute] int id)
    {
        GetVesselResponseDto getVesselResponseDto = await _vesselService.GetVesselAsync(id);

        return getVesselResponseDto.Successful ? Ok(getVesselResponseDto) : BadRequest(getVesselResponseDto);
    }

    [HttpGet("mmsi/{mmsi:int}")]
    public async Task<ActionResult<GetVesselResponseDto>> GetVesselByMmsiAsync([FromRoute] int mmsi)
    {
        GetVesselResponseDto getVesselResponseDto = await _vesselService.GetVesselAsync(mmsi.ToString());

        return getVesselResponseDto.Successful ? Ok(getVesselResponseDto) : BadRequest(getVesselResponseDto);
    }

    [HttpPost]
    public async Task<ActionResult<InsertVesselResponseDto>> InsertVesselAsync([FromBody] InsertVesselRequestDto insertVesselRequestDto)
    {
        InsertVesselResponseDto insertVesselResponseDto = await _vesselService.InsertVesselAsync(insertVesselRequestDto);

        return insertVesselResponseDto.Successful ? Ok(insertVesselResponseDto) : BadRequest(insertVesselResponseDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UpdateVesselResponseDto>> UpdateVesselAsync(
        [FromRoute] int id,
        [FromBody] UpdateVesselRequestDto updateVesselRequestDto)
    {
        UpdateVesselResponseDto updateVesselResponseDto = await _vesselService.UpdateVesselAsync(id, updateVesselRequestDto);

        return updateVesselResponseDto.Successful ? Ok(updateVesselResponseDto) : BadRequest(updateVesselResponseDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<DeleteVesselResponseDto>> DeleteVesselAsync([FromRoute] int id)
    {
        DeleteVesselResponseDto deleteVesselResponseDto = await _vesselService.DeleteVesselAsync(id);

        return deleteVesselResponseDto.Successful ? Ok(deleteVesselResponseDto) : BadRequest(deleteVesselResponseDto);
    }
}