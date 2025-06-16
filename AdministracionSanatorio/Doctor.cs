using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministracionSanatorio
{
    public class Doctor
    {
        public string Nombre { get; set; }
        public string Matricula { get; set; }
        public string Especialidad { get; set; }
        public bool Disponible { get; set; }

        public Doctor(string nombre, string matricula, string especialidad, bool disponible)
        {
            Nombre = nombre;
            Matricula = matricula;
            Especialidad = especialidad;
            Disponible = disponible;
        }

        public override string ToString()
        {
            var estado = Disponible ? "Disponible" : "No disponible";
            return $"Doctor: {Nombre} (Mat.{Matricula}), Esp.: {Especialidad}, {estado}";
        }
    }
}