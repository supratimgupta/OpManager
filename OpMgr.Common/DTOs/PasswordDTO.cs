using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class PasswordDTO
    {
            public int CapitalLettersLength { get; set; }
            public int SmallLettersLength { get; set; }
            public int DigitsLength { get; set; }
            public int SpecialCharactersLength { get; set; }

            public int PasswordLength { get; set; }

        
    }
}
