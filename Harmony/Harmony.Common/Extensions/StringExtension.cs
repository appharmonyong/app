using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Common.Extensions
{
    public static class StringExtension
    {
        public static string ToCapitalize(this string str)
        {
            var wordList = str.Split(' ').ToList();
            string output = string.Empty;

            wordList.ForEach(word =>
            {
                output += " " + word.Substring(0, 1).ToUpper() + word.Substring(1);
            });

            return output.Substring(1);

        }

        public static Guid ToGuid(this string str)
        {
            return Guid.Parse(str);
        }

        public static T? ToObject<T>(this string jsonString) where T : class
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        ///     ''' Remueve los caracteres especiales de la cadena de caracteres
        ///     ''' </summary>
        ///     ''' <param name="str">String a evaluar</param>
        ///     ''' <returns>String normalizado sin caracteres especiales</returns>
        ///     ''' <remarks></remarks>
        public static string NormalizeString(this string str)
        {
            string aString = str;
            char[] toReplace = "àèìòùÀÈÌÒÙ äëïöüÄËÏÖÜ âêîôûÂÊÎÔÛ áéíóúÁÉÍÓÚðÐýÝ ãñõÃÑÕšŠžŽçÇåÅøØ".ToCharArray();
            char[] replaceChars = "aeiouAEIOU aeiouAEIOU aeiouAEIOU aeiouAEIOUdDyY anoANOsSzZcCaAoO".ToCharArray();
            for (int index = 0; index <= toReplace.GetUpperBound(0); index++)
                aString = aString.Replace(toReplace[index], replaceChars[index]);
            return aString;
        }

        /// <summary>
        ///     ''' Completa o trunca el string hasta el largo requerido
        ///     ''' </summary>
        ///     ''' <param name="Value">Valor a ingresar</param>
        ///     ''' <param name="Length">Largo maximo del valor</param>
        ///     ''' <param name="Filler">Complemento del espacio en blanco</param>
        ///     ''' <param name="Direction">Direccion del complemento, 'left' o 'right' para adicionarlo a la izquierda o derecha del string</param>
        ///     ''' <returns>String complementado</returns>
        ///     ''' <remarks></remarks>
        public static string FillWith(this string Value, int Length, string Filler = " ", string Direction = "right")
        {
            string result = Value;
            string toadd = "";
            while (result.Length + toadd.Length < Length)
                toadd += Filler;

            result = Direction.ToLower() switch
            {
                "left" => toadd + result,
                "right" => result + toadd,
                _ => throw new Exception($"Direccion del texto '{Direction}' no reconocida, contacte al departamento de TI")
            };

            if (result.Length > Length)
            {
                string str = "";
                int leftOverflow = Direction.ToLower() == "left" ? result.Length - Length : 0;
                int index = 0;
                while (index < Length)
                {
                    str += result[leftOverflow + index].ToString();
                    index++;
                }
                result = str;
            }
            return result;
        }
    }
}
