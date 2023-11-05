using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using User_Management.Models;
using System;
using User_Management.Services;

namespace User_Management.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(User));

        group.MapGet("/", async Task<Results<Ok<User>, NotFound>> (Guid? userid, string? phone_no, middlewaredbContext db) =>
        {
            if (userid.HasValue)
            {
                return await db.User.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserId == userid.Value.ToByteArray())
                is User model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
            }
            else if (!string.IsNullOrEmpty(phone_no))
            {
                return await db.User.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PhoneNumber == phone_no)
                is User model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
            }

            return TypedResults.NotFound();
        })
        .WithName("GetUserByIdOrPhoneNo")
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

        group.MapPost("/", async Task<Results<Created<Guid>, BadRequest<string>>> (User user, middlewaredbContext db) =>
        {
            // Check if user already exists
            if (await db.User.AsNoTracking()
                           .AnyAsync(model => model.PhoneNumber == user.PhoneNumber))
            {
                return TypedResults.BadRequest("User already exists.");
            }
            
            // Check if user has authentication data
            if (user.Auth == null)
            {
                return TypedResults.BadRequest("User authentication data is required.");
            }

            // Create new guid
            user.UserId = Guid.NewGuid().ToByteArray();

            // secure the password
            var hashedPassword = PasswordHasher.Hash(user.Auth.Password);
            user.Auth.Password = hashedPassword.Item1;
            user.Auth.Salt = hashedPassword.Item2;

            db.User.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/User/{user.UserId}", new Guid(user.UserId));
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
