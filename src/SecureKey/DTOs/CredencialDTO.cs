namespace DefaultNamespace;
using System.Text;
using System;

// Opa, essa classe aqui é o "Telão do Estádio". Ela existe só pra separar o que é dado seguro (Credencial) 
// do que vai pro tela pra todo mundo vai assistir (DTO). Na API, isso é o que barra o cliente de ver a tática secreta do 
// nosso banco de dados, entregando só o essencial pro telão do app.
public class CredencialDTO
{
    public int Id { get; private set; }
    public string Site { get; private set; }
    public string Usuario { get; private set; }
    public int Forca { get; private set; }
    public ForcaSenha Classificacao { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public bool Expirada { get; private set; }

    public CredencialDTO(Credencial credencial)
    {
        this.Id = credencial.Id;
        this.Site = credencial.Site;
        this.Usuario = credencial.Usuario;
        this.Forca = credencial.Forca;
        this.Classificacao = SenhaNivel.Classificar(credencial.Forca);
        this.Expirada = credencial.EstaExpirada();
        this.CriadoEm = CriadoEm;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        
        sb.AppendLine($"ID: {Id}");
        sb.AppendLine($"Site: {Site}");
        sb.AppendLine($"Usuário: {Usuario}");
        sb.AppendLine($"Força da Senha: {Forca}");
        sb.AppendLine($"Classificação: {Classificacao}");
        sb.AppendLine($"Criado Em: {CriadoEm:dd/MM/yyyy HH:mm:ss}");
        sb.AppendLine($"Expirada: {(Expirada ? "Sim" : "Não")}");
        
        if (Expirada)
        {
            sb.AppendLine("SENHA EXPIRADA");
        }
        return sb.ToString();
    }
}