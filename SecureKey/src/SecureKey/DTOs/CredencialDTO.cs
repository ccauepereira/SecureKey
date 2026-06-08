namespace DefaultNamespace;

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

    }
}