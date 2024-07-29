using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string diretorio = @""; // Caminho dos arquivos
        string textoProcurado1 = ""; // Primeiro texto a ser procurado
        string textoProcurado2 = ""; // Segundo texto a ser procurado (pode ser nulo ou vazio)
        string[] arquivos = Directory.GetFiles(diretorio);

        Dictionary<string, List<string>> resultados = new Dictionary<string, List<string>>();

        if (arquivos.Length > 0)
        {
            int totalArquivosProcessados = 0;
            int totalLinhasEncontradas = 0;

            foreach (string caminhoDoArquivo in arquivos)
            {
                string nomeDoArquivo = Path.GetFileName(caminhoDoArquivo);
                totalArquivosProcessados++;

                if (File.Exists(caminhoDoArquivo))
                {
                    string[] linhas = File.ReadAllLines(caminhoDoArquivo);
                    for (int i = 0; i < linhas.Length; i++)
                    {
                        if (linhas[i].IndexOf(textoProcurado1, StringComparison.OrdinalIgnoreCase) >= 0 &&
                            (string.IsNullOrEmpty(textoProcurado2) || linhas[i].IndexOf(textoProcurado2, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            if (!resultados.ContainsKey(nomeDoArquivo))
                            {
                                resultados[nomeDoArquivo] = new List<string>();
                            }
                            resultados[nomeDoArquivo].Add($"Linha {i + 1}: {linhas[i]}");
                            totalLinhasEncontradas++;
                        }
                    }
                }
            }

            // Cria a pasta "relatorio" se não existir
            string pastaRelatorio = Path.Combine(diretorio, "relatorio");
            if (!Directory.Exists(pastaRelatorio))
            {
                Directory.CreateDirectory(pastaRelatorio);
            }

            // Caminho do arquivo de relatório
            string caminhoRelatorio = Path.Combine(pastaRelatorio, "relatorio.txt");

            using (StreamWriter writer = new StreamWriter(caminhoRelatorio))
            {
                // Verifica se foram encontradas linhas
                if (totalLinhasEncontradas > 0)
                {
                    writer.WriteLine("Total de Arquivos Processados: " + totalArquivosProcessados);
                    writer.WriteLine("Total de Arquivos Encontrados: " + resultados.Count);
                    writer.WriteLine("Total de Linhas Encontradas: " + totalLinhasEncontradas);
                    writer.WriteLine();
                    writer.WriteLine("Resultados:");
                    foreach (var resultado in resultados)
                    {
                        writer.WriteLine($"Arquivo: {resultado.Key}");
                        foreach (var linha in resultado.Value)
                        {
                            writer.WriteLine(linha);
                        }
                        writer.WriteLine();
                    }
                }
                else
                {
                    writer.WriteLine("Nenhuma linha encontrada.");
                    writer.WriteLine("Parâmetros de pesquisa:");
                    writer.WriteLine("Texto procurado 1: " + textoProcurado1);
                    writer.WriteLine("Texto procurado 2: " + (string.IsNullOrEmpty(textoProcurado2) ? "N/A" : textoProcurado2));
                }
            }

            // Exibe na tela o conteúdo do arquivo de relatório
            Console.WriteLine();
            Console.WriteLine("Resultados gravados em: " + caminhoRelatorio);
            Console.WriteLine("Conteúdo do relatório:");
            Console.WriteLine(File.ReadAllText(caminhoRelatorio));
        }
        else
        {
            Console.WriteLine("Não existem arquivos no diretório.");
        }
    }
}
