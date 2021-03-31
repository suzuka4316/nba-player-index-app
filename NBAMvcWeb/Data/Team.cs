using System.ComponentModel.DataAnnotations;

namespace NBAMvcWeb.Data
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}