using System;
using System.Linq;
using System.Runtime.InteropServices;
using DefaultNamespace;
using SecureKey.Services;

namespace SecureKey.Demo;

public class Demo
{
    public static void Executar()
    {
        CofreSenhas cofreSenhas = null;
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("==============================");
        Console.WriteLine("    REGISTRADOR DE SENHAS");
        Console.WriteLine("==============================");
        Console.ResetColor();

        try
        {
            Console.WriteLine("Digite sua senha mestra: ");
            string senhaCriada = Console.ReadLine();
            
            cofreSenhas = new CofreSenhas(senhaCriada);
            
            Console.Write("Processando");
            for (int i = 0; i < 3; i++)
            {
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
            }
            Console.WriteLine("\n SENHA CRIADA COM SUCESSO ");
            System.Threading.Thread.Sleep(2000); 
        }
        catch (ArgumentException)
        {
            Console.WriteLine("\n SENHA INVALIDA!");
            for (int i = 0; i < 3; i++)
            {
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
            }
            Console.WriteLine("\nEncerrando a aplicação");
            Environment.Exit(0);
        }
        
        while (true)
        {
            Console.WriteLine("\nDigite a senha para acessar: ");
            string tentativa = Console.ReadLine();
            
            Console.Write("Processando login");
            for (int i = 0; i < 3; i++)
            {
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
            }
            Console.WriteLine();
            
            var Resultado = cofreSenhas.Desbloquear(tentativa);
            var x = Resultado.sucesso ? "■" : "□";
            
            Console.WriteLine($"Resultado: {x} {Resultado.mensagem}");
            System.Threading.Thread.Sleep(2000);
            
            if (Resultado.sucesso)
            {
                break;
            }
            else if (Resultado.mensagem.Contains("Bloqueado"))
            {
                Console.WriteLine("\nSistema bloqueado definitivamente!");
                Environment.Exit(0);
            }
        }

        int opcao;
        do
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==============================");
            Console.WriteLine("          SISTEMA OK");
            Console.WriteLine("==============================");
            Console.ResetColor();
            
            Console.WriteLine("1 - Adicionar credencial");
            Console.WriteLine("2 - Ver senha");
            Console.WriteLine("3 - Total cadastradas");
            Console.WriteLine("4 - Listar todas");
            Console.WriteLine("5 - Buscar por site");
            Console.WriteLine("6 - Mais fraca");
            Console.WriteLine("7 - Gerar senha (sem salvar)");
            Console.WriteLine("0 - Sair");
            Console.Write("\nEscolha uma opção: ");
            
            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                opcao = -1; 
            }

            switch (opcao)
            {
                case 1:
                    Console.WriteLine("\nSite: ");
                    string s = Console.ReadLine();
                    Console.WriteLine("\nUsuario: ");
                    string u = Console.ReadLine();
                    
                    Console.WriteLine("\nDeseja criar sua senha? (s/n)");
                    for (int i = 0; i < 3; i++)
                    {
                        System.Threading.Thread.Sleep(500);
                        Console.Write(".");
                    }
                    Console.WriteLine();
        
                    string resposta = Console.ReadLine().Trim().ToLower();
                    string senhaFinal = "";
        
                    if (resposta == "s")
                    {
                        Console.WriteLine("\nSenha: ");
                        senhaFinal = Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Digite o tamanho da senha desejada (Entre 8 a 16):");
                        if (!int.TryParse(Console.ReadLine(), out int tamanho))
                        {
                            tamanho = 8;
                        }
                        else if (tamanho < 8 || tamanho > 16)
                        { 
                            Console.WriteLine("TAMANHO INVALIDO!, tamanho foi ajustado para 8 caracteres");
                            tamanho = 8;
                        }

                        senhaFinal = GeradorSenhas.Gerar(tamanho);
                        Console.WriteLine($"Senha gerada: {senhaFinal}");
                        System.Threading.Thread.Sleep(2000);
                    }

                    try
                    {
                        Credencial novaCredencial = new Credencial(s, u, senhaFinal);
                        
                        Console.Write("\nDeseja adicionar senha gerada também? (s/n): ");
                        string respostaExtra = Console.ReadLine().Trim().ToLower();

                        if (respostaExtra == "s")
                        {
                            Console.Write("Digite o tamanho para a senha gerada adicional (entre 8 e 16): ");
                            if (!int.TryParse(Console.ReadLine(), out int tamanhoExtra) || tamanhoExtra < 8 || tamanhoExtra > 16)
                            {
                                tamanhoExtra = 8;
                            }

                            string senhaExtraGerada = GeradorSenhas.Gerar(tamanhoExtra);
                            Console.WriteLine($"Segunda senha gerada: {senhaExtraGerada}");
                            
                            novaCredencial.DefinirSenhaGerada(senhaExtraGerada);
                        }
                        
                        var adicionouComSucesso = cofreSenhas.Adicionar(novaCredencial);
                        var r = adicionouComSucesso.sucesso ? "Credencial adicionada: ■" : "\nNão foi possível adicionar: □";
                        Console.WriteLine($"Resultado: {r} {adicionouComSucesso.mensagem}");
                        
                        
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"\nDados inválidos: {ex.Message}");
                    }

                    System.Threading.Thread.Sleep(2000);
                    break;

                case 2:
                    Console.WriteLine("Site: ");
                    string _site = Console.ReadLine();
                    var credencial = cofreSenhas.BuscarCredencialInterna(_site);
                    if (credencial == null)
                    {
                        Console.WriteLine("Site não encontrado.");
                    }
                    else
                    {
                        Console.WriteLine(credencial.VerSenha());
                    }
                    System.Threading.Thread.Sleep(3000);
                    break;
                
                case 3:
                    Console.WriteLine($"\nTotal de credenciais cadastradas: {cofreSenhas.TotalCadastradas()}");
                    System.Threading.Thread.Sleep(3000);
                    break;

                case 4:
                    var lista = cofreSenhas.Listar();
                    if (!lista.Any())
                    {
                        Console.WriteLine("\nNenhuma credencial cadastrada.");
                    }
                    else
                    {
                        Console.WriteLine("\n=== LISTA DE CREDENCIAIS ===");
                        foreach (var x in lista)
                        {
                            Console.WriteLine(x.ToString()); 
                        }
                    }
                    System.Threading.Thread.Sleep(4000);
                    break;
                
                case 5:
                    Console.Write("\nDigite o nome do site para buscar: ");
                    string siteBusca = Console.ReadLine();

                    var credencialEncontrada = cofreSenhas.BuscarPorSite(siteBusca);

                    if (credencialEncontrada == null)
                    {
                        Console.WriteLine("\n Nenhuma credencial encontrada para este site.");
                    }
                    else
                    {
                        Console.WriteLine("\n=== CREDENCIAL ENCONTRADA ===");
                        Console.WriteLine(credencialEncontrada.ToString());
                    }

                    System.Threading.Thread.Sleep(3000);
                    break;

                case 6:
                    var credencialMaisFraca = cofreSenhas.MaisFraca();

                    if (credencialMaisFraca == null)
                    {
                        Console.WriteLine("\nNenhuma credencial cadastrada no cofre ainda.");
                    }
                    else
                    {
                        Console.WriteLine("\n=== CREDENCIAL MAIS FRACA ===");
                        Console.WriteLine(credencialMaisFraca.ToString());
                    }

                    System.Threading.Thread.Sleep(3000);
                    break;

                case 7:
                    Console.Write("\nDigite o tamanho da senha a ser testada: ");
                    
                    if (!int.TryParse(Console.ReadLine(), out int tamanhoTeste))
                    {
                        tamanhoTeste = 8;
                    }

                    string senhaTestada = GeradorSenhas.Gerar(tamanhoTeste);

                    Console.WriteLine($"\nSenha Gerada: {senhaTestada}");
                    Console.WriteLine($"Força da Senha: {GeradorSenhas.AvaliarForca(senhaTestada)}");
                    Console.WriteLine($"É Segura? {(GeradorSenhas.EhSegura(senhaTestada) ? "Sim ■" : "Não □")}");

                    System.Threading.Thread.Sleep(4000);
                    break;
                    
                case 0:
                    Console.WriteLine("Saindo do sistema...");
                    break;
                    
                default:
                    Console.WriteLine("Opção inválida! Tente novamente.");
                    System.Threading.Thread.Sleep(1500);
                    break;
            }
        } while (opcao != 0);
    }
}