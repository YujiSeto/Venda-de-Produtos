using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

// Alunos: Daniel Azevedo | Rodrigo Yuji

namespace Venda_de_Produtos
{
    class Program
    {
        private static Semaphore Semaforo;

        static void Main(string[] args)
        {
            //Quantidade de Filiais e Produtos
            int numFil;
            int numProd;

            //Console
            string opcao = "0";

            //Randomizador:
            Random rnd = new Random();
            int random;
            int minQtdRandom = 10;
            int maxQtdRandom = 20;
            int minValorRandom = 1;
            int maxValorRandom = 100;

            //Semaforo
            Semaforo = new Semaphore(1, 1);

            //FOR
            int i;
            int j;

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
                if (numFil <= 0 || numFil > 100)
                {
                    Console.WriteLine("ERRO!!!");
                    Console.WriteLine("O número de filiais não pode ser 0, menor que 0 ou maior que 100");
                    Console.ReadKey();
                }
            } while (numFil <= 0 || numFil > 200);
            do
            {
                Console.Clear();
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("|      CONFIGURAÇÃO VENDA DE PRODUTOS      |");
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("");
                Console.WriteLine("Digite o número de Produtos:");
                numProd = Convert.ToInt32(Console.ReadLine());
                if (numProd <= 0 || numProd > 200)
                {
                    Console.WriteLine("ERRO!!!");
                    Console.WriteLine("O número de produtos não pode ser 0, menor que 0 ou maior que 200");
                    Console.ReadKey();
                }
            } while (numProd <= 0 || numProd > 200);

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
                    estoqueProd[j] += qtdeEstoque[i,j];
                    estoqueGeral += qtdeEstoque[i,j];

                    random = rnd.Next(minValorRandom, maxValorRandom);
                    valorProd[i,j] = random;
                    valorFil[i]+= valorProd[i, j];
                    valorGeral += valorProd[i, j]* qtdeEstoque[i, j];
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
                        Console.WriteLine("|          Minímo: 1 Máximo 1000           |");
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("");
                        qtdVendas = Convert.ToInt32(Console.ReadLine());
                        if (qtdVendas < 1 || qtdVendas > 1000)
                        {
                            Console.WriteLine("ERRO!!!");
                            Console.WriteLine("O número de Vendas não pode ser menor que 1 ou maior que 500");
                            Console.ReadKey();
                        }
                    } while (qtdVendas < 1 || qtdVendas > 1000);
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
                if (qtdeEstoque[randomFilial, randomProdutos] <= 0)
                {
                    Console.WriteLine("Venda " + numVenda + " Não Realizada Quantidade Insuficiente em Estoque");
                }
                else
                {
                    qtdVenda = rnd.Next(1, qtdeEstoque[randomFilial, randomProdutos]);
                    if (qtdVenda <= qtdeEstoque[randomFilial, randomProdutos])
                    {
                        valorVenda = qtdVenda * valorProd[randomFilial, randomProdutos];
                        valorFil[randomFilial] -= valorVenda;
                        valorGeral -= valorVenda;
                        qtdeEstoque[randomFilial, randomProdutos] -= qtdVenda;
                        estoqueGeral -= qtdVenda;
                        Console.WriteLine("Venda:" + numVenda + " Filial: " + codigoFil[randomFilial]);
                        Console.WriteLine("Codigo do Produto:" + codigoProd[randomFilial, randomProdutos] + " | Descrição do Produto: " + nomeProd[randomFilial, randomProdutos] + " | Quantidade de Venda: " + qtdVenda + " | Valor Unitário: R$" + valorProd[randomFilial, randomProdutos] + ",00");
                        Console.WriteLine("Valor do Pedido: R$" + valorVenda + ",00");
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine("Venda " + numVenda + " Não Realizada Quantidade Insuficiente em Estoque");
                    }
                }
                cont++;
                Semaforo.Release();
            }
        }
    }
}
