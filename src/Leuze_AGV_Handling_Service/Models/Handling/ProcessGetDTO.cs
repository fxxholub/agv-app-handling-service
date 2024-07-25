using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Handling
{
    public class ProcessGetDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Pid { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public string Host { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
    }
}
