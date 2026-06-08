using System;
using System.Text;
using System.Linq;

namespace DefaultNamespace;

public class Credencial
{
    public DateTime CriadoEm { get; private set; }
    public int Forca { get; private set; }
    public string Usuario { get; private set; }
    public string Site { get; private set; }
    public int Id { get; private set; }
    public DateTime? UltimoAcesso { get; private set; } = null;
    private static int _idAux = 1;
    private string _senhaManual;
    private string? _senhaGerada;

    public Credencial(string site, string usuario, string senhaManual)
    {
        if (string.IsNullOrWhiteSpace(site))
        {
            throw new ArgumentException("Site não pode ser vazio");
        }
        if (string.IsNullOrWhiteSpace(usuario))
        {
            throw new ArgumentException("Usuario não pode ser vazia");
        }
        if (string.IsNullOrWhiteSpace(senhaManual))
        {
            throw new ArgumentException("Senha não pode ser vazia");
        }
        this.Id = _idAux++;
        this.Site = site;
        this.Usuario = usuario;
        this.CriadoEm = DateTime.Now;
        this._senhaManual = senhaManual;
        this.Forca = CalcularForca(_senhaManual);
    }

    private int CalcularForca(string senha)
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

    public void DefinirSenhaGerada(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("Senha não pode ser vazia");
        _senhaGerada = senha;
        int forcaManual = CalcularForca(_senhaManual);
        int forcaGerada = CalcularForca(_senhaGerada);
        Forca = Math.Max(forcaManual, forcaGerada);
    }
    
    public bool Expirada()
    {
        TimeSpan tempoPassado = DateTime.Now - CriadoEm;
        return tempoPassado > TimeSpan.FromDays(90);
    }

    public string VerSenha()
    {
        UltimoAcesso = DateTime.Now;
        var sb = new StringBuilder();
        sb.AppendLine($"Senha feita: {_senhaManual}");
        if (!string.IsNullOrWhiteSpace(_senhaGerada))
        {
            sb.AppendLine($"Senha Gerada: {_senhaGerada}");
        }

        sb.AppendLine($"Usuario: {Usuario}");
        sb.AppendLine($"Site: {Site}");
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Forca: {Forca}");
        sb.AppendLine($"Criado em: {CriadoEm.ToString("dd/MM/yyyy")}");
        return sb.ToString();
        // Calma, eu sei que mostrar senha é meio suspeito kkk :P
        // Mas esse projeto é um cofre de senhas, então faz sentido conseguir visualizar elas.
        // Em sistemas de login reais isso não acontece.
        // A senha fica protegida por mechanisms de segurança e não pode ser recuperada.
        // Aqui estou fazendo assim apenas para aprender os conceitos e praticar C# :)
    }
}