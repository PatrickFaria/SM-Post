using Confluent.Kafka;
using CQRS_Core.Domain;
using CQRS_Core.Handlers;
using CQRS_Core.Infrastructure;
using CQRS_Core.Producers;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispacthers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Producers;
using Post.Cmd.Infrastructure.Repository;
using Post.Cmd.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

// register command handler methods
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispacther = new CommandDispatcher();
dispacther.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
dispacther.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
dispacther.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
dispacther.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
dispacther.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
dispacther.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
dispacther.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispacther);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
