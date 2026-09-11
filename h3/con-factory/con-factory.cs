public interface IAviso
{
    void Enviar(string mensaje, string destinatario);
}

public class AvisoCuaderno : IAviso
{
    public void Enviar(string mensaje, string destinatario)
        => Console.WriteLine($"[CUADERNO] {mensaje} — pegado para {destinatario}");
}
public class AvisoWhatsApp : IAviso
{
    public void Enviar(string mensaje, string destinatario)
        => Console.WriteLine($"[WHATSAPP] 📱 {mensaje} — enviado a {destinatario}");
}
public class AvisoLlamada : IAviso
{
    public void Enviar(string mensaje, string destinatario)
        => Console.WriteLine($"[LLAMADA] {mensaje} — llamando a {destinatario}");
}
public static class FabricaDeAvisos
{
    public static IAviso Crear(string canal) => canal switch
    {
        "cuaderno" => new AvisoCuaderno(),
        "whatsapp" => new AvisoWhatsApp(),
        "llamada"  => new AvisoLlamada(),
        _ => throw new ArgumentException($"Canal desconocido: {canal}")
    };
}