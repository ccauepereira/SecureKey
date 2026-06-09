namespace DefaultNamespace;

public enum ForcaSenha
{
    Fraca,
    Media,
    Forte,
    MuitoForte,
    Militar
}

public static class SenhaNivel
{
    public static ForcaSenha Classificar(int pontuacao)
    {
        if (pontuacao < 50)
        {
            return ForcaSenha.Fraca;
        }

        if (pontuacao < 100)
        {
            return ForcaSenha.Media;
        }

        if (pontuacao < 150)
        {
            return ForcaSenha.Forte;
        }

        if (pontuacao < 200)
        {
            return ForcaSenha.MuitoForte;
        }

        return ForcaSenha.Militar;
    }
}

