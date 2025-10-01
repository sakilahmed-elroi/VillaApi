using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaApi.Model
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }       
        public string? Name { get; set; }
        public string? VillaType { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime LastModifiedDate { get; set;  } 
    }
}
