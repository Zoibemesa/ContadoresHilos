using System;
using System.Collections.Generic;
using System.Threading;

class Contador
{
    public int Id { get; set; }
    public int Valor { get; set; }
    public int Intervalo { get; set; }
    public bool Activo { get; set; }

    private Thread hilo;

    public Contador(int id, int intervalo)
    {
        Id = id;
        Intervalo = intervalo;
        Valor = 0;
        Activo = false;
    }

    public void Iniciar()
    {
        if (Activo) return;

        Activo = true;
        hilo = new Thread(Ejecutar);
        hilo.Start();
    }

    private void Ejecutar()
    {
        while (Activo)
        {
            Valor++;
            Console.WriteLine($"[Contador {Id}] Valor: {Valor}");
            Thread.Sleep(Intervalo);
        }
    }

    public void Detener()
    {
        Activo = false;
    }

    public string Estado()
    {
        return $"Contador {Id} -> Valor: {Valor}, Estado: {(Activo ? "Activo" : "Detenido")}";
    }
}

class Program
{
    static List<Contador> contadores = new List<Contador>();
    static int contadorId = 1;

    static void Main(string[] args)
    {
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("\n===== MENÚ =====");
            Console.WriteLine("1. Iniciar contador");
            Console.WriteLine("2. Detener contador");
            Console.WriteLine("3. Mostrar estado");
            Console.WriteLine("4. Salir");
            Console.Write("Opción: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    IniciarContador();
                    break;
                case "2":
                    DetenerContador();
                    break;
                case "3":
                    MostrarEstado();
                    break;
                case "4":
                    salir = true;
                    DetenerTodos();
                    break;
                default:
                    Console.WriteLine("Opción inválida");
                    break;
            }
        }
    }

    static void IniciarContador()
    {
        Console.Write("Intervalo (ms): ");
        int intervalo = int.Parse(Console.ReadLine());

        Contador c = new Contador(contadorId++, intervalo);
        contadores.Add(c);
        c.Iniciar();

        Console.WriteLine("Contador iniciado.");
    }

    static void DetenerContador()
    {
        Console.Write("ID del contador: ");
        int id = int.Parse(Console.ReadLine());

        foreach (var c in contadores)
        {
            if (c.Id == id)
            {
                c.Detener();
                Console.WriteLine("Contador detenido.");
                return;
            }
        }

        Console.WriteLine("No encontrado.");
    }

    static void MostrarEstado()
    {
        Console.WriteLine("\n--- ESTADO ---");
        foreach (var c in contadores)
        {
            Console.WriteLine(c.Estado());
        }
    }

    static void DetenerTodos()
    {
        foreach (var c in contadores)
        {
            c.Detener();
        }
        Console.WriteLine("Todos los contadores detenidos.");
    }
}
