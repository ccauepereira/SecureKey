```markdown
# 🔐 SecureKey — Gerenciador e Cofre de Senhas

O **SecureKey** é um aplicativo de console desenvolvido em C# (.NET) projetado para gerenciar credenciais de acesso locais com segurança. O sistema conta com recursos avançados de avaliação criptográfica, geração randômica de senhas, validações de integridade, proteção contra força bruta e segregação de dados usando padrões de arquitetura modernos.

---

## 🚀 Funcionalidades Principais

* **Senha Mestra de Entrada:** Proteção inicial do cofre. Se a senha mestra for considerada inválida durante a criação do cofre, a aplicação é abortada por segurança.
* **Mecanismo Antiforça Bruta:** Sistema de bloqueio definitivo após 3 tentativas consecutivas de login incorretas.
* **Geração Criptográfica de Senhas:** Utilização da classe `RandomNumberGenerator` (criptograficamente segura) para criar senhas verdadeiramente aleatórias, evitando vulnerabilidades de geradores baseados em tempo de CPU (`Random`).
* **Análise de Entropia e Força:** Algoritmo que pontua as senhas baseando-se em comprimento, letras maiúsculas, minúsculas, números e caracteres especiais.
* **Dupla Validação de Senha:** Suporte para salvar uma senha definida manualmente e/ou atrelar uma senha forte gerada aleatoriamente à mesma credencial.
* **Verificação de Expiração:** Identifica automaticamente se a credencial ultrapassou a janela recomendada de 90 dias para alteração.

---

## 🏗️ Arquitetura e Padrões de Projeto

A estrutura do projeto foi planejada seguindo boas práticas de engenharia de software para garantir escalabilidade e segurança:

### 🛡️ Padrão DTO (Data Transfer Object)
O projeto implementa o padrão DTO através da classe `CredencialDTO`. 
* **Por que isso importa?** Ele separa os dados sensíveis de persistência interna (`Credencial`) daquilo que é exibido nas telas do sistema (`DTO`). Isso impede a exposição acidental de strings de senhas originais em listagens ou logs de auditoria gerais, transmitindo apenas metadados seguros (como ID, site, usuário, força e se está expirada).

### 🏷️ Classificação por Enums
Uso de Enums estruturados para remover "strings mágicas" e tipar fortemente a classificação de força de segurança:
* `Fraca` | `Media` | `Forte` | `MuitoForte` | `Militar` (Destinada a senhas com pontuação máxima de entropia).

---

## 💻 Demonstração das Telas do Sistema (Outputs)

Como o programa roda inteiramente no terminal, abaixo estão representadas as principais saídas visuais geradas durante a execução do fluxo:

### 1. Inicialização e Criação do Cofre Mestre
Quando o programa é aberto pela primeira vez, ele solicita a senha mestra. Se o usuário digitar um valor válido, a barra de carregamento fictícia entra em ação:

```text
==============================
    REGISTRADOR DE SENHAS
==============================
Digite sua senha mestra: 
********

Processando...
 SENHA CRIADA COM SUCESSO 

```

Se o usuário digitar uma senha inválida (vazia ou menor que 4 caracteres), o bloco `try/catch(ArgumentException)` intercepta a falha e encerra a aplicação:

```text
==============================
    REGISTRADOR DE SENHAS
==============================
Digite sua senha mestra: 
123

 SENHA INVALIDA!
...
Encerrando a aplicação

```

### 2. Tela de Login (Mecanismo de Desbloqueio)

O loop `while(true)` exige que o usuário digite a senha correta para liberar o acesso ao menu principal.

* **Tentativa Incorreta:**

```text
Digite a senha para acessar: 
senha_errada

Processando login...
Resultado: □ Senha incorreta. Você ainda tem 2 chances antes do acesso ser bloqueado.

```

* **Bloqueio Definitivo (Excesso de tentativas):**

```text
Digite a senha para acessar: 
outra_errada

Processando login...
Resultado: □ Senha incorreta!
Sistema bloqueado definitivamente!

```

### 3. Menu Principal (Sistema Liberado)

Após o login com sucesso, a tela é limpa (`Console.Clear()`) e o menu interativo é exibido:

```text
==============================
          SISTEMA OK
==============================
1 - Adicionar credencial
2 - Ver senha
3 - Total cadastradas
4 - Listar todas
5 - Buscar por site
6 - Mais fraca
7 - Gerar senha (sem salvar)
0 - Sair

Escolha uma opção: 

```

### 4. Exemplos de Execução dos Casos

* **Caso 1: Adicionando Credencial (Geração automática com criptografia segura)**

```text
Site: 
github.com

Usuario: 
ccauepereira

Deseja criar sua senha? (s/n)
n
...
Digite o tamanho da senha desejada (Entre 8 a 16):
12
Senha gerada: aB9!$kL2mNxP

Deseja adicionar senha gerada também? (s/n): n
Resultado: ■ O Site 'github.com' foi cadastrado com sucesso no time.

```

* **Caso 4: Listar Todas (Exibição Sanitizada via DTO)**
A listagem utiliza o método `ToString()` modificado da classe `CredencialDTO`, ocultando as strings de senha reais por segurança de exibição.

```text
=== LISTA DE CREDENCIAIS ===
ID: 1
Site: github.com
Usuário: ccauepereira
Força da Senha: 150
Classificação: Forte
Criado Em: 09/06/2026 07:45:12
Expirada: Não

ID: 2
Site: google.com
Usuário: caue.dev
Força da Senha: 50
Classificação: Media
Criado Em: 09/06/2026 07:46:01
Expirada: Não

```

* **Caso 6: Buscar a Credencial Mais Fraca (Auditoria do Cofre)**

```text
=== CREDENCIAL MAIS FRACA ===
ID: 2
Site: google.com
Usuário: caue.dev
Força da Senha: 50
Classificação: Media
Criado Em: 09/06/2026 07:46:01
Expirada: Não

```

---

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C# 10 / 12
* **Plataforma:** .NET 6.0 / 8.0 / 10.0 (Core)
* **Criptografia:** `System.Security.Cryptography`
* **Manipulação de Strings:** `System.Text.StringBuilder` (para otimização de memória em loops de concatenação).
* **Consultas de Dados:** LINQ (`.Any()`, `.Select()`, `.FirstOrDefault()`, `.OrderBy()`).

---

## 🎯 Como Executar o Projeto

1. Certifique-se de ter o **.NET SDK** instalado em sua máquina.
2. Clone o repositório ou navegue até a pasta raiz do projeto.
3. Abra o terminal e restaure as dependências do NuGet:
```bash
dotnet restore

```


4. Execute o projeto:
```bash
dotnet run --project src/SecureKey

```



---

> 📝 **Nota de Aprendizado:** Este projeto foi desenvolvido com objetivos acadêmicos para consolidar conceitos de encapsulamento, tratamento de exceções com blocos `try/catch`, manipulação de fluxos com coleções genéricas (`List<T>`) e segurança orientada a objetos.

```

```
