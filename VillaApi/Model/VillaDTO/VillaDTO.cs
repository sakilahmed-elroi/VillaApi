using System.ComponentModel.DataAnnotations;

namespace VillaApi.Model.VillaDTO
{
    public class VillaDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
       public string? Name { get; set; }
        [Required]
        public string? VillaType { get; set; }
        [Required]
        public int Rate { get; set; }
    }
}
