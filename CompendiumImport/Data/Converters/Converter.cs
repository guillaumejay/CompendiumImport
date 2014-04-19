using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CompendiumImport.Tools;
using HtmlAgilityPack;
using Masterplan.Data;

namespace CompendiumImport.Data.Converters
{
    /// <summary>
    /// Basic abstract class for converting Data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Converter<T>:IConverter<T>
    {

        protected static Regex _regIsAttack = new Regex(@"\+([0-9])* vs(\.)? (AC|Will|Reflex|Fortitude)", RegexOptions.IgnoreCase);

        public abstract T GetMasterPlanObjectFromDoc(HtmlDocument doc, List<string> warnings);        

        public IRole GetLevelAndRoleFrom(string temp, out int level)
        {
            RoleType rt;
            string[] tempArray = temp.Replace("Level", "").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            level = Convert.ToInt32(tempArray[0]);
            //c.Role = new ComplexRole(Enum.Parse(typeof(RoleType), Upper1(temp[1]));
            IRole role;
            if (temp.Contains("Minion"))
            {
                role = new Minion();
                
                if (EnumUtility.TryParseEnum<RoleType>(Upper1(tempArray[2]), true, out rt))
                {
                    (role as Minion).HasRole = true;
                    (role as Minion).Type = rt;
                }
            }
            else
            {
                RoleFlag rf = RoleFlag.Standard;
                int index = 1;
                if (tempArray[1].ToLower() == "solo")
                    rf = RoleFlag.Solo;
                if (tempArray[1].ToLower() == "elite")
                    rf = RoleFlag.Elite;
                if (rf != RoleFlag.Standard)
                    index = 2;
                
                //if (!EnumUtility.TryParse(,out rt))
                //{
                //    throw new InvalidStructureException(" ")
                //}
                role = new ComplexRole((RoleType)Enum.Parse(typeof(RoleType), Upper1(tempArray[index])));
                (role as ComplexRole).Leader = temp.Contains("Leader");
                (role as ComplexRole).Flag = rf;
            }

            return role;
        }
        /// <summary>
        /// Cleans the HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        protected string CleanHtml(string html)
        { //TODO check encoding
            string tmp = HtmlEntity.DeEntitize(html).Replace("â€“", "-").Replace("â€œ", "'").Replace("â€", "'").Replace("â€™", "'").Replace("’", "'");
            return tmp.Replace("'™","'").Trim();
        }

        protected  string Upper1(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            return s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
        }

        /// <summary>
        /// Cleans the parenthesis, and extra comma
        /// </summary>
        /// <param name="text">string to clean.</param>
        /// <returns></returns>
        protected static string CleanParenthesis(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            string cleaned = text.Replace("(", "").Replace(")", "").Trim();
            cleaned = CleanExtraComma(cleaned);
            return cleaned.Trim();
        }

        /// <summary>
        /// Removes , and ; at start or end of text
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Cleaned Text</returns>
        protected static string CleanExtraComma(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            char[] toRemove = new char[] { ',', ';' };
            text = text.Trim();
            bool modified;
            do
            {
                modified = false;
                foreach (char c in toRemove)
                {
                    if (text.EndsWith(c.ToString()))
                    {
                        text = text.Substring(0, text.Length - 1);
                        modified = true;
                    }
                    if (text.StartsWith(c.ToString()))
                    {
                        text = text.Substring(1).Trim();
                        modified = true;
                    }
                }    
            } while (modified);
            
            return text.Trim();
        }

        /// <summary>
        /// Get a string representing the bonus
        /// </summary>
        /// <param name="p">integer value for bonus</param>
        /// <returns></returns>
        protected string AsBonus(int p)
        {
            string ret = p.ToString();
            if (p > 0)
                ret = "+" + p;
            return ret;
        }

        protected string GetTextUntilTag(string temp, string startString)
        {
            return GetTextUntilTag(temp, startString, 1);
        }

        /// <summary>
        /// Gets the text until a html tag open
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="startString">The start string.</param>
        /// <param name="count">how many start string to search for.</param>
        /// <returns></returns>
        protected string GetTextUntilTag(string text, string startString, int count)
        {
            int pos = 0;
            if (!String.IsNullOrEmpty(startString))
            {
                pos = -1;
                do
                {
                    pos = text.IndexOf(startString, pos + 1);
                    if (pos == -1)
                        break;
                    count--;
                } while (count > 0);

                if (pos == -1)
                    return null;
                pos += (startString.Length);
            }
            if (pos >= text.Length)
                return null;
            StringBuilder sb = new StringBuilder();
            while (text[pos] != '<')
            {
                sb.Append(text[pos]);
                pos++;
                if (pos >= text.Length)
                    break;
            }
            return CleanHtml(sb.ToString()).Trim();
        }

        /// <summary>
        /// Get Int value from a string +|-xx
        /// </summary>
        /// <param name="temp">The temp.</param>
        /// <param name="startString">The start string.</param>
        /// <returns></returns>
        protected int GetIntValue(string temp, string startString)
        {
            int pos = 0;
            if (!String.IsNullOrEmpty(startString))
            {
                pos = temp.IndexOf(startString);
                Trace.Assert(pos >= 0);
                pos += (startString.Length);
            }
            string number = "";
            while (temp[pos] == ' ' || temp[pos] == '+' || temp[pos] == '-' || char.IsNumber(temp[pos]))
            {
                char c1 = temp[pos];
                if (c1 == ' ')
                {
                    if (!String.IsNullOrEmpty(number))
                        break;
                }
                else
                {
                    {
                        number += c1;
                    }
                }
                pos++;
                if (pos >= temp.Length)
                    break;
            }

            return Convert.ToInt32(number);
        }
        /// <summary>
        /// Checks a text to see if there's an attack definition in it : adds it if found
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>PowerAttack, or null if fail</returns>
        protected PowerAttack TryToGetAttackFrom(string text)
        {
            Match match = _regIsAttack.Match(text); //Try to look for range ?
            if (match.Success)
            {
                string temp = match.Value;
                PowerAttack pa = new PowerAttack();
                pa.Bonus = GetIntValue(temp, null);
                temp = temp.Substring(temp.IndexOf("vs") + 3).Trim();
                pa.Defence = (DefenceType)(Enum.Parse(typeof(DefenceType), temp));
                return pa;
            }
            return null;
        }
    }
}
