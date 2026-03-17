using System;
using System.Collections.Generic;
using System.Threading;

class Contador
{
    public int Id { get; private set; }
    public int Valor { get; private set; }
    public int Intervalo { get; private set; }

    private bool activo;
    private Thread hilo;

    public Contador(int id, int intervalo)
    {
        Id = id;
        Intervalo = intervalo;
        Valor = 0;
        activo = false;
    }

    public void Iniciar()
    {
        if (activo)
        {
            Console.WriteLine($"El contador {Id} ya está en ejecución.");
            return;
        }

        activo = true;
        hilo = new Thread(Ejecutar);
        hilo.IsBackground = true;
        hilo.Start();
    }

    private void Ejecutar()
    {
        while (activo)
        {
            Valor++;
            Console.WriteLine($"[Contador {Id}] -> {Valor}");
            Thread.Sleep(Intervalo);
        }
    }

    public void Detener()
    {
        if (!activo)
        {
            Console.WriteLine($"El contador {Id} ya está detenido.");
            return;
        }

        activo = false;
    }

    public string Estado()
    {
        return $"Contador {Id}: Valor={Valor}, Estado={(activo ? "Activo" : "Detenido")}";
    }
}

class Program
{
    static List<Contador> contadores = new List<Contador>();
    static int siguienteId = 1;

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
            Console.Write("Seleccione una opción: ");

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
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }
    }

    static void IniciarContador()
    {
        Console.Write("Ingrese intervalo en milisegundos: ");
        if (!int.TryParse(Console.ReadLine(), out int intervalo) || intervalo <= 0)
        {
            Console.WriteLine("Intervalo inválido.");
            return;
        }

        Contador c = new Contador(siguienteId++, intervalo);
        contadores.Add(c);
        c.Iniciar();

        Console.WriteLine($"Contador {c.Id} iniciado.");
    }

    static void DetenerContador()
    {
        Console.Write("Ingrese ID del contador: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        foreach (var c in contadores)
        {
            if (c.Id == id)
            {
                c.Detener();
                return;
            }
        }

        Console.WriteLine("Contador no encontrado.");
    }

    static void MostrarEstado()
    {
        Console.WriteLine("\n--- ESTADO DE CONTADORES ---");

        if (contadores.Count == 0)
        {
            Console.WriteLine("No hay contadores.");
            return;
        }

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

        Console.WriteLine("Todos los contadores han sido detenidos.");
    }
}
