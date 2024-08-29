public static class GameData
{
    public static int Vida { get; set; } = 3;
    public static int Puntos { get; set; } = 0;
    public static int TotalPuntos { get; set; } = 5;

    public static void ReiniciarDatos()
    {
        Vida = 3; // Vida inicial, puedes ajustar según sea necesario
        Puntos = 0;
        TotalPuntos = 5;
    }
}
