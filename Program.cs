using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



public class Transaccion
{
    public DateTime Fecha { get; set; }
    public string Descripcion { get; set; }
    public decimal Monto { get; set; }

}

public class Usuario
{
    CajeroAutomatico cajero = new CajeroAutomatico();

    public string NombreUsuario { get; set; }
    public string Contrasena { get; set; }
    public decimal Saldo { get; private set; }
    public List<Transaccion> Transacciones { get; private set; }

    public Usuario(string usuario, string contrasena)
    {
        NombreUsuario = usuario;
        Contrasena = contrasena;
        Saldo = 0;
        Transacciones = new List<Transaccion>();
    }




    public class CajeroAutomatico
    {
        public static List<Usuario> usuarios = new List<Usuario>();
        // ...
    }





    public void RealizarTransaccion(string descripcion, decimal monto)
    {
        var transaccion = new Transaccion
        {
            Fecha = DateTime.Now,
            Descripcion = descripcion,
            Monto = monto
        };

        Transacciones.Add(transaccion);
        Saldo += monto;

    }

    public void MostrarInformacion()
    {
        Console.WriteLine("--------------------------------------------------\nInformación de la cuenta:\n--------------------------------------------------\n");
        Console.WriteLine("Usuario: " + NombreUsuario);
        Console.WriteLine("Saldo: Q." + Saldo);

        Console.WriteLine("\n--------------------------------------------------\nTransacciones:\n--------------------------------------------------\n");
        foreach (var transaccion in Transacciones)
        {

            Console.WriteLine($" -  Fecha: {transaccion.Fecha}, Descripción: {transaccion.Descripcion}, Monto: Q. {transaccion.Monto}");
        }
        Console.WriteLine("\n\n--------------------------------------------------");
    }

}

public class CajeroAutomatico
{


    public List<Usuario> usuarios;
    private Usuario usuarioActual;


    public void CargarUsuarios(string RutaArchivo)
    {
        using (StreamReader reader = new StreamReader(RutaArchivo))
        {
            string line;
            int lineN = 0;
            while ((line = reader.ReadLine()) != null)
            {
                String[] x = line.Split(',');
                string usuario = x[0].ToString();
                string contrasena = x[1].ToString();
                decimal saldo = decimal.Parse(x[2]);

                Usuario user = CrearUsuario(usuario, contrasena, 0);



                lineN++;
                while (line != "%")
                {
                    line = reader.ReadLine();
                    if (line != "%")
                    {
                        String[] y = line.Split(',');
                        DateTime fecha = DateTime.Parse(y[0]);
                        string descripcion = y[1].ToString();
                        decimal monto = decimal.Parse(y[2]);
                        user.RealizarTransaccion(descripcion, monto);

                    }
                    else
                    {
                        break;
                    }
                    lineN++;
                }
                lineN++;
            }
        }
    }
    public void GuardarUsuarios(string RutaArchivo)
    {
        using (var writer = new StreamWriter(RutaArchivo))
        {
            foreach (var usuario in usuarios)
            {
                writer.WriteLine($"{usuario.NombreUsuario},{usuario.Contrasena},{usuario.Saldo}");

                foreach (var transaccion in usuario.Transacciones)
                {
                    writer.WriteLine($"{transaccion.Fecha},{transaccion.Descripcion},{transaccion.Monto}");
                }
                writer.WriteLine("%");

            }
        }
    }
    public Usuario IniciarSesion(string nombreUsuario, string contraseña)
    {
        foreach (var usuario in usuarios)
        {
            if (usuario.NombreUsuario == nombreUsuario && usuario.Contrasena == contraseña)
            {
                usuarioActual = usuario;
                Console.WriteLine("---- Sesión iniciada exitosamente. ----");
                Console.WriteLine($"Bienvenido {usuario.NombreUsuario}.");
                return usuarioActual;
            }
        }
        Console.WriteLine("Numero de cuenta o contraseña incorrectos.");
        Console.Write("Presione cualquier tecla para continuar...");
        Console.ReadKey();
        return null;
    }



    public decimal ConsultarSaldo()
    {
        return usuarioActual.Saldo;
    }

    public Usuario CrearUsuario(string usuario, string contrasena, decimal saldoInicial)
    {
        foreach (var user in usuarios)
        {
            if (user.NombreUsuario == usuario)
            {
                Console.WriteLine("Este numero de cuenta ya existe");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return null;
            }
        }
        var nuevoUsuario = new Usuario(usuario, contrasena);
        if (saldoInicial > 0)
        {

            nuevoUsuario.RealizarTransaccion("Saldo Inicial", saldoInicial);
        }
        usuarios.Add(nuevoUsuario);
        return nuevoUsuario;


    }


    public void CerrarSesion()
    {
        usuarioActual = null;
        Console.WriteLine("---------- Sesión cerrada. ----------");
        Console.Write("Presione cualquier tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
        MenuPrincipal();
    }

    public void MenuPrincipal()
    {

        Console.WriteLine(@"   ___  ___     __  ____ ____    ___       ___  __ __ ______   ___   ___  ___  ___  ______ __   ___   ___  
  //   // \\    || ||    || \\  // \\     // \\ || || | || |  // \\  ||\\//|| // \\ | || | ||  //    // \\ 
 ((    ||=||    || ||==  ||_// ((   ))    ||=|| || ||   ||   ((   )) || \/ || ||=||   ||   || ((    ((   ))
  \\__ || ||   ||| ||_   || \\  \\ //     || || \\//    ||    \\_//  ||    || || ||   ||   ||  \\    \\_//");
        Console.WriteLine("\n\n");
        Console.WriteLine("1. Iniciar sesión");
        Console.WriteLine("2. Crear nueva cuenta");
        Console.WriteLine("3. Salir");
        string Res = Console.ReadLine();


        switch (Res)
        {
            case "1":
                Console.Clear();
                Login();
                break;

            case "2":
                Register();
                break;
            case "3":
                GuardarUsuarios("usuarios.txt");
                Environment.Exit(0);
                break;
            case "4":

            default:
                Console.Clear();
                Console.WriteLine("Opción no válida. Intente nuevamente.");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                break;
        }

    }

    public void Register()
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------------------------------------------\n");
        Console.WriteLine(@" ____   ____   ___  __  __  ______ ____   ___  ____ 
 || \\ ||     // \\ || (( \ | || | || \\ // \\ || \\
 ||_// ||==  (( ___ ||  \\    ||   ||_// ||=|| ||_//
 || \\ ||___  \\_|| || \_))   ||   || \\ || || || \\");
        Console.WriteLine("\n-------------------------------------------------------------------------\n\n");
        Console.WriteLine("Ingrese un numero de cuenta para la nueva cuenta: ");
        string nuevoNombreUsuario = Console.ReadLine();
        Console.WriteLine("Ingrese una contraseña para la nueva cuenta: ");
        string nuevaContraseña = Console.ReadLine();
        Console.WriteLine("Ingrese el saldo inicial para la nueva cuenta: ");
        try
        {
            decimal saldoInicial = decimal.Parse(Console.ReadLine());
            if (saldoInicial < 0)
            {
                Console.WriteLine("Saldo no válido. Intente nuevamente.");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
                Register();
            }
            else if (saldoInicial > 0)
            {
                CrearUsuario(nuevoNombreUsuario, nuevaContraseña, saldoInicial);
                Console.WriteLine("Cuenta creada exitosamente.");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
                GuardarUsuarios("usuarios.txt");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Saldo no válido. Intente nuevamente.");
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
            Register();
        }
    }

    public void Login()
    {
        Console.WriteLine("-------------------------------------------------------------------------\n");
        Console.WriteLine(@" __ __  __ __   ___ __  ___  ____      __   ____  __  __   ___   __  __
 || ||\ || ||  //   || // \\ || \\    (( \ ||    (( \ ||  // \\  ||\ ||
 || ||\\|| || ((    || ||=|| ||_//     \\  ||==   \\  || ((   )) ||\\||
 || || \|| ||  \\__ || || || || \\     \)) ||__  \_)) ||  \\_//  || \||");

        Console.WriteLine("\n-------------------------------------------------------------------------\n\nIngrese su numero de cuenta: ");
        string nombreUsuario = Console.ReadLine();
        Console.WriteLine("Ingrese su contraseña: ");
        string contraseña = Console.ReadLine();
        if (IniciarSesion(nombreUsuario, contraseña) != null)
        {
            Console.Clear();
            MenuUsuario();
        };


    }

    public void MenuUsuario()
    {
        Console.WriteLine("-----------------------------------------------------------------------------------\n");
        Console.WriteLine(@" ___  ___  ____ __  __ __ __    ____  ____  __ __  __   ___ __ ____   ___  __   
 ||\\//|| ||    ||\ || || ||    || \\ || \\ || ||\ ||  //   || || \\ // \\ ||   
 || \/ || ||==  ||\\|| || ||    ||_// ||_// || ||\\|| ((    || ||_// ||=|| ||   
 ||    || ||___ || \||  \\//    ||    || \\ || || \||  \\_  || ||    || || ||__|");
        Console.WriteLine("\n-----------------------------------------------------------------------------------\n\n");
        Console.WriteLine("1. Consultar saldo");
        Console.WriteLine("2. Realizar transacción");
        Console.WriteLine("3. Historial de Transacciones");
        Console.WriteLine("4. Cerrar sesión");
        string Res = Console.ReadLine();
        switch (Res)
        {
            case "1":
                Console.Clear();
                Console.WriteLine("--------------------------------");
                Console.WriteLine("Su saldo es: Q. " + ConsultarSaldo());
                Console.WriteLine("--------------------------------");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();

                MenuUsuario();
                break;

            case "2":
                Console.Clear();
                Transacciones();
                break;

            case "3":
                Console.Clear();
                usuarioActual.MostrarInformacion();
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
                MenuUsuario();
                break;


            case "4":
                Console.Clear();
                GuardarUsuarios("usuarios.txt");
                CerrarSesion();
                break;

            default:
                Console.WriteLine("Opción no válida. Intente nuevamente.");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
                MenuUsuario();
                break;

        }
    }

    public void Transacciones()
    {
        Console.WriteLine("-----------------------------------------------------------------------------------");
        Console.WriteLine(@" ______ ____   ___  __  __  __   ___    ___   ___ __   ___   __  __  ____  __ 
 | || | || \\ // \\ ||\ || (( \ // \\  //    //   ||  // \\  ||\ || ||    (( \
   ||   ||_// ||=|| ||\\||  \\  ||=|| ((    ((    || ((   )) ||\\|| ||==   \\ 
   ||   || \\ || || || \||  \)) || ||  \\_   \\__ ||  \\ //  || \|| ||___ \_))");
        Console.WriteLine("----------------------------------------------------------------------------------- \n\n");
        Console.WriteLine("1. Retiro");
        Console.WriteLine("2. Depósito");
        Console.WriteLine("3. Salir");
        string Res = Console.ReadLine();

        if (Res == "1")
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----------------------------------------\n");
                Console.WriteLine(@" ____   ____ ______ __ ____    ___  
 || \\ ||    | || | || || \\  // \\ 
 ||_// ||==    ||   || ||_// ((   ))
 || \\ ||___   ||   || || \\  \\_//");
                Console.WriteLine("----------------------------------------\n\n");
                Console.WriteLine("Ingrese el monto a retirar: ");

                String monto = Console.ReadLine();
                decimal MontoC = 0;

                if (!monto.Contains("-") && !monto.Contains("+") && monto.All(Char.IsDigit) && monto != "")
                {
                    MontoC = decimal.Parse(monto);
                    if (ConsultarSaldo() < MontoC)
                    {
                        Console.WriteLine("Saldo insuficiente. Intente nuevamente.");
                        Console.Write("Presione cualquier tecla para continuar...");
                        Console.ReadKey();

                    }
                    else
                    {
                        usuarioActual.RealizarTransaccion("Retiro", -MontoC);

                        Console.WriteLine("\n\n---- Transaccion Realizada ----");
                        Console.WriteLine("Su saldo actual es: " + ConsultarSaldo());
                        Console.WriteLine("--------------------------------");
                        Console.Write("Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        Transacciones();
                        GuardarUsuarios("usuarios.txt");
                        break;
                    }

                }

                else
                {
                    Console.WriteLine("Monto no válido. Intente nuevamente.");
                    Console.Write("Presione cualquier tecla para continuar...");
                    Console.ReadKey();

                }

            }

        }
        else if (Res == "2")
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--------------------------------------------------\n");
                Console.WriteLine(@" ____    ____ ____    ___    __  __ ______   ___  
 || \\  ||    || \\  // \\  (( \ || | || |  // \\ 
 ||  )) ||==  ||_// ((   ))  \\  ||   ||   ((   ))
 || //  ||__  ||     \\_//  \_)) ||   ||    \\_//");
                Console.WriteLine("\n--------------------------------------------------\n\n");
                Console.WriteLine("Ingrese el monto a depositar: ");

                String monto = Console.ReadLine();
                decimal MontoC = 0;

                if (!monto.Contains("-") && !monto.Contains("+") && monto.All(Char.IsDigit) && monto != "")
                {
                    MontoC = decimal.Parse(monto);
                    usuarioActual.RealizarTransaccion("Depósito", MontoC);
                    Console.WriteLine("\n--------------------------------");
                    Console.WriteLine("Su saldo actual es: " + ConsultarSaldo());
                    Console.WriteLine("--------------------------------");
                    Console.Write("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                    Transacciones();
                    GuardarUsuarios("usuarios.txt");
                    break;
                }

                else
                {
                    Console.WriteLine("Monto no válido. Intente nuevamente.");
                    Console.Write("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                }

            }
        }
        else if (Res == "3")
        {
            Console.Clear();
            MenuUsuario();
        }
        else
        {
            Console.WriteLine("Opción no válida. Intente nuevamente.");
            Console.ReadKey(true);
        }


    }
}

class Program
{
    static void Main(string[] args)
    {

        CajeroAutomatico cajero = new CajeroAutomatico
        {
            usuarios = new List<Usuario>()
        };
        String RutaArchivo = "usuarios.txt";
        if (File.Exists(RutaArchivo))
        {
            cajero.CargarUsuarios(RutaArchivo);
        }



        while (true)
        {
            Console.Clear(); // Limpiar la consola

            cajero.MenuPrincipal();





        }
    }
}