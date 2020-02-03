
using PortfolioAPI.Logic.DI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortfolioAPI.Logic.Puzzles
{
    public class EnigmaPuzzle : IPuzzle
    {
        #region constant Data
        private static Regex isAlpha = new Regex("^[A-Z]*$");

        private static List<List<int>> notch = new List<List<int>>
        {
            new List<int>{ 16, 16},
            new List<int>{ 4, 4},
            new List<int>{ 21, 21},
            new List<int>{ 9, 9},
            new List<int>{ 25, 25},
            new List<int>{ 25, 12},
            new List<int>{ 25, 12},
            new List<int>{ 25, 12}
        };
        #endregion

        #region public facing logic
        public String Encrypt(String plain, String keySettings, String ringSettings, List<int> rotorSettings)
        {
            plain = plain.ToUpper();
            String cipherText = "";

            List<int> key = new List<int>();
            List<int> ring = new List<int>();
            List<int> rotorValues = rotorSettings.Select(x => x - 1).ToList();
            ring.AddRange(ringSettings.Select(x => code(x.ToString())));
            key.AddRange(keySettings.Select(x => code(x.ToString())));

            for (var i = 0; i < plain.Length; i++)
            {
                var ch = plain[i];
                if (!isAlpha.IsMatch(ch.ToString()))
                {
                    cipherText += ch;
                }
                else
                {
                    key = incrementSettings(key, rotorValues);
                    cipherText = cipherText + enigma(ch, key, rotorValues, ring);
                }
            }
            return cipherText;
        }
        #endregion

        #region internal logic
        private static String enigma(Char chr, List<int> key, List<int> rotors, List<int> ring)
        {
            String strOutput = chr.ToString();
            strOutput = rotor(strOutput, rotors[2], key[2] - ring[2]);
            strOutput = rotor(strOutput, rotors[1], key[1] - ring[1]);
            strOutput = rotor(strOutput, rotors[0], key[0] - ring[0]);
            strOutput = simpleSub(strOutput[0], "YRUHQSLDPXNGOKMIEBFZCWVJAT");
            strOutput = rotor(strOutput, rotors[0] + 8, key[0] - ring[0]);
            strOutput = rotor(strOutput, rotors[1] + 8, key[1] - ring[1]);
            strOutput = rotor(strOutput, rotors[2] + 8, key[2] - ring[2]);
            return strOutput;
        }   
        
        private List<int> incrementSettings(List<int> key, List<int> rotors)
        {
            //notch = [['Q','Q'],['E','E'],['V','V'],['J','J'],['Z','Z'],['Z','M'],['Z','M'],['Z','M']];
            if(key[1] == notch[rotors[1]][0] || key[1] == notch[rotors[1]][1] )
            {
                key[0] = (key[0] + 1) % 26;
                key[1] = (key[1] + 1) % 26;
            }

            var key2 = key[2];
            var notch1 = notch[rotors[2]][0];
            var notch2 = notch[rotors[2]][1];

            if (key[2] == notch[rotors[2]][0] || key[2] == notch[rotors[2]][1] )
            {
                key[1] = (key[1] + 1) % 26;
            }
            key[2] = (key[2] + 1) % 26;

            return key;
        }

        private static String rotor(String chr, int r, int offset)
        {
            var key = new List<String> {"EKMFLGDQVZNTOWYHXUSPAIBRCJ", "AJDKSIRUXBLHWTMCQGZNPYFVOE", "BDFHJLCPRTXVZNYEIWGAKMUSQO",
                       "ESOVPZJAYQUIRHXLNFTGKDCMWB", "VZBRGITYUPSDNHLXAWMJQOFECK", "JPGVOUMFYQBENHZRDKASXLICTW",
                       "NZJHGRCXMYSWBOUFAIVLPEKQDT", "FKQHTLXOCBJSPDZRAMEWNIUYGV",
                       "UWYGADFPVZBECKMTHXSLRINQOJ", "AJPCZWRLFBDKOTYUQGENHXMIVS", "TAGBPCSDQEUFVNZHYIXJWLRKOM",
                       "HZWVARTNLGUPXQCEJMBSKDYOIF", "QCYLXWENFTZOSMVJUDKGIARPHB", "SKXQLHCNWARVGMEBJPTYFDZUIO",
                       "QMGYVPEDRCWTIANUXFKZOSLHJB", "QJINSAYDVKBFRUHMCPLEWZTGXO" };

            var chcode = (code(chr) + 26 + offset) % 26;


            var letterTest = key[r][chcode].ToString();
            var mapch = (code(key[r][chcode].ToString()) + 26 - offset) % 26 + 65;

            return ((char)mapch).ToString();
        }

        private static int code(String character)
        {
            return (int)(character.ToUpper().FirstOrDefault()) - 65;
        }    
        
        private static String simpleSub(Char character, String key)
        {
            return key[code(character.ToString())].ToString();
        }
        #endregion


    }
}
