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

public class NotificadorWhatsApp : ISuscriptorVencimiento
{
    public void Notificar(Socio socio)
    {
        Console.WriteLine($"[WhatsApp] Hola {socio.Nombre}, tu membresia vencio el {socio.FechaVencimiento:dd/MM/yyyy}.");
    }
}

public class RegistroVencidos : ISuscriptorVencimiento
{
    public void Notificar(Socio socio)
    {
        Console.WriteLine($"[RegistroVencidos] Se agrego a {socio.Nombre} a la lista de vencidos");
    }
}

public class PantallaRecepcion : ISuscriptorVencimiento
{
    public void Notificar(Socio socio)
    {
        Console.WriteLine($"[Recepcion] Aviso en pantalla: {socio.Nombre} debe renovar");
    }
}

public class ModuloPromociones : ISuscriptorVencimiento
{
    public void Notificar(Socio socio)
    {
        Console.WriteLine($"[Promociones] Le ofrecemos a {socio.Nombre} un 15% de descuento por renovar hoy");
    }
}

public class Program
{
    public static void Main()
    {
        var gestor = new GestorVencimientos();

        gestor.Suscribir(new NotificadorWhatsApp());
        gestor.Suscribir(new RegistroVencidos());
        gestor.Suscribir(new PantallaRecepcion());
        gestor.Suscribir(new ModuloPromociones());

        var socio = new Socio("Carlos Mamani","70011122", DateTime.Today);
        gestor.AvisarVencimiento(socio);
    }
}