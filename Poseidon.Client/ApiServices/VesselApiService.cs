namespace Poseidon.Client.ApiServices;

public class VesselApiService : IVesselApiService
{
    private readonly HttpClient _httpClient;

    public VesselApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<GetVesselsResponseDto> GetVesselsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/vessel");

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        GetVesselsResponseDto response = JsonSerializer.Deserialize<GetVesselsResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task<GetVesselResponseDto> GetVesselAsync(string mmsi)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"api/vessel/mmsi/{mmsi}");

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        GetVesselResponseDto response = JsonSerializer.Deserialize<GetVesselResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }
    
    public async Task<InsertVesselResponseDto> InsertVesselAsync(InsertVesselRequestDto insertVesselRequestDto)
    {
        StringContent data = new(JsonSerializer.Serialize(insertVesselRequestDto), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await _httpClient.PostAsync("api/vessel", data);

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        InsertVesselResponseDto response = JsonSerializer.Deserialize<InsertVesselResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task<UpdateVesselResponseDto> UpdateVesselAsync(int id, UpdateVesselRequestDto updateVesselRequestDto)
    {
        StringContent data = new(JsonSerializer.Serialize(updateVesselRequestDto), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await _httpClient.PutAsync($"api/vessel/{id}", data);

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        UpdateVesselResponseDto response = JsonSerializer.Deserialize<UpdateVesselResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task<DeleteVesselResponseDto> DeleteVesselAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"api/vessel/{id}");

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        DeleteVesselResponseDto userResponse = JsonSerializer.Deserialize<DeleteVesselResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return userResponse;
    }
}