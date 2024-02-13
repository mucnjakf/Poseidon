WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddHangfire(configuration =>
{
    configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireProduction"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        });
});
builder.Services.AddHangfireServer();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultProduction")));

builder.Services.AddDefaultIdentity<ApplicationUserEntity>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});

builder.Services.AddAutoMapper(typeof(ServerAutoMapperProfile));

builder.Services.AddSingleton<EventHub>();

builder.Services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
builder.Services.AddScoped<IValidator<InsertUserRequestDto>, InsertUserRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserEmailRequestDto>, UpdateUserEmailRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserUsernameRequestDto>, UpdateUserUsernameRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserPasswordRequestDto>, UpdateUserPasswordRequestDtoValidator>();
builder.Services.AddScoped<IValidator<InsertVesselRequestDto>, InsertVesselRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateVesselRequestDto>, UpdateVesselRequestDtoValidator>();
builder.Services.AddScoped<IValidator<InsertEventRequestDto>, InsertEventRequestDtoValidator>();

builder.Services.AddScoped<IAccountValidationService, AccountValidationService>();
builder.Services.AddScoped<IVesselValidationService, VesselValidationService>();
builder.Services.AddScoped<IEventValidationService, EventValidationService>();

builder.Services.AddScoped<IVesselRepository, VesselRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IVesselService, VesselService>();
builder.Services.AddScoped<IEventService, EventService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseHangfireDashboard();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<EventHub>("/event-hub");
    endpoints.MapHangfireDashboard();
});

app.MapFallbackToFile("index.html");

app.Run();