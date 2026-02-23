using System.Net;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
namespace MyWeb.Controllers;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;


[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _db;
    public PostsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetPosts()
    {
        var posts = _db.Posts
            .Include(p => p.Members)
            .Select(p => new
            {
                p.PostId,
                p.Title,
                p.Description,
                p.Time,
                p.CreateDate,
                MemberCount = p.Members.Count()
            })
            .OrderByDescending(p => p.CreateDate).ToList();

                             
        return Ok(posts);
    }
    
}
