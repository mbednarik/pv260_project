using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
