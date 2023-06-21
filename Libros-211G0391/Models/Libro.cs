using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libros_211G0391.Models
{
    public class Libro
    {
        public string Titulo { get; set; } = "";
        public string TituloOriginal { get; set; } = "";
        public string Autor { get; set; } = "";
        public string? Reseña { get; set; }
        public string? URLPortada { get; set; }
        public DateTime FehcaDePublicacion { get; set; } = DateTime.Now.Date;
    }
}
