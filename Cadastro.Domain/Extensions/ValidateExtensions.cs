using System;
using System.Text.RegularExpressions;

namespace Cadastro.Domain.Extensions
{
    public static class ValidateExtensions
    {
        #region CEPValido
        public static bool CEPValido(this string cep)
        {
            var ERegular = @"^\d{5}\-?\d{3}$";

            return Regex.IsMatch(cep, ERegular, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        #endregion

        #region CPFValido
        public static bool CPFValido(this string cpf)
        {
            var ERegular = @"^\d{3}\.?\d{3}\.?\d{3}\-?\d{2}$";

            return Regex.IsMatch(cpf, ERegular, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        #endregion

        #region CNPJValido
        public static bool CNPJValido(this string cnpj)
        {
            string CNPJ = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;

            ftmt = "6543298765432";
            digitos = new int[14];

            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;

            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;

            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = Convert.ToInt32(CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                    {
                        soma[0] += (digitos[nrDig] * Convert.ToInt32(ftmt.Substring(nrDig + 1, 1)));
                    }

                    if (nrDig <= 12)
                    {
                        soma[1] += (digitos[nrDig] * Convert.ToInt32(ftmt.Substring(nrDig, 1)));
                    }
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                    {
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                    }
                    else
                    {
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                    }
                }

                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
