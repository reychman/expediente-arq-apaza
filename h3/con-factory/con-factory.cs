public interface IAviso
{
    void Enviar(string mensaje, string destinatario);
}
public class AvisoLlamada : IAviso
{
    public void Enviar(string mensaje, string destinatario)
        => Console.WriteLine($"[LLAMADA] {mensaje} — llamando a {destinatario}");
}