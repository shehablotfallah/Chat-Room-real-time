var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

// Add services to the container.

// Bind Pusher settings to options
builder.Services.Configure<PusherSettings>(builder.Configuration.GetSection(nameof(PusherSettings)));
builder.Services.AddCors(options =>
			options.AddDefaultPolicy(policy =>
			policy
			.AllowAnyHeader()
			.AllowAnyMethod()
		.WithOrigins("http://localhost:3000", "https://localhost:44375")));
// Register services
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddSingleton<IPusherService, PusherService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
