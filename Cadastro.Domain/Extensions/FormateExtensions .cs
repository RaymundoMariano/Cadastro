namespace Cadastro.Domain.Extensions
{
    public static class FormateExtensions
    {
        #region FormateCGC
        public static string FormateCGC(this object cgc)
        {
            string str = cgc.ToString().PadLeft(14, '0');
            if (str == "00000000000000")
                return string.Empty;
            str = str.Substring(0, 2) + "." + str.Substring(2, 3) + "." + str.Substring(5, 3) + "/" + str.Substring(8, 4) + "-" + str.Substring(12, 2);
            return str;
        }
        #endregion

        #region FormateCPF
        public static string FormateCPF(this object cpf)
        {
            cpf = cpf.RemoveMascara();

            if (cpf == null) return null;

            string str = cpf.ToString().PadLeft(11, '0');
            if (str == "00000000000")
                return string.Empty;
            str = str.Substring(0, 3) + "." + str.Substring(3, 3) + "." + str.Substring(6, 3) + "-" + str.Substring(9, 2);
            return str;
        }
        #endregion

        #region FormateCEP
        public static string FormateCEP(this object cep)
        {
            cep = cep.RemoveMascara();

            string str = cep.ToString().PadLeft(8, '0');
            if (str == "00000000")
                return string.Empty;
            str = str.Substring(0, 5) + "-" + str.Substring(5, 3);
            return str;
        }
        #endregion        
    }
}
