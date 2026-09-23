cuando un pedido queda preparado, el estudiante debe recibir un aviso
esto es cuando algo cambia, avisale a los interesados sin que el que cambia tenga que saber a quien ni como -> patron OBSERVER

Pedido es el sujeto, porque guarda una lista de observaciones y, cuando CambiarEstado lo pasa a PREPARADO, los recorre y les avisa. los observadores concretos son quienes saben como avisar, ya sea correo notificacion, o lo que se  quiera agregar en un futuro

public interface IObservadorPedido
{
    void Actualizar(Pedido pedido);
}
public class Pedido
{
    public int Id { get; set; }
    public Estudiante Estudiante { get; set; }
    public EstadoPedido Estado { get; private set; }
    private readonly List<IObservadorPedido> _observadores = new();
    public void Suscribir(IObservadorPedido observador) => _observadores.Add(observador);
    public void CambiarEstado(EstadoPedido nuevoEstado)
    {
        Estado = nuevoEstado;
        if (nuevoEstado == EstadoPedido.Preparado)
        {
            foreach (var observador in _observadores)
                observador.Actualizar(this);
        }
    }
}
public class AvisoCorreoEstudiante : IObservadorPedido
{
    public void Actualizar(Pedido pedido)
        => Console.WriteLine($"[CORREO] Tu pedido #{pedido.Id} está listo, {pedido.Estudiante.Nombre}");
}
public class AvisoAppEstudiante : IObservadorPedido
{
    public void Actualizar(Pedido pedido)
        => Console.WriteLine($"[APP] Notificación push: pedido #{pedido.Id} preparado");
}