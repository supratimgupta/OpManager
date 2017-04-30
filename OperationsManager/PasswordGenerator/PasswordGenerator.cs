using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.PasswordGenerator
{
    public class PasswordGenerator
    {
        string password = null;
        char[] arrCaps;
        char[] arrSmall;
        char[] arrDigits;
        char[] arrSpecialCharacter;

        public void InitializePasswordArrays()
        {
            int ch = 65;
            int i;
            arrCaps = new char[26];
            arrSmall = new char[26];
            arrDigits = new char[10];
            arrSpecialCharacter = new char[193];
            for (i = 0; i < 26; i++)
            {
                arrCaps[i] = (char)(ch);
                arrSmall[i] = (char)(ch + 32);
                ch++;
            }
            ch = 48;
            for (i = 0; i < 10; i++)
            {
                arrDigits[i] = (char)ch++;
            }
            ch = 0;
            i = 0;
            while (i < 193)
            {
                if ((!(ch >= 65 && ch <= 90) && !(ch >= 48 && ch <= 57) && !(ch >= 97 && ch <= 122) && !(ch == 32 || ch == 5)))
                {
                    arrSpecialCharacter[i++] = (char)(ch);

                }
                ch++;
            }
        }
        public string GeneratePassword(PasswordDTO passwordConfig)
        {

            string password = null;
            List<char> finalPasswordList = null;
            Random r = null;
            char ch;
            try
            {
                if (passwordConfig != null)
                {
                    if (passwordConfig.PasswordLength != 0)
                    {
                        //get random Caps
                        finalPasswordList = new List<char>();
                        r = new Random();
                        for (int i = 0; i < passwordConfig.CapitalLettersLength; i++)
                        {

                            ch = arrCaps[r.Next(arrCaps.Length)];
                            finalPasswordList.Add(ch);
                        }
                        //get random Smalls

                        for (int i = 0; i < passwordConfig.SmallLettersLength; i++)
                        {

                            ch = arrSmall[r.Next(arrSmall.Length)];
                            finalPasswordList.Add(ch);
                        }
                        //get random Digits
                        for (int i = 0; i < passwordConfig.DigitsLength; i++)
                        {

                            ch = arrDigits[r.Next(arrDigits.Length)];
                            finalPasswordList.Add(ch);
                        }

                        //get random Special Characters
                        for (int i = 0; i < passwordConfig.SpecialCharactersLength; i++)
                        {

                            ch = arrSpecialCharacter[r.Next(arrSpecialCharacter.Length)];
                            finalPasswordList.Add(ch);
                        }

                        Random rFinal = new Random();
                        finalPasswordList = new List<char>(finalPasswordList.OrderBy(ran => rFinal.Next()).ToList());
                        password = new string(finalPasswordList.ToArray());

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return password;
        }
    }
}