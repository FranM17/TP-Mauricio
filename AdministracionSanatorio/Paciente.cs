using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdministracionSanatorio
{
    public class Paciente
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string ObraSocial { get; set; }
        public int Cobertura { get; set; }
        public List<RegistroIntervencion> IntervencionesRealizadas { get; set; }

        private static int contadorIntervenciones = 1;

        public Paciente(string dni, string nombre, string telefono, string obraSocial, int cobertura)
        {
            Dni = dni;
            Nombre = nombre;
            Telefono = telefono;
            ObraSocial = obraSocial;
            Cobertura = cobertura;
            IntervencionesRealizadas = new List<RegistroIntervencion>();
        }

        public void AgregarIntervencion(Intervencion intervencion, Doctor medico, DateTime fecha, bool pagado)
        {
            var registro = new RegistroIntervencion
            {
                Id = contadorIntervenciones++,
                Fecha = fecha,
                Intervencion = intervencion,
                Medico = medico,
                Pagado = pagado
            };
            IntervencionesRealizadas.Add(registro);
        }

        public decimal CalcularCostoTotal()
        {
            decimal total = 0;
            foreach (var r in IntervencionesRealizadas)
                total += r.Intervencion.CalcularCosto(Cobertura);
            return total;
        }

        public List<RegistroIntervencion> ObtenerIntervencionesPendientes()
        {
            return IntervencionesRealizadas.FindAll(r => !r.Pagado);
        }

        public override string ToString()
        {
            var obra = string.IsNullOrEmpty(ObraSocial) ? "-" : ObraSocial;
            return $"DNI: {Dni}, Nombre: {Nombre}, Tel: {Telefono}, ObraSocial: {obra}, Cobertura: {Cobertura}%";
        }

        public class RegistroIntervencion
        {
            public int Id { get; set; }
            public DateTime Fecha { get; set; }
            public Intervencion Intervencion { get; set; }
            public Doctor Medico { get; set; }
            public bool Pagado { get; set; }
        }
    }
}
