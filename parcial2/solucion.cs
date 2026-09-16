//Apaza Carballo Reychman Cristopher
//Situacion 1: aviso de vencimiento de membresia

using System;
namespace GimnasioFuerzaAndina
{
    public interface ISuscriptorVencimiento
    {
        void Notificar(Socio socio);
    }
}
public class Socio
{
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public Socio(string nombre, string telefono, DateTime fechaVencimiento)
    {
        Nombre = nombre;
        Telefono = telefono;
        FechaVencimiento = fechaVencimiento;
    }
}