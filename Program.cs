using AYUD_MINIMAL_API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("UserList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var users = app.MapGroup("api/user");

users.MapGet("/", GetUsers);
users.MapGet("/active", GetActiveUsers);
users.MapGet("/{rut}",GetUser);
users.MapPost("/", AddUser);
users.MapPut("/{id}", EditUser);
users.MapPut("/{id}/state", ChangeUserState);
users.MapDelete("/{id}", DeleteUser);


app.Run();


static IResult GetUsers(DataContext db)
{
    return TypedResults.Ok(db.Users.ToArray());
}

static IResult GetActiveUsers(DataContext db)
{
    return TypedResults.Ok(db.Users.Where(u => u.IsActive).ToList());
}

static IResult GetUser(string rut, DataContext db)
{
    var user = db.Users.Where(u => u.Rut == rut).FirstOrDefault();
    if (user == null)
    {
        return TypedResults.NotFound("El usuario no existe");
    }

    return TypedResults.Ok(user);
}

static IResult AddUser([FromBody] User user, DataContext db)
{
    var existingUser = db.Users.Where(u => u.Rut == user.Rut || u.Email == user.Email).FirstOrDefault();
    if (existingUser != null)
    {
        return TypedResults.BadRequest("El usuario ingresado ya existe!");
    }

    db.Users.Add(user);
    db.SaveChanges();
    return TypedResults.Created($"/user/",db.Users.ToArray());
}

static IResult EditUser(int id, [FromBody] User user, DataContext db)
{
    var existingUser = db.Users.Find(id);
    if (existingUser == null)
    {
        return TypedResults.NotFound("El usuario no existe");
    }

    existingUser.Name = user.Name;
    existingUser.Email = user.Email;
    
    db.Entry(existingUser).State = EntityState.Modified;
    db.SaveChanges();

    return TypedResults.Ok("Usuario editado correctamente");
}

static IResult ChangeUserState(int id, DataContext db)
{
    var existingUser = db.Users.Find(id);
    if (existingUser == null)
    {
        return TypedResults.NotFound("El usuario no existe");
    }

    existingUser.IsActive = !existingUser.IsActive;
    
    db.Entry(existingUser).State = EntityState.Modified;
    db.SaveChanges();

    return TypedResults.Ok("Usuario editado correctamente");
}

static IResult DeleteUser(int id, DataContext db)
{
    var existingUser = db.Users.Find(id);
    if (existingUser == null)
    {
        return TypedResults.NotFound("El usuario no existe");
    }

    db.Users.Remove(existingUser);
    db.SaveChanges();
    return TypedResults.Ok(db.Users.ToArray());
}