using System.ComponentModel.DataAnnotations;

namespace PRN222.Model
{
    public class HealthCheckUpdateModel
    {
        public int HealthCheckID { get; set; }

        [Required]
        public string Result { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public double LeftEye { get; set; }

        [Required]
        public double RightEye { get; set; }
    }
}
