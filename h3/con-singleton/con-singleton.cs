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