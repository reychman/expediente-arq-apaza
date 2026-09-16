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
using System.Collections.Generic;

public class GestorVencimientos
{
    private readonly List<ISuscriptorVencimiento> _suscriptores =
        new List<ISuscriptorVencimiento>();
    public void Suscribir(ISuscriptorVencimiento suscriptor)
    {
        _suscriptores.Add(suscriptor);
    }
    public void Desuscribir(ISuscriptorVencimiento suscriptor)
    {
        _suscriptores.Remove(suscriptor);
    }
    public void AvisarVencimiento(Socio socio)
    {
        foreach (var suscriptor in _suscriptores)
        {
            suscriptor.Notificar(socio);
        }
    }
}