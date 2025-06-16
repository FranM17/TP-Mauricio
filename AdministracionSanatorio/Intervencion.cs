using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdministracionSanatorio
{
    public abstract class Intervencion
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Especialidad { get; set; }
        public decimal Arancel { get; set; }

        protected Intervencion(string codigo, string descripcion, string especialidad, decimal arancel)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            Especialidad = especialidad;
            Arancel = arancel;
        }

        public abstract decimal CalcularCosto(int cobertura);

        public override string ToString()
        {
            return $"[{Codigo}] {Descripcion} – {Especialidad} – ${Arancel}";
        }
    }

    public class IntervencionComun : Intervencion
    {
        public IntervencionComun(string codigo, string descripcion, string especialidad, decimal arancel)
            : base(codigo, descripcion, especialidad, arancel) { }

        public override decimal CalcularCosto(int cobertura)
        {
            var descuento = Arancel * cobertura / 100m;
            return Arancel - descuento;
        }
    }

    public class IntervencionAltaComplejidad : Intervencion
    {
        public static decimal PorcentajeAdicional { get; set; }

        public IntervencionAltaComplejidad(string codigo, string descripcion, string especialidad, decimal arancel)
            : base(codigo, descripcion, especialidad, arancel) { }

        public override decimal CalcularCosto(int cobertura)
        {
            var conExtra = Arancel + (Arancel * PorcentajeAdicional / 100m);
            var descuento = conExtra * cobertura / 100m;
            return conExtra - descuento;
        }

        public override string ToString()
        {
            return base.ToString() + $" (+{PorcentajeAdicional}% extra)";
        }
    }
}
