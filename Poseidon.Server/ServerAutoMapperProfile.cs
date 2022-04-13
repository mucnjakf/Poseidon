namespace Poseidon.Server;

public class ServerAutoMapperProfile : Profile
{
    public ServerAutoMapperProfile()
    {
        CreateMap<ApplicationUserEntity, ApplicationUserModel>();

        CreateMap<VesselEntity, VesselModel>();
        CreateMap<InsertVesselRequestDto, VesselEntity>().ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<UpdateVesselRequestDto, VesselEntity>().ForMember(x => x.Id, opt => opt.Ignore());
        
        CreateMap<EventEntity, EventModel>();
        CreateMap<InsertEventRequestDto, EventEntity>().ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<LatestEventEntity, LatestEventModel>();
    }
}
