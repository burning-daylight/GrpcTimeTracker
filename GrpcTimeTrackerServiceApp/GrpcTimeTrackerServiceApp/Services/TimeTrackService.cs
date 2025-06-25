using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcTimeTrackerServiceApp.Data;
using GrpcTimeTrackerServiceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcTimeTrackerServiceApp.Services;

public class TimeTrackService : TimeTracker.TimeTrackerBase
{
    private readonly AppDbContext _dbContext;

    public TimeTrackService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<ItemReply> Insert(InsertRequest request, ServerCallContext context)
    {
        if (request.Title == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Please, supply a valid object"));

        var item = new ActiveItem { Title = request.Title, TimeSpent = request.Timespent.ToTimeSpan() }; 

        await _dbContext.AddAsync(item);
        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new ItemReply
        {
            Id = item.Id,
            Title = item.Title,
            Timespent = Duration.FromTimeSpan(item.TimeSpent)
        });
    }

    public override async Task<ItemReply> Update(UpdateRequest request, ServerCallContext context)
    {
        if (request.Id <= 0 || request.Title == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Please, supply a valid object"));

        var item = await _dbContext.ActiveItems.FirstOrDefaultAsync(t => t.Id == request.Id);

        if (item is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Item with id {request.Id} is not found"));

        item.Title = request.Title;
        item.TimeSpent = request.Timespent.ToTimeSpan();

        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new ItemReply
        {
            Id = item.Id,
            Title = item.Title,
            Timespent = Duration.FromTimeSpan(item.TimeSpent)
        });
    }

    public override async Task<ItemReply> GetSingle(GetSingleRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Index must be greater then 0"));

        var item = await _dbContext.ActiveItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (item is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Item with id {request.Id} is not found"));

        return await Task.FromResult(new ItemReply
        {
            Id = item.Id,
            Title = item.Title,
            Timespent = Duration.FromTimeSpan(item.TimeSpent)
        });
    }


    public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
    {
        var response = new GetAllResponse();
        var items = await _dbContext.ActiveItems.ToListAsync();

        foreach (var t in items)
            response.Items.Add(new ItemReply
            {
                Id= t.Id,
                Title = t.Title,
                Timespent = Duration.FromTimeSpan(t.TimeSpent),
            });

        return await Task.FromResult(response);
    }

    public override async Task<ItemReply> Delete(GetSingleRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Index must be greater then 0"));

        var item = await _dbContext.ActiveItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (item is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Item with id {request.Id} is not found"));

        _dbContext.Remove(item);
        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new ItemReply
        {
            Id = item.Id,
            Title = item.Title,
            Timespent = Duration.FromTimeSpan(item.TimeSpent)
        });
    }

}