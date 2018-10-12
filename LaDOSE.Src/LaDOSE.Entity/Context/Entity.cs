using System.ComponentModel.DataAnnotations;

namespace LaDOSE.Entity.Context
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}