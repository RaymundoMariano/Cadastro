namespace Cadastro.Service.Extensions
{
    public static class RemoveExtensions
    {
        #region RemoveMascara
        public static string RemoveMascara(this object campo)
        {
            if (campo is null || string.IsNullOrEmpty(campo.ToString())) return null;

            string str = campo.ToString().Replace(".", "").Replace("-", "").Replace("/", "");
            str = str.Replace("(", "").Replace(")", "");
            return str;
        }
        #endregion

        #region RemoveAcentos
        public static string RemoveAcentos(this string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(input);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        #endregion
    }
}
