using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace myRecipeBook.Domain.Extensions
{
    public static class StringExtension
    {
        //quando receber um valor nulo ou vazio, retorna true, caso contrário, retorna false
        public static bool IsEmpty([NotNullWhen(false)]this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        // se receber um valor com espaçoes em branco, retorna false, caso contrário, retorna true
        public static bool IsNotEmpty([NotNullWhen(true)]this string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
