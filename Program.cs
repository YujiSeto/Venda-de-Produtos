using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace Venda_de_Produtos
{
    class Program
    {
        private static Semaphore Semaforo;

        static void Main(string[] args)
        {
            ///Criação de Variáveis
            int numFil;
            int numProd;

            //Configuração
            do
            {
                Console.Clear();
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("|      CONFIGURAÇÃO VENDA DE PRODUTOS      |");
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("");
                Console.WriteLine("Digite o número de Filiais:");
                numFil = Convert.ToInt32(Console.ReadLine());
                if (numFil <= 0)
                {
                    Console.WriteLine("ERRO!!!");
                    Console.WriteLine("O número de filiais não pode ser 0 ou menor que 0.");
                    Console.ReadKey();
                }
            } while (numFil <= 0);
            do
            {
                Console.Clear();
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("|      CONFIGURAÇÃO VENDA DE PRODUTOS      |");
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("");
                Console.WriteLine("Digite o número de Produtos:");
                numProd = Convert.ToInt32(Console.ReadLine());
                if (numProd <= 0)
                {
                    Console.WriteLine("ERRO!!!");
                    Console.WriteLine("O número de produtos não pode ser 0 ou menor que 0.");
                    Console.ReadKey();
                }
            } while (numProd <= 0);

            //Filial:
            int[] codigoFil = new int[numFil];
            string[] nomeFil = new string[numFil];

            //Produto:
            int[,]codigoProd = new int[numFil,numProd];
            string[,] nomeProd = new string[numFil,numProd];
            int[,] qtdeEstoque = new int[numFil,numProd];
            int[,] valorProd = new int[numFil, numProd];

            //Controle
            int[] estoqueProd = new int[numProd];
            int estoqueGeral = 0;
            int[] valorFil = new int[numFil];
            int valorGeral = 0;
            int qtdVendas = 0;

            //Venda:
            int cont=1;
            int randomFilial;
            int randomProdutos;
            int numVenda;
            int qtdVenda;
            int valorVenda;

            //Randomizador:
            Random rnd = new Random();
            int random;
            int minQtdRandom = 10;
            int maxQtdRandom = 20;
            int minValorRandom = 1;
            int maxValorRandom = 100;

            //FOR
            int i;
            int j;

            //Semaforo
            Semaforo = new Semaphore(1, 1);

            //Console
            string opcao = "0";




            //Limpeza de Dados:
            for (i = 0; i < numFil; i++)
            {
                valorFil[i] = 0;
            }
            for (j = 0; j < numProd; j++)
            {
                estoqueProd[j] = 0;
            }

            //Entrada de Dados:
            for (i = 0; i < numFil; i++)
            {
                codigoFil[i] = i+1;
                nomeFil[i] = "Filial "+(i+1);
                for (j = 0; j < numProd; j++)
                {
                    codigoProd[i,j] = j + 1;
                    nomeProd[i,j] = "Produto "+(j+1);

                    random = rnd.Next(minQtdRandom, maxQtdRandom);
                    qtdeEstoque[i,j] = random;
                    estoqueProd[j] += random;
                    estoqueGeral += random;

                    random = rnd.Next(minValorRandom, maxValorRandom);
                    valorProd[i,j] = random;
                    valorFil[i]+= random;
                    valorGeral += random;
                }
            }

            //Console:
            do
            {
                Console.Clear();
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("|       VENDA DE PRODUTOS                  |");
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("  Número de Filiais  : {0}", numFil);
                Console.WriteLine("  Número de Produtos : {0}", numProd);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("  RESUMO DAS {0} FILIAIS",numFil);
                Console.WriteLine("  Total de Produtos de Estoque : {0}",estoqueGeral);
                Console.WriteLine("  Valor Total em Estoque       : R$ {0},00",valorGeral);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("|       Selecione uma opção                |");
                Console.WriteLine("|  1  - Inserir Vendas                     |");
                Console.WriteLine("|  0  - Sair do Programa                   |");
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("");
                opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("|       INSERIR VENDAS                     |");
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("");
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("  RESUMO DAS {0} FILIAIS", numFil);
                        Console.WriteLine("  Total de Produtos de Estoque : {0}", estoqueGeral);
                        Console.WriteLine("  Valor Total em Estoque       : R$ {0},00", valorGeral);
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("");
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("|Digite a quantidade de Vendas Simultâmeas:|");
                        Console.WriteLine("|          Minímo: 1 Máximo 20             |");
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("");
                        qtdVendas = Convert.ToInt32(Console.ReadLine());
                        if (qtdVendas < 1 || qtdVendas > 20)
                        {
                            Console.WriteLine("ERRO!!!");
                            Console.WriteLine("O número de Veiculos Entrando não pode ser maior que número de Cancelas.");
                            Console.ReadKey();
                        }
                    } while (qtdVendas < 1 || qtdVendas > 20);

                    Console.Clear();
                    Console.WriteLine("+------------------------------------------+");
                    Console.WriteLine("|       VENDAS                             |");
                    Console.WriteLine("+------------------------------------------+");
                    Console.WriteLine("");
                    for (i = 0; i < qtdVendas; i++)
                    {
                        Thread t = new Thread(new ThreadStart(Venda));
                        t.Start();
                    }
                    Console.ReadKey();
                }
            } while (opcao != "0");

            void Venda()
            {
                Semaforo.WaitOne();
                numVenda = cont;
                randomFilial = rnd.Next(0, numFil);
                randomProdutos = rnd.Next(0, numProd);
                qtdVenda = rnd.Next(1, qtdeEstoque[randomFilial, randomProdutos]);
                if (qtdVenda <= qtdeEstoque[randomFilial, randomProdutos])
                {
                    valorVenda = qtdVenda * valorProd[randomFilial, randomProdutos];
                    valorFil[randomFilial] -= valorVenda;
                    valorGeral -= valorVenda;
                    qtdeEstoque[randomFilial, randomProdutos] -= qtdVenda;
                    Console.WriteLine("Venda:" + numVenda + " Filial: " + codigoFil[randomFilial]);
                    Console.WriteLine("Codigo do Produto:" + codigoProd[randomFilial, randomProdutos] + " | Descrição do Produto: " + nomeProd[randomFilial, randomProdutos] + " | Quantidade de Venda: " + qtdVenda + " | Valor Unitário: R$" + valorProd[randomFilial, randomProdutos]+",00");
                    Console.WriteLine("Valor do Pedido: R$" + valorVenda + ",00");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Venda " + numVenda + " Não Realizada Quantidade Insuficiente em Estoque");
                }
                cont++;
                Semaforo.Release();
            }
        }
    }
}
