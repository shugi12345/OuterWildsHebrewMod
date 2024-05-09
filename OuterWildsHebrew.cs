using OuterWildsHebrew;
using OWML.Common;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace OuterWildsHebrew
{
    public class OuterWildsHebrew : ModBehaviour
    {
        public static OuterWildsHebrew Instance;

        private void Start()
        {
            var api = ModHelper.Interaction.TryGetModApi<ILocalizationAPI>("xen.LocalizationUtility");
            api.RegisterLanguage(this, "Hebrew", "assets/Translation.xml");
            api.AddLanguageFixer("Hebrew", HebrewFixer);
            api.AddLanguageFont(this, "Hebrew", "assets/alef-bold", "Assets/alef-bold.ttf");
        }
        public static string HebrewFixer(string input)
        {
                // Reverse the entire string
                char[] charArray = input.ToCharArray();
                Array.Reverse(charArray);
                string reversedString = new string(charArray);

                // Reverse only the English segments within the reversed string and insert newline characters every 20 characters
                StringBuilder result = new StringBuilder();
                bool inEnglishSegment = false;
                StringBuilder englishSegment = new StringBuilder();
                int charCount = 0;

                int NumOfLines = reversedString.Length / 60;

                foreach (char c in reversedString)
                {
                    if (IsEnglishCharacter(c) || IsSpecialCharacter(c))
                    {
                        if (!inEnglishSegment)
                        {
                            inEnglishSegment = true;
                        }
                        englishSegment.Append(c);
                    }
                    else
                    {
                        if (inEnglishSegment)
                        {
                            inEnglishSegment = false;
                            char[] englishArray = englishSegment.ToString().ToCharArray();
                            Array.Reverse(englishArray);
                            result.Append(new string(englishArray));
                            englishSegment.Clear();
                        }
                        result.Append(c);
                    }

                    // Increment character count
                    charCount++;

                    // Check if newline encountered to split the line
                    if (c == '\n')
                    {
                        charCount = 0;
                    }

                    // Check if 20 characters have been processed and if it's a space to split the line
                    if (input.Length - charCount < 60 * NumOfLines && c == ' ')
                    {
                        NumOfLines--;
                        result.AppendLine();
                        charCount = 0;
                    }
                }

                // Handle the case when the last segment is English
                if (inEnglishSegment)
                {
                    char[] englishArray = englishSegment.ToString().ToCharArray();
                    Array.Reverse(englishArray);
                    result.Append(new string(englishArray));
                }

                // Split by newline characters, reverse the order, and join the lines back together
                string[] lines = result.ToString().Split('\n');
                Array.Reverse(lines);
                return string.Join("\n", lines);
        }



        public static bool IsEnglishCharacter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        public static bool IsSpecialCharacter(char c)
        {
            // Add any other special characters you want to consider here
            return c == '<' || c == '&' || c == '>' || c == ';' || c == '>' || c == '/' || c == '=';
        }

    }
}
