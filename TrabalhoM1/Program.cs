using System;
using System.Collections.Generic;
using System.Linq;

namespace TrabalhoM1
{
    class Program
    {
        public static MatrixAdjacencia AdjacenciaMatrix { get; set; }

        public static IList<ListaAdjacencia> AdjacenciaList { get; set; }

        static void Main(string[] args)
        {
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) // sair
                    return;

                if(key.Key == ConsoleKey.NumPad0 || key.Key == ConsoleKey.D0) // Cria um arco não orientado e não ponderado
                    CriarGrafo(MatrixType.NOrientadoNPonderado);
                else if (key.Key == ConsoleKey.NumPad1 || key.Key == ConsoleKey.D1) // Cria um arco orientado e não ponderado
                    CriarGrafo(MatrixType.Orientado);
                else if (key.Key == ConsoleKey.NumPad2 || key.Key == ConsoleKey.D2) // Cria um arco não orientado e ponderado
                    CriarGrafo(MatrixType.Ponderado);
                else if (key.Key == ConsoleKey.NumPad3 || key.Key == ConsoleKey.D3) // Cria um arco orientado e ponderado
                    CriarGrafo(MatrixType.OrientadoPonderado);
                else if (key.Key == ConsoleKey.A || key.Key == ConsoleKey.Add) // Add Vertice
                {
                    Console.WriteLine("Nome do vertice");
                    AdjacenciaMatrix.Insert(Console.ReadLine());
                }
                else if (key.Key == ConsoleKey.X) // Remove Vertice
                {
                    Console.WriteLine("Nome do vertice");
                    AdjacenciaMatrix.Remove(Console.ReadLine());
                }
                else if (key.Key == ConsoleKey.O) // Add Arco ou Arresta
                {
                    AdjacenciaMatrix.InsertArcoOrArresta();
                }
                else if (key.Key == ConsoleKey.V) // retorna Vertice
                {
                    Console.WriteLine("Nome do vertice:");
                    var vertice = Console.ReadLine();
                    var v = AdjacenciaMatrix.RetornaVertice(vertice);
                    if(v == null)
                        return;
                    
                    Console.Write(v.Name + " -> ");
                    foreach (var item in v.Items)
                    {
                        Console.Write($"{item.Name} - {item.Peso}");
                        if (v.Items.IndexOf(item) != v.Items.Count - 1)
                            Console.Write(" | ");
                    }
                }
                else if (key.Key == ConsoleKey.E) // Verifica se existe arresta
                {
                    Console.WriteLine("Origem");
                    var origem = Console.ReadLine();
                    Console.WriteLine("Destino");
                    var destino = Console.ReadLine();
                    Console.WriteLine(AdjacenciaMatrix.ExisteArresta(origem, destino));
                }
                else if (key.Key == ConsoleKey.R) // Retorna arrestas
                {
                    Console.WriteLine("Nome do vertice:");
                    var vertice = Console.ReadLine();
                    var items = AdjacenciaMatrix.RetornaArresta(vertice);
                    foreach (var item in items)
                    {
                        Console.Write($"{item.Name} - {item.Peso}");
                        if(items.IndexOf(item) != items.Count - 1)
                            Console.Write(" | ");
                    }
                }
                else if(key.Key == ConsoleKey.P)
                {
                    AdjacenciaMatrix.Print();
                }
            }
        }

        private static void CriarGrafo(MatrixType type)
        {
            AdjacenciaMatrix = new MatrixAdjacencia(type);
            Console.WriteLine("Grafo criado");
        }
    }

    class MatrixAdjacencia 
    {
        public MatrixAdjacencia(MatrixType type)
        {
            Type = type;
            Vertices = new List<Vertice>();
        }

        public MatrixType Type { get; set; }

        public IList<Vertice> Vertices { get; set; }

        public void Print()
        {
            Console.Clear();
            Console.WriteLine();

            var verticesOrdened = Vertices.OrderBy(c => c.Name).ToList();

            foreach (var vertex in verticesOrdened)
            {
                vertex.Items = vertex.Items.OrderBy(c => c.Name).ToList();
            }

            foreach (var vertex in verticesOrdened)
            {
                Console.Write(vertex.Name + " -> ");
                foreach (var item in vertex.Items)
                {
                    Console.Write($"{item.Name}[{item.Peso}]");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            for (var i = 0; i < verticesOrdened.Count(); i++)
            {
                if (i == 0)
                {
                    Console.Write(" ");
                    for (var j = 0; j < verticesOrdened.Count(); j++)
                    {
                        Console.Write($"{verticesOrdened.ElementAt(j).Name} ");
                    }
                    Console.WriteLine();
                }
                var vertex = verticesOrdened.ElementAt(i);

                Console.WriteLine();
                Console.Write(vertex.Name);

                var items = new List<VerticeItem>(vertex.Items);

                for (int g = 0; g < verticesOrdened.Count; g++)
                {
                    var item = items.FirstOrDefault();

                    if (item != null && verticesOrdened.ElementAt(g).Name== item.Name)
                    {
                        Console.Write($"{item.Peso} ");
                        items.Remove(item);
                    }
                    else
                    {
                        Console.Write("0 ");
                    }
                }

                //foreach (var item in vertex.Items)
                //{
                //    if (verticesOrdened.IndexOf(verticesOrdened.FirstOrDefault(c => c.Name == item.Name)) == i)
                //    {
                //        Console.Write($"{item.Peso} ");
                //    }
                //    else
                //    {
                //        Console.Write("0 ");
                //    }
                //}
                

            }


        }

        public void Insert(string name)
        {
            if (Vertices.Any(c => c.Name == name))
            {
                Console.WriteLine("Vertice já existe");
            }
            else
            {
                Vertices.Add(new Vertice(name));
                Console.WriteLine("Vertice criado");
            }
        }

        public void Remove(string name)
        {
            var item = Vertices.FirstOrDefault(c => c.Name == name);

            Vertices.Remove(item);
            Console.WriteLine("Vertice removido");
        }

        public void InsertArcoOrArresta()
        {
            var origem = "";
            while (string.IsNullOrEmpty(origem))
            {
                Console.Write("Origem: ");
                origem = Console.ReadLine();
                if (Vertices.All(c => c.Name != origem))
                {
                    Console.WriteLine($"Origem '{origem}' não existe");
                    origem = string.Empty;
                }
            }
            var destino = string.Empty;

            while (string.IsNullOrEmpty(destino))
            {
                Console.Write("Destino: ");
                destino = Console.ReadLine();
                if (Vertices.All(c => c.Name != destino))
                {
                    Console.WriteLine($"Destino '{destino}' não existe");
                    destino = string.Empty;
                }
            }

            var peso = "1";
            if (Type.HasFlag(MatrixType.Ponderado))
            {
                Console.WriteLine("Informe o peso do arco/arresta:");
                peso = Console.ReadLine();
            }

            if (Type.HasFlag(MatrixType.Orientado))
            {
                InsertArco(origem, destino, peso);
            }
            else
            {
                InsertArresta(origem, destino, peso);
            }
        }

        public void InsertArco(string origem, string destino, string peso)
        {
            if (!Type.HasFlag(MatrixType.Orientado))
            {
                return;
            }

            var verticeOrigem = Vertices.FirstOrDefault(c => c.Name == origem);
            if(verticeOrigem == null)
                return;

            verticeOrigem.Items.Add(new VerticeItem()
            {
                Name = destino,
                Peso = peso
            });
            
            Console.WriteLine("Arco adicionado");
        }

        public void RemoveArco(string origem, string destino)
        {
            if (!Type.HasFlag(MatrixType.Orientado))
            {
                return;
            }

            var verticeOrigem = Vertices.FirstOrDefault(c => c.Name == origem);
            if (verticeOrigem == null)
                return;

            var verticeItem = verticeOrigem.Items.FirstOrDefault(c => c.Name == destino);
            verticeOrigem.Items.Remove(verticeItem);

            Console.WriteLine("Arco Removido");
        }

        public void InsertArresta(string origem, string destino, string peso)
        {
            if (Type.HasFlag(MatrixType.Orientado))
            {
                return;
            }

            var verticeOrigem = Vertices.FirstOrDefault(c => c.Name == origem);
            if (verticeOrigem == null)
                return;

            verticeOrigem.Items.Add(new VerticeItem()
            {
                Name = destino,
                Peso = peso
            });

            var verticeDestino = Vertices.FirstOrDefault(c => c.Name == destino);
            if (verticeDestino == null)
                return;

            verticeDestino.Items.Add(new VerticeItem()
            {
                Name = origem,
                Peso = peso
            });
            Console.WriteLine("Arresta adicionada");
        }

        public void RemoveArresta(string origem, string destino)
        {
            if (Type.HasFlag(MatrixType.Orientado))
            {
                return;
            }

            var verticeOrigem = Vertices.FirstOrDefault(c => c.Name == origem);
            if (verticeOrigem == null)
                return;

            var verticeItem = verticeOrigem.Items.FirstOrDefault(c => c.Name == destino);
            verticeOrigem.Items.Remove(verticeItem);

            var verticeDestino = Vertices.FirstOrDefault(c => c.Name == destino);
            if (verticeDestino == null)
                return;

            verticeItem = verticeOrigem.Items.FirstOrDefault(c => c.Name == origem);
            verticeOrigem.Items.Remove(verticeItem);

            Console.WriteLine("Arresta removida");
        }

        public Vertice RetornaVertice(string name)
        {
            return Vertices.FirstOrDefault(c => c.Name == name);
        }

        public bool ExisteArresta(string origem, string destino)
        {
            return Vertices.Any(c => c.Name == origem && c.Items.Any(d => d.Name == destino));
        }

        public IList<VerticeItem> RetornaArresta(string name)
        {
            return Vertices.FirstOrDefault(c => c.Name == name)?.Items;
        }


    }

    class Vertice
    {
        public Vertice(string name)
        {
            Name = name;
            Items = new List<VerticeItem>();
        }

        public string Name { get; set; }
        public IList<VerticeItem> Items { get; set; }
    }

    class VerticeItem
    {
        public string Name { get; set; }
        public string Peso { get; set; }
    }

    [Flags]
    internal enum MatrixType
    {
        NOrientadoNPonderado=1,
        Orientado = 2,
        Ponderado = 4,
        OrientadoPonderado = Orientado|Ponderado
    }

    struct ListaAdjacencia
    {
        public int From { get; set; }
        public IList<int> To { get; set; }
    }
}
