﻿using System.ComponentModel.DataAnnotations;

namespace FundParser.DAL.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; init; }

        public DateTime CreatedAt { get; } = DateTime.Now;

        public DateTime? DeletedAt { get; set; } = null;
    }
}
