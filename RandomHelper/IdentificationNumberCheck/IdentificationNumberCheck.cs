namespace Core.Utilities.RandomHelper.IdentificationNumberCheck
{
    public static class IdentificationNumberCheck
    {
        public static bool IsValidTCKimlikNo(string tcNumber)
        {
            // TC numarası uzunluğunun 11 olması gerekiyor.
            if (tcNumber.Length != 11)
            {
                return false;
            }

            // TC numarasının tüm karakterleri sayı olmalıdır.
            for (int i = 0; i < 11; i++)
            {
                if (!char.IsDigit(tcNumber[i]))
                {
                    return false;
                }
            }

            // TC numarasının ilk hanesi sıfır olmamalıdır.
            if (tcNumber[0] == '0')
            {
                return false;
            }

            // TC numarasının son hanesi, ilk 10 hanenin doğru bir şekilde hesaplanması sonucu elde edilen kontrol rakamı olmalıdır.
            int[] tcNumberArray = new int[11];
            for (int i = 0; i < 11; i++)
            {
                tcNumberArray[i] = int.Parse(tcNumber[i].ToString());
            }
            int sumFirstTenDigits = tcNumberArray[0] + tcNumberArray[1] + tcNumberArray[2] + tcNumberArray[3] + tcNumberArray[4] + tcNumberArray[5] + tcNumberArray[6] + tcNumberArray[7] + tcNumberArray[8] + tcNumberArray[9];
            int controlDigit = (sumFirstTenDigits % 10);
            if (controlDigit != tcNumberArray[10])
            {
                return false;
            }

            // Tüm kontroller geçildi, TC numarası doğru.
            return true;
        }
    }
}
