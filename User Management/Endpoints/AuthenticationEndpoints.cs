using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using User_Management.Models;
using User_Management.DataModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using User_Management.Services;

namespace User_Management.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Auth").WithTags(nameof(Auth));

        group.MapGet("/{id}", async Task<Results<Ok<Auth>, NotFound>> (Guid userid, middlewaredbContext db) =>
        {
            return await db.Auth.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserId == userid.ToByteArray())
                is Auth model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("CheckAccountExists")
        .WithOpenApi();

        group.MapPost("/", async Task<Results<Ok<Guid>, UnauthorizedHttpResult>> (UserData data, middlewaredbContext db) =>
        {
            // get user by username
            var user = await db.User.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserName == data.UserName);

            // user can be null here. If not, verify password
            if (user == null || !PasswordHasher.Verify(data.Password, user.Auth?.Password, user.Auth?.Salt))
            {
                return TypedResults.Unauthorized();
            }

            return TypedResults.Ok(new Guid(user.UserId));
        })
        .WithName("AuthenticateUser")
        .WithOpenApi();
    }
}
