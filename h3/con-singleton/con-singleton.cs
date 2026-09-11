public sealed class ConfiguracionDeMora
{
    private static readonly ConfiguracionDeMora _instancia = new();
    public static ConfiguracionDeMora Instancia => _instancia;

    public decimal TasaDeInteresMoratorio { get; private set; } = 0.03m; // 3% mensual
    public int DiasDeGracia { get; private set; } = 5;

    private ConfiguracionDeMora() { }

    public void ActualizarReglas(decimal nuevaTasa, int nuevosDiasDeGracia)
    {
        TasaDeInteresMoratorio = nuevaTasa;
        DiasDeGracia = nuevosDiasDeGracia;
    }
}
public static class DemoConfiguracionDeMora
{
    public static void Correr()
    {
        var configEnCalculoDeMora = ConfiguracionDeMora.Instancia;
        var configEnPanelDeSupervisor = ConfiguracionDeMora.Instancia;
        Console.WriteLine($"[CONFIG] Tasa actual: {configEnCalculoDeMora.TasaDeInteresMoratorio:P0}, dias de gracia: {configEnCalculoDeMora.DiasDeGracia}");
        Console.WriteLine($"¿Son la MISMA instancia? {ReferenceEquals(configEnCalculoDeMora, configEnPanelDeSupervisor)}");
        Console.WriteLine("---");
        configEnPanelDeSupervisor.ActualizarReglas(0.05m, 3);
        Console.WriteLine($"[CONFIG] Tasa vista desde CalculoDeMora tras el cambio: {configEnCalculoDeMora.TasaDeInteresMoratorio:P0}, dias de gracia: {configEnCalculoDeMora.DiasDeGracia}");
        Console.WriteLine("Si fueran dos instancias distintas, este ultimo valor NO habria cambiado, y el calculo de mora podria estar mal.");
    }
}