using DefaultNamespace;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureKey.Services;

public class CofreSenhas
{
    private readonly List<Credencial> _credenciais = new();
    
    
    private readonly string _senhaMestra;
    
    private int _tentativas = 0;
    private const int maxTentativas = 3;
    
    public CofreSenhas(string? senhaMestra)
    {
        if (string.IsNullOrWhiteSpace(senhaMestra) || senhaMestra.Length < 4)
        {
            throw new ArgumentException("A senha mestra deve ter pelo menos 4 caracteres");
        }
        _senhaMestra = senhaMestra;
    }
    
    public (bool sucesso, string mensagem) Desbloquear(string senha)
    {
        if (_tentativas >= maxTentativas)
        {
            return (false, "Acesso Bloqueado!.");
        }

        if (_senhaMestra.Equals(senha))
        {
            _tentativas = 0;
            return (true, "Acesso liberado!.");
        }

        _tentativas++;
        int restas = maxTentativas - _tentativas;

        if (_tentativas >= maxTentativas)
        {
            return (false, "Senha incorreta!.");
        }

        return (false, $"Senha incorreta. Você ainda tem {restas} chances antes do acesso ser bloqueado.");
    }
    
    public (bool sucesso, string mensagem) Adicionar(Credencial credencial)
    {
        if (credencial == null)
        {
            return (false, "Não existe nenhuma credencial válida para cadastrar.");
        }
        
        bool existe = _credenciais.Any(c => c.Site.Equals(credencial.Site, StringComparison.OrdinalIgnoreCase));
        if (existe)
        {
            return (false, $"Ih, deu ruim! O Site '{credencial.Site}' já existe no elenco.");
        }

        _credenciais.Add(credencial);
        return (true, $"O Site '{credencial.Site}' foi cadastrado com sucesso no time.");
    }
    
    public int TotalCadastradas()
    {
        return _credenciais.Count;
    }
    
    public List<CredencialDTO> Listar()
    {
        return _credenciais.Select(c => new CredencialDTO(c)).ToList();
    }
    
    public CredencialDTO? BuscarPorSite(string site)
    {
        var credencial = _credenciais.FirstOrDefault(c => c.Site.Equals(site, StringComparison.OrdinalIgnoreCase));
        return credencial != null ? new CredencialDTO(credencial) : null;
    }
    
    public CredencialDTO? MaisFraca()
    {
        var piorChute = _credenciais.OrderBy(c => c.Forca).FirstOrDefault();
        return piorChute != null ? new CredencialDTO(piorChute) : null;
    }
    
    public Credencial? BuscarCredencialInterna(string site)
    {
        return _credenciais.FirstOrDefault(c => c.Site.Equals(site, StringComparison.OrdinalIgnoreCase));
    }
}
    