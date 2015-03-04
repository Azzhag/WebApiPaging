using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiPaging.Models
{
    public class Tweet
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        public string Text { get; set; }
    }
}