using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public struct Produto
{
    public int Id;
    public string Nome;
    public decimal Preco;

    public Produto(int id, string nome, decimal preco)
    {
        Id = id;
        Nome = nome;
        Preco = preco;
    }
}

public struct Pedido
{
    public int IdPedido;
    public List<Produto> Produtos;
    public decimal Total;

    public Pedido(int idPedido)
    {
        IdPedido = idPedido;
        Produtos = new List<Produto>();
        Total = 0;
    }

    public void AdicionarProduto(Produto produto)
    {
        Produtos.Add(produto);
        Total += produto.Preco;
    }
}

class Program
{
    static List<Produto> produtos = new List<Produto>();
    static List<Pedido> pedidos = new List<Pedido>();

    static void Main(string[] args)
    {
        MostrarMensagemInicial();

        bool executar = true;

        while (executar)
        {
            try
            {
                MostrarMenu();
                string escolha = Console.ReadLine();

                switch (escolha)
                {
                    case "1":
                        CadastrarProduto();
                        break;
                    case "2":
                        EditarProduto();
                        break;
                    case "3":
                        CadastrarPedido();
                        break;
                    case "4":
                        ListarProdutos();
                        break;
                    case "5":
                        ListarPedidos();
                        break;
                    case "6":
                        ExibirTotalVendas();
                        break;
                    case "7":
                        executar = false;
                        Console.WriteLine("Obrigado por utilizar o sistema! Até mais.");
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Pressione Enter para continuar.");
                        Console.ReadLine();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                Console.WriteLine("Pressione Enter para continuar.");
                Console.ReadLine();
            }
        }
    }

    static void MostrarMensagemInicial()
    {
        Console.Clear();
        Console.WriteLine("=== Bem-vindo ao Sistema de Lanchonete ===");
        Thread.Sleep(2000); // Pausa para o usuário visualizar
        Console.Clear();
    }

    static void MostrarMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Sistema Lanchonete ===");
        Console.WriteLine("1. Cadastro de Produto");
        Console.WriteLine("2. Editar Produto");
        Console.WriteLine("3. Cadastro de Pedido");
        Console.WriteLine("4. Listar Produtos");
        Console.WriteLine("5. Listar Pedidos");
        Console.WriteLine("6. Total de Vendas");
        Console.WriteLine("7. Sair");
        Console.Write("Escolha uma opção: ");
    }

    static void CadastrarProduto()
    {
        Console.Clear();
        Console.WriteLine("=== Cadastro de Produto ===");

        int id = LerInteiro("ID do Produto (deve ser único): ");
        if (produtos.Any(p => p.Id == id))
        {
            Console.WriteLine("ID já existe. Operação cancelada.");
            Console.ReadLine();
            return;
        }

        Console.Write("Nome do Produto: ");
        string nome = Console.ReadLine();
        if (string.IsNullOrEmpty(nome))
        {
            Console.WriteLine("Nome não pode ser vazio. Operação cancelada.");
            Console.ReadLine();
            return;
        }

        decimal preco = LerDecimal("Preço do Produto: ");
        if (preco <= 0)
        {
            Console.WriteLine("Preço deve ser maior que zero. Operação cancelada.");
            Console.ReadLine();
            return;
        }

        produtos.Add(new Produto(id, nome, preco));
        Console.WriteLine("Produto cadastrado com sucesso! Pressione Enter para continuar.");
        Console.ReadLine();
    }

    static void EditarProduto()
    {
        Console.Clear();
        Console.WriteLine("=== Editar Produto ===");
        int id = LerInteiro("Digite o ID do produto que deseja editar: ");
        var produto = produtos.FirstOrDefault(p => p.Id == id);

        if (produto.Id == 0)
        {
            Console.WriteLine("Produto não encontrado! Pressione Enter para continuar.");
            Console.ReadLine();
            return;
        }

        Console.Write("Novo nome do Produto (ou deixe vazio para manter o atual): ");
        string novoNome = Console.ReadLine();
        if (!string.IsNullOrEmpty(novoNome))
        {
            produto.Nome = novoNome;
        }

        decimal novoPreco = LerDecimal("Novo preço do Produto (ou 0 para manter o atual): ");
        if (novoPreco > 0)
        {
            produto.Preco = novoPreco;
        }

        produtos.Remove(produtos.First(p => p.Id == id));
        produtos.Add(produto);

        Console.WriteLine("Produto atualizado com sucesso! Pressione Enter para continuar.");
        Console.ReadLine();
    }

    static void CadastrarPedido()
    {
        Console.Clear();
        Console.WriteLine("=== Cadastro de Pedido ===");
        int idPedido = LerInteiro("ID do Pedido (deve ser único): ");
        if (pedidos.Any(p => p.IdPedido == idPedido))
        {
            Console.WriteLine("ID do pedido já existe. Operação cancelada.");
            Console.ReadLine();
            return;
        }

        Pedido pedido = new Pedido(idPedido);

        while (true)
        {
            Produto produto = BuscarProduto();
            if (produto.Id != 0)
            {
                pedido.AdicionarProduto(produto);
                Console.WriteLine($"Produto '{produto.Nome}' adicionado ao pedido!");
            }
            else
            {
                Console.WriteLine("Produto não encontrado! Tente novamente.");
            }

            Console.Write("Deseja adicionar mais produtos? (s/n): ");
            string resposta = Console.ReadLine().ToLower();
            if (resposta != "s")
            {
                break;
            }
        }

        pedidos.Add(pedido);
        Console.WriteLine("Pedido cadastrado com sucesso! Pressione Enter para continuar.");
        Console.ReadLine();
    }

    static void ListarProdutos()
    {
        Console.Clear();
        Console.WriteLine("=== Lista de Produtos ===");
        if (!produtos.Any())
        {
            Console.WriteLine("Nenhum produto cadastrado.");
        }
        else
        {
            foreach (var produto in produtos.OrderBy(p => p.Id))
            {
                Console.WriteLine($"ID: {produto.Id}, Nome: {produto.Nome}, Preço: {produto.Preco:C}");
            }
        }
        Console.WriteLine("Pressione Enter para continuar.");
        Console.ReadLine();
    }

    static void ListarPedidos()
    {
        Console.Clear();
        Console.WriteLine("=== Lista de Pedidos ===");
        if (!pedidos.Any())
        {
            Console.WriteLine("Nenhum pedido cadastrado.");
        }
        else
        {
            foreach (var pedido in pedidos)
            {
                Console.WriteLine($"Pedido ID: {pedido.IdPedido}, Total: {pedido.Total:C}");
                foreach (var produto in pedido.Produtos)
                {
                    Console.WriteLine($" - Produto: {produto.Nome}, Preço: {produto.Preco:C}");
                }
            }
        }
        Console.WriteLine("Pressione Enter para continuar.");
        Console.ReadLine();
    }

    static void ExibirTotalVendas()
    {
        Console.Clear();
        decimal totalVendas = pedidos.Sum(p => p.Total);
        Console.WriteLine($"Total de vendas: {totalVendas:C}");
        Console.WriteLine("Pressione Enter para continuar.");
        Console.ReadLine();
    }

    static Produto BuscarProduto()
    {
        Console.WriteLine("=== Busca de Produto ===");
        Console.Write("Digite o ID ou Nome do Produto: ");
        string entrada = Console.ReadLine();

        Produto produtoEncontrado;
        if (int.TryParse(entrada, out int id))
        {
            produtoEncontrado = produtos.FirstOrDefault(p => p.Id == id);
        }
        else
        {
            produtoEncontrado = produtos.FirstOrDefault(p => p.Nome.IndexOf(entrada, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        if (produtoEncontrado.Id == 0)
        {
            Console.WriteLine("Produto não encontrado.");
        }

        return produtoEncontrado;
    }

    static int LerInteiro(string mensagem)
    {
        int valor;
        while (true)
        {
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out valor) && valor > 0)
                return valor;
            Console.WriteLine("Valor inválido. Tente novamente.");
        }
    }

    static decimal LerDecimal(string mensagem)
    {
        decimal valor;
        while (true)
        {
            Console.Write(mensagem);
            if (decimal.TryParse(Console.ReadLine(), out valor))
                return valor;
            Console.WriteLine("Valor inválido. Tente novamente.");
        }
    }
}

