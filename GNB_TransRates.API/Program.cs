using GNB_TransRates.API.Middlewares;
using GNB_TransRates.DAL.Contexts;
using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Infrastructure;
using GNB_TransRates.DL.Repositories;
using GNB_TransRates.DL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<AppDbContext>(con => con.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));

builder.Services.AddTransient<IBaseRepository<Rates>, BaseRepository<Rates>>();
builder.Services.AddTransient<IBaseRepository<Transactions>, BaseRepository<Transactions>>();
builder.Services.AddTransient<IBaseService<Rates>, BaseService<Rates>>();
builder.Services.AddTransient<IBaseService<Transactions>, BaseService<Transactions>>();
builder.Services.AddTransient<IErrorHandler, ErrorHandler>();
builder.Services.AddTransient<IRatesService, RatesService>();
builder.Services.AddTransient<ITransactionsService, TransactionsService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();

app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.MapControllers();

app.UseSwaggerUI();

app.Run();
