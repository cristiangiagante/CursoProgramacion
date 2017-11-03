using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CursoDeProgramacion.Models
{
    public class ClaseDiaria
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string VideoLink { get; set; }
    }
    public class ClasesDiarias
    {
        public List<ClaseDiaria> ListaClases { get; set; }
    }
}