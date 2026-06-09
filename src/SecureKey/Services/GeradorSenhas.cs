using System;
using System.Linq;

namespace SecureKey.Services;
using System.Security.Cryptography;
using System.Text;

public class GeradorSenhas
{
    private const string LETRAS = "abcdefghijklmnopqrstuvwxyz";
    private const string MAIUSCULAS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NUM = "0123456789";
    private const string ESP = "!@#$%&*";

    public static string Gerar(int tamanho, bool usarESP = true)
    {
        if (tamanho < 4)
        {
            throw new ArgumentException("Senha deve ter no minimo 4 caracteres");
        }

        string todos = LETRAS + MAIUSCULAS + NUM + ESP;
        
        if (!usarESP)
        {
            var caracteresFiltrados = todos.Where(c => !ESP.Contains(c));
            
            todos = string.Concat(caracteresFiltrados);
        }

        var sb = new StringBuilder(tamanho);
        byte[] bytes = new byte[tamanho];
        
        RandomNumberGenerator.Fill(bytes);

        for (int i = 0; i < tamanho; i++)
        {
            int index = bytes[i] % todos.Length;
            sb.Append(todos[index]);
        }
        return sb.ToString();
    }
    public static int AvaliarForca(string senha)
    {
        int forca = 0;
        
        if (senha.Length >= 8)
            forca += 25;

        if (senha.Length >= 12)
            forca += 25;

        if (senha.Length >= 16)
            forca += 25;
        
        if (senha.Any(c => char.IsLower(c)))
            forca += 25;

        if (senha.Any(c => char.IsUpper(c)))
            forca += 25;

        if (senha.Any(c => char.IsDigit(c)))
            forca += 25;

        string especiais = "@!%&*$#";

        if (senha.Any(c => especiais.Contains(c)))
            forca += 25;

        return forca;
    }

    public static bool EhSegura(string senha)
    {
        return AvaliarForca(senha) >= 150;
    }
    
}