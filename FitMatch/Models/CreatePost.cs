using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMatch.Models
{
    public class CreatePost
    {


        [Required]
        public int UserId { get; set; }

        public string? Title { get; set; }
        public string? Location { get; set; }

        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }

        public string? Description { get; set; }
        public string? SportType { get; set; }
        public int? MaxPeople { get; set; }




    }
}
