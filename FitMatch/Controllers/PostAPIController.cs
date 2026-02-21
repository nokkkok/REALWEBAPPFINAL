using Microsoft.AspNetCore.Mvc;
using FitMatch.Data;
using FitMatch.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FitMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostAPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostAPIController(AppDbContext context)
        {
            _context = context;
        }

        private NpgsqlConnection GetConnection()
        {
            var connectionString = _context.Database.GetConnectionString();
            return new NpgsqlConnection(connectionString);
        }

        [HttpGet]
        public async Task<ActionResult> GetPosts()
        {
            var posts = new List<Post>();
            await using var conn = GetConnection();
            await conn.OpenAsync();
            var sql = @"SELECT * FROM ""Post""";
            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                posts.Add(new Post
                {
                    PostId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Location = reader.GetString(3),
                    Date = reader.GetFieldValue<DateOnly>(4),
                    Time = reader.GetFieldValue<TimeOnly>(5),
                    Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                    SportType = reader.IsDBNull(7) ? null : reader.GetString(7),
                    CreateDate = reader.GetFieldValue<DateTime>(8),
                    MaxPeople = reader.GetInt32(9)


                });
            }
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPosts(int id)
        {
            var posts = new List<Post>();
            await using var conn = GetConnection();
            await conn.OpenAsync();
            var sql = @"SELECT * FROM ""Post"" WHERE ""UserId"" = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                posts.Add(new Post
                {
                    PostId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Location = reader.GetString(3),
                    Date = reader.GetFieldValue<DateOnly>(4),
                    Time = reader.GetFieldValue<TimeOnly>(5),
                    Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                    SportType = reader.IsDBNull(7) ? null : reader.GetString(7),
                    CreateDate = reader.GetFieldValue<DateTime>(8),
                    MaxPeople = reader.GetInt32(9)


                });
            }
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost ([FromBody] CreatePost createPost)
        {
            await using var conn = GetConnection();
            await conn.OpenAsync();
            var sql = @"INSERT INTO ""Post"" 
                (""UserId"", ""Title"", ""Location"",""Date"",""Time"",""Description"",""SportType"",""MaxPeople"") 
                VALUES (@userId, @title, @location,@date,@time,@description,@sportType,@maxPeople)
                RETURNING ""PostId"";";

                await using var cmd = new NpgsqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("userId",createPost.UserId);
                cmd.Parameters.AddWithValue("title",createPost.Title ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("location",createPost.Location ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("date",createPost.Date);
                cmd.Parameters.AddWithValue("time",createPost.Time);
                cmd.Parameters.AddWithValue("description",createPost.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("sportType",createPost.SportType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("maxPeople",createPost.MaxPeople ?? (object)DBNull.Value);

                var postId = (int?)await cmd.ExecuteScalarAsync() ?? 0;
                var port = new List<Post>();
                return Ok(new
                {
                    portId = postId,
                    createPost.Title,
                });
                
        }
    }
}

