using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using User_Management.Models;

namespace User_Management.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(User));

        group.MapGet("/", async (middlewaredbContext db) =>
        {
            return await db.User.ToListAsync();
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<User>, NotFound>> (Guid userid, middlewaredbContext db) =>
        {
            return await db.User.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserId == userid.ToByteArray())
                is User model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid userid, User user, middlewaredbContext db) =>
        {
            var affected = await db.User
                .Where(model => model.UserId == userid.ToByteArray())
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.UserId, userid.ToByteArray())
                  .SetProperty(m => m.Name, user.Name)
                  .SetProperty(m => m.UserName, user.UserName)
                  .SetProperty(m => m.PhoneNumber, user.PhoneNumber)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapPost("/", async (User user, middlewaredbContext db) =>
        {
            db.User.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/User/{user.UserId}", user);
        })
        .WithName("CreateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid userid, middlewaredbContext db) =>
        {
            var affected = await db.User
                .Where(model => model.UserId == userid.ToByteArray())
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}
