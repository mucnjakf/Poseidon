WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.AddScoped<IValidator<InsertUserRequestDto>, InsertUserRequestDtoValidator>();
builder.Services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserEmailRequestDto>, UpdateUserEmailRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserUsernameRequestDto>, UpdateUserUsernameRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserPasswordRequestDto>, UpdateUserPasswordRequestDtoValidator>();

builder.Services.AddScoped<IValidator<InsertVesselRequestDto>, InsertVesselRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateVesselRequestDto>, UpdateVesselRequestDtoValidator>();

builder.Services.AddScoped<IAccountApiService, AccountApiService>();
builder.Services.AddScoped<IVesselApiService, VesselApiService>();
builder.Services.AddScoped<IEventApiService, EventApiService>();

builder.Services.AddSingleton<EventsNotificationService>();

await builder.Build().RunAsync();