using System;
using System.Globalization;

namespace DepreciacionSaldoDecreciente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("  DEPRECIACIÓN POR SALDO DECRECIENTE (RECURSIVO)");
            Console.WriteLine("==============================================\n");

            bool continuar = true;

            while (continuar)
            {
                // 1. Entrada de datos con validación
                decimal costoInicial = LeerDecimal("Valor de compra original (V): ", minimo: 0m, maximo: null);
                decimal tasa = LeerDecimal("Tasa de depreciación anual en % (d, entre 0 y 100): ", minimo: 0m, maximo: 100m);
                int anios = LeerEnteroNoNegativo("Años transcurridos (t): ");

                // 2. Factor de retención r = 1 - d/100 (solo informativo)
                decimal r = 1m - tasa / 100m;
                Console.WriteLine($"\nFactor de retención (r): {r.ToString("F4", CultureInfo.InvariantCulture)}");

                // 3. Detalle año por año (cada valor se calcula con la función recursiva)
                Console.WriteLine("\n--- Detalle por año ---");
                for (int i = 0; i <= anios; i++)
                {
                    decimal valorAnio = CalcularValorResidual(costoInicial, tasa, i);
                    string etiqueta = i == 0 ? " (Caso Base)" : "";
                    Console.WriteLine($"Año {i}: ${valorAnio.ToString("N2", CultureInfo.InvariantCulture)}{etiqueta}");
                }

                // 4. Resultado final
                decimal valorFinal = CalcularValorResidual(costoInicial, tasa, anios);
                Console.WriteLine("\n----------------------------------------------");
                Console.WriteLine($"Valor residual después de {anios} año(s): ${valorFinal.ToString("N2", CultureInfo.InvariantCulture)}");
                Console.WriteLine("----------------------------------------------");

                Console.Write("\n¿Desea realizar otro cálculo? (s/n): ");
                string respuesta = (Console.ReadLine() ?? "").Trim().ToLower();
                continuar = respuesta == "s" || respuesta == "si" || respuesta == "sí";
                Console.WriteLine();
            }

            Console.WriteLine("Programa finalizado. ¡Hasta luego!");
        }

        static decimal CalcularValorResidual(decimal costoInicial, decimal tasaDepreciacion, int t)
        {
            // Caso base: t = 0 -> el valor es el costo original
            if (t == 0)
            {
                return costoInicial;
            }

            // Caso recursivo: V(t) = V(t - 1) * (1 - d/100)
            return CalcularValorResidual(costoInicial, tasaDepreciacion, t - 1)
                   * (1m - tasaDepreciacion / 100m);
        }

        static decimal LeerDecimal(string mensaje, decimal minimo, decimal? maximo)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = (Console.ReadLine() ?? "").Trim().Replace(',', '.');

                if (!decimal.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal valor))
                {
                    Console.WriteLine("  Error: ingrese un número válido.\n");
                    continue;
                }

                if (valor < minimo)
                {
                    Console.WriteLine($"  Error: el valor no puede ser menor que {minimo}.\n");
                    continue;
                }

                if (maximo.HasValue && valor > maximo.Value)
                {
                    Console.WriteLine($"  Error: el valor no puede ser mayor que {maximo.Value}.\n");
                    continue;
                }

                return valor;
            }
        }
        static int LeerEnteroNoNegativo(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = (Console.ReadLine() ?? "").Trim();

                if (!int.TryParse(entrada, out int valor))
                {
                    Console.WriteLine("  Error: ingrese un número entero válido.\n");
                    continue;
                }

                if (valor < 0)
                {
                    Console.WriteLine("  Error: los años no pueden ser negativos.\n");
                    continue;
                }

                return valor;
            }
        }
    }
}