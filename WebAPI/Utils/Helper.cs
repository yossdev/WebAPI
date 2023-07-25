namespace WebAPI.Utils
{
    public static class Helper
    {
        public static string ProcessNIK(string nik)
        {
            var split = nik.Split('-');
            split[0] = split[0].ToUpper();
            nik = string.Join("-", split);
            return nik;
        }
    }
}
