using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;


namespace AdministracionSanatorio
{
    class Program
    {
        static void Main(string[] args)
        {

            decimal pct;
            Console.Write("ingresa el porcentaje de complejidad: ");
            while (!decimal.TryParse(Console.ReadLine(), out pct) || pct < 0)
            {
                Console.Write("Entrada invalida. Ingrese un número válido: ");
            }
            IntervencionAltaComplejidad.PorcentajeAdicional = pct;

            var hospital = new Hospital();
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n ** MENU SANATORIO DE LOS REYES DE GIT **");

                Console.WriteLine("1- Alta de paciente");
                Console.WriteLine("2- Listar pacientes");
                Console.WriteLine("3- Asignar intervención");
                Console.WriteLine("4- Calcular costo por DNI");
                Console.WriteLine("5- Reporte pendientes de pago");
                Console.WriteLine("6- Terminar programa");
                Console.Write("Opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AltaPaciente(hospital);
                        break;
                    case "2":
                        ListarPacientes(hospital);
                        break;
                    case "3":
                        AsignarIntervencion(hospital);
                        break;
                    case "4":
                        CalcularCosto(hospital);
                        break;
                    case "5":
                        ReportePendientes(hospital);
                        break;
                    case "6":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opcion inválida.");
                        break;
                }
            }
        }

        static void AltaPaciente(Hospital h)
        {
            Console.Write("DNI: ");
            var dni = Console.ReadLine();
            if (h.Pacientes.Any(p => p.Dni == dni))
            {
                Console.WriteLine("Ya existe ese DNI.");
                return;
            }
            Console.Write("Nombre y Apellido: ");
            var nom = Console.ReadLine();
            Console.Write("Telefono: ");
            var tel = Console.ReadLine();
            Console.Write("Obra Social (enter para ninguna): ");
            var obra = Console.ReadLine();
            int cobertura = 0;
            if (!string.IsNullOrWhiteSpace(obra))
            {
                Console.Write("Cobertura (%): ");
                while (!int.TryParse(Console.ReadLine(), out cobertura)
                       || cobertura < 0 || cobertura > 100)
                    Console.Write("Valor inválido. Ingrese 0–100: ");
            }
            h.Pacientes.Add(new Paciente(dni, nom, tel, obra, cobertura));
            Console.WriteLine("Paciente agregado.");
        }

        static void ListarPacientes(Hospital h)
        {
            Console.WriteLine("-- Pacientes --");
            foreach (var p in h.Pacientes)
                Console.WriteLine(p);
        }

        static void AsignarIntervencion(Hospital h)
        {
            Console.Write("DNI del paciente: ");
            var dni = Console.ReadLine();
            var pac = h.Pacientes.FirstOrDefault(p => p.Dni == dni);
            if (pac == null)
            {
                Console.WriteLine("Paciente no existe. Lo damos de alta.");
                AltaPaciente(h);
                pac = h.Pacientes.First(p => p.Dni == dni);
            }

            Console.WriteLine("Intervenciones disponibles:");
            for (int i = 0; i < h.Intervenciones.Count; i++)
                Console.WriteLine($"{i + 1}) {h.Intervenciones[i]}");
            Console.Write("Elija (número): ");
            if (!int.TryParse(Console.ReadLine(), out var idx)
                || idx < 1 || idx > h.Intervenciones.Count)
            {
                Console.WriteLine("Selección inválida."); return;
            }
            var inv = h.Intervenciones[idx - 1];

            var medDisponibles = h.Doctores
                .Where(d => d.Especialidad == inv.Especialidad && d.Disponible)
                .ToList();
            if (!medDisponibles.Any())
            {
                Console.WriteLine("No hay médicos disponibles para esa especialidad.");
                return;
            }
            Console.WriteLine("Médicos disponibles:");
            for (int i = 0; i < medDisponibles.Count; i++)
                Console.WriteLine($"{i + 1}) {medDisponibles[i]}");
            Console.Write("Elija (número): ");
            if (!int.TryParse(Console.ReadLine(), out var mIdx)
                || mIdx < 1 || mIdx > medDisponibles.Count)
            {
                Console.WriteLine("Selección inválida."); return;
            }
            var doc = medDisponibles[mIdx - 1];

            Console.Write("Fecha (dd/MM/yyyy): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy",
                    null, DateTimeStyles.None, out var fecha))
            {
                Console.WriteLine("Formato de fecha inválido."); return;
            }

            Console.Write("¿Pagado? (s/n): ");
            var pagado = Console.ReadLine().Trim().ToLower() == "s";

            pac.AgregarIntervencion(inv, doc, fecha, pagado);
            doc.Disponible = false;
            Console.WriteLine("Intervención asignada.");
        }

        static void CalcularCosto(Hospital h)
        {
            Console.Write("DNI: ");
            var dni = Console.ReadLine();
            var pac = h.Pacientes.FirstOrDefault(p => p.Dni == dni);
            if (pac == null)
            {
                Console.WriteLine("Paciente no encontrado."); return;
            }
            Console.WriteLine($"Costo total: {pac.CalcularCostoTotal():C}");
        }

        static void ReportePendientes(Hospital h)
        {
            Console.WriteLine("-- Liquidaciones pendientes --");
            foreach (var p in h.Pacientes)
                foreach (var r in p.ObtenerIntervencionesPendientes())
                {
                    var obra = string.IsNullOrEmpty(p.ObraSocial) ? "-" : p.ObraSocial;
                    var costo = r.Intervencion.CalcularCosto(p.Cobertura);
                    Console.WriteLine(
                        $"ID:{r.Id} | {r.Fecha:dd/MM/yyyy} | {r.Intervencion.Descripcion} | " +
                        $"Paciente:{p.Nombre} | Médico:{r.Medico.Nombre}({r.Medico.Matricula}) | " +
                        $"ObraSocial:{obra} | Importe:{costo:C}");
                }
        }
    }
}
