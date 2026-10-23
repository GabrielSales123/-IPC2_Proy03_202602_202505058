
namespace AutorizadorDTE.BackendAPI.Services
{
    public class ValidadorNIT
    {
        public static char CalcularDigitoVerificador(string nit)
{
    int suma = 0;
    int posicion = 2;

    for (int i = nit.Length - 1; i >= 0; i--)
    {
        int digito = int.Parse(nit[i].ToString());
        suma += digito * posicion;
        posicion++;
    }

    int modulo = suma % 11;
    int resultado = (11 - modulo) % 11;

    return resultado == 10 ? 'K' : resultado.ToString()[0];
}

        
public static bool Validar(string nit)
{
    if (string.IsNullOrWhiteSpace(nit))
    {
        return false;
    }
    if (nit.Length < 2 || nit.Length > 21)
    {
        return false;
    }

    string numeros = nit.Substring(0, nit.Length - 1);
    char verificadorIngresado = char.ToUpperInvariant(nit[nit.Length - 1]);
    foreach (char caracter in numeros)
    {
        if (!char.IsDigit(caracter))
        {
            return false;
        }
    }

    if (!char.IsDigit(verificadorIngresado) && verificadorIngresado != 'K')
    {
        return false;
    }

    char verificadorCalculado = CalcularDigitoVerificador(numeros);

        return verificadorIngresado == verificadorCalculado;
    }

    }
}
