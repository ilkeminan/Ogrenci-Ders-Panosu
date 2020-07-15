using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OgrenciDersPanosu.Models
{
    //Extension Method 
    public static partial class Html
    {
        public const int SinavKatPuani = 60;
        public const int SozluKatPuani = 40;

        public static double? NotHesapla(this HtmlHelper html, Not not)
        {
            int count = 0;
            double? notOrt = null;
            if (!IsNull(not.Sinav1))
            {
                if (!IsNull(not.Sozlu1))
                {
                    count++;
                    notOrt = (not.Sinav1 * SinavKatPuani + not.Sozlu1 * SozluKatPuani) / 100;

                    if (!IsNull(not.Sinav2))
                    {
                        if (!IsNull(not.Sozlu2))
                        {
                            count++;
                            notOrt += (not.Sinav2 * SinavKatPuani + not.Sozlu2 * SozluKatPuani) / 100;
                            if (!IsNull(not.Sinav3))
                            {
                                if (!IsNull(not.Sozlu3))
                                {
                                    count++;
                                    notOrt += (not.Sinav3 * SinavKatPuani + not.Sozlu3 * SozluKatPuani) / 100;

                                }
                            }
                        }
                    }
                }
            }
            return notOrt.HasValue ? notOrt / count : null;
        }

        public static bool IsNull(int? value)
        {
            return value == null;
        }
    }
}