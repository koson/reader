using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BBeBinderPlugins
{
    public class HtmlParser
    {
        const int STRUCT = 0;
        const int TEXT = 1;
        const int STRUCT_END = 2;

        bool table_special = false;
        string par_html = "</font>\n";


        string fontcenter = "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>\n";
        string fontright = "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>\n";
        string fontleft = "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>\n";

        string footnotecolor1 = "<font color='#000000'>";
        string footnotecolor2 = "</font>";

        string footnote_txt = "";
        int footnote_count = 0;

        bool par_is_p = false; // Should be passed in from main application
        bool no_small_footnotes = false; // Should be passed in from main application

        StringBuilder output = new StringBuilder();
        string ptext;
        int parbr;

        Hashtable myStruct = new Hashtable();
        List<Hashtable> stack = new List<Hashtable>();
        ArrayList[] footnote_stack = new ArrayList[3];

        int stack_count;
        bool flag_caps;
        bool flag_li = false;
        bool flag_ul = false;
        bool flag_b;
        bool flag_i;
        bool flag_u;
        bool flag_font;
        bool flag_table;
        bool flag_td;
        bool flag_b_r;
        bool flag_i_r;
        bool flag_u_r;
        int special_footnotes;
        bool special_footnotes_flag;
        bool flag_image = false;
        int flag_image_doc;
        string fontfootnote;


        public HtmlParser( bool par_is_p, bool no_small_footnotes)
        {
            this.par_is_p = par_is_p;
            this.no_small_footnotes = no_small_footnotes;

            setparbr(1);

            flag_b = false;
            flag_i = false;
            flag_u = false;

            flag_b_r = false;
            flag_i_r = false;
            flag_u_r = false;


            flag_font = false;
            flag_table = false;

            flag_td = false;
            flag_caps = false;
            output.Length = 0;

            fontfootnote = "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='1'>\n";

            special_footnotes = 0;
            special_footnotes_flag = false;

            flag_image = false;

        }

        public string go(string input)
        {
            int pos = 0;
            int count1 = 0;
            while ((pos = input.IndexOf("<table>", pos + 1)) != -1)
                count1++;
            pos = 0;
            int count2 = 0;
            while ((pos = input.IndexOf("</table>", pos + 1)) != -1)
                count2++;
            if (count1 != count2) //substr_count(input,"<table") != substr_count(input,"</table>")) 
                table_special = true;


            pos = 0;
            count1 = 0;
            while ((pos = input.IndexOf("##", pos)) != -1)
                count1++;
            flag_image_doc = count1;

            for (int i = 0; i < footnote_stack.Length; ++i)
                footnote_stack[i] = new ArrayList();

            htmlparser(input);
            Console.WriteLine("finished");

            // footnotes 
            int count = 0;


            Console.WriteLine("doing footnotes");
            while (count < footnote_count)
            {
                newpar("ptext" + fontfootnote);
                stack[stack_count++] = (Hashtable)footnote_stack[STRUCT][count];
                myStruct["FOOTNOTE"] = false;
                scmp();
                spop();
                string x = (string)footnote_stack[TEXT][count];

                if (!no_small_footnotes)
                {
                    x = x.Replace("' size='3'>", "' size='1'>");
                    x = x.Replace("' size='2'>", "' size='1'>");
                }

                output.Append(x);
                myStruct = (Hashtable)footnote_stack[STRUCT_END][count];
                count++;
            }

            if (flag_image)
            {
                output.Append("</b></i></u></td></table>");
                if (flag_b)
                    output.Append("<b>");
                if (flag_i)
                    output.Append("<i>");
                if (flag_u)
                    output.Append("<u>");
            }

            input = output.ToString();
            output.Length = 0;

            Console.WriteLine("correcting html");
            input = correctHtml(input);

            return input;
        }

        void setparbr(int x)
        {
            if (par_is_p)
                x = 0;
            parbr = x;
            if (parbr != 0)
            {
                ptext = "<br>";
            }
            else
            {
                ptext = "<p>";
            }
        }

        void newpar(string text)
        {
            if (flag_b)
                output.Append("</b>");
            if (flag_i)
                output.Append("</i>");
            if (flag_u)
                output.Append("</u>");
            if (getInt(myStruct["SUP"]) != 0)
                output.Append("</sup>");

            if (flag_li)
                output.Append("</li>");
            if (flag_ul)
                output.Append("</ul>");

            flag_li = false;
            flag_ul = false;

            output.Append(par_html);

            if (parbr == 0)
            {
                output.Append("</p>");
            }
            else if (getBool(myStruct["QR"]) || getBool(myStruct["QC"]))
                output.Append("</p>");

            output.Append(text);

            if (flag_b)
                output.Append("<b>");
            if (flag_i)
                output.Append("<i>");
            if (flag_u)
                output.Append("<u>");
            if (getInt(myStruct["SUP"]) != 0)
                output.Append("<sup>");
        }

        void spush()
        {
            myStruct["CAPS"] = flag_caps;
            myStruct["B"] = flag_b;
            myStruct["I"] = flag_i;
            myStruct["U"] = flag_u;

            stack.Add(myStruct);
            stack_count++;
            myStruct = new Hashtable(myStruct);
        }

        void spop()
        {
            if (stack_count == 0)
                return;

            myStruct = null;
            --stack_count;
            myStruct = (Hashtable)(stack[stack_count]);
            stack.RemoveAt(stack_count);

            flag_caps = getBool(myStruct["CAPS"]);
            flag_b = getBool(myStruct["B"]);
            flag_i = getBool(myStruct["I"]);
            flag_u = getBool(myStruct["U"]);

            int x = getInt(myStruct["SAB"]) == 0 ? 1 : 0;
            setparbr(x);
        }

        void printflag(string command, bool state)
        {
            if (state)
                output.Append("<" + command + ">");
            else
                output.Append("</" + command + ">");
        }

        public int getInt(Object val)
        {
            if (val == null)
                return 0;
            else
                return (int)val;
        }
        public bool getBool(Object val)
        {
            if (val == null)
                return false;
            else
                return (bool)val;
        }

        void scmp()
        {
            if (stack_count == 0)
                return;

            Hashtable x = stack[stack_count - 1];

            if (getBool(x["FOOTNOTE"]) != getBool(myStruct["FOOTNOTE"]))
            {
                footnote_stack[TEXT][footnote_count - 1] = output.ToString();

                myStruct["CAPS"] = flag_caps;
                myStruct["B"] = flag_b;
                myStruct["I"] = flag_i;
                myStruct["U"] = flag_u;
                myStruct["FOOTNOTE"] = false;

                footnote_stack[STRUCT_END][footnote_count - 1] = myStruct;

                output.Length = 0;
                output.Append(footnote_txt);
                footnote_txt = "";
                spop();
                x = stack[stack_count - 1];
                myStruct["FOOTNOTE"] = false;
            }

            if (flag_b != getBool(x["B"]))
                printflag("b", getBool(x["B"]));
            if (flag_i != getBool(x["I"]))
                printflag("i", getBool(x["I"]));
            if (flag_u != getBool(x["U"]))
                printflag("u", getBool(x["U"]));

            if (getBool(x["SUP"]) != getBool(myStruct["SUP"]))
                printflag("sup", getBool(x["SUP"]));

            if ((getBool(x["QC"]) != getBool(myStruct["QC"])) || (getBool(myStruct["QR"]) != getBool(x["QR"])))
            {
                // change state 
                if (getBool(x["QC"]) != false)
                    newpar("<p align=\"center\">" + fontcenter);
                else if (getBool(x["QR"]) != false)
                    newpar("<p align=\"right\">" + fontright);
                else
                    newpar("ptext" + fontleft);
            }
        }



        void htmlparser_text(string htmltext)
        {
            if (special_footnotes != 0 && !flag_table && flag_image_doc != 0)
            {
                //  ##1 images
                string flgs = fontleft;

                if (flag_b)
                    flgs += "<b>";
                if (flag_i)
                    flgs += "<i>";
                if (flag_u)
                    flgs += "<u>";

                if (flag_image)
                {
                    Regex re = new Regex("((##[0-9]*))");
                    htmltext = re.Replace(htmltext, "</td><tr><td>" + flgs + "$2", 1);
                }
                else
                {
                    string htmltext0 = htmltext;
                    Regex re = new Regex("((##[0-9]*))");
                    htmltext = re.Replace(htmltext, "<table><td>" + flgs + "$2", 1);
                    if (htmltext.Equals(htmltext0))
                        flag_image = true;
                }
            }

            // special: end table 
            if (flag_caps)
            {
                string[] pattern = new string[] { "(a)", "(ä)", "(á)", "(b)", "(c)", "(è)", "(d)", "(ï)", "(e)", "(ì)", "(é)", "(ë)", "(f)", "(g)", "(h)", "(i)", "(í)", "(j)", "(k)", "(l)", "(µ)", "(å)", "(m)", "(n)", "(ñ)", "(ò)", "(o)", "(ó)", "(ö)", "(ô)", "(p)", "(q)", "(r)", "(à)", "(ø)", "(s)", "(¹)", "(t)", "(»)", "(u)", "(ü)", "(ú)", "(ù)", "(v)", "(w)", "(x)", "(y)", "(ý)", "(z)", "(¾)" };
                string[] replace = new string[] { "A", "Ä", "Á", "B", "C", "È", "D", "Ï", "E", "Ì", "É", "Ë", "F", "G", "H", "I", "Í", "J", "K", "L", "¥", "Å", "M", "N", "Ñ", "Ò", "O", "Ó", "Ö", "Ô", "P", "Q", "R", "À", "Ø", "S", "©", "T", "«", "U", "Ü", "Ú", "Ù", "V", "W", "X", "Y", "Ý", "Z", "®" };
                for (int i = 0; i < pattern.Length; ++i)
                {
                    htmltext = Regex.Replace(htmltext, pattern[i], replace[i]);
                }
            }

            if (table_special && flag_table)
            {
                if (htmltext.Trim().Length > 40)
                {
                    if (flag_font)
                    {
                        output.Append("</font>");
                        flag_font = false;
                        flag_caps = false;
                    }

                    if (flag_b)
                    {
                        output.Append("</b>");
                        flag_b = false;
                        flag_b_r = false;
                    }
                    if (flag_i)
                    {
                        output.Append("</i>");
                        flag_i = false;
                        flag_i_r = false;
                    }
                    if (flag_u)
                    {
                        output.Append("</u>");
                        flag_u = false;
                        flag_u_r = false;
                    }

                    if (flag_td)
                    {
                        output.Append("</td>");
                        flag_td = false;
                    }

                    output.Append("</table>");
                    flag_table = false;

                    output.Append("</p>" + ptext + fontleft + "\n");

                    output.Append(htmltext);

                    return;
                }
            }

            // table without <td>
            if (!flag_td && flag_table)
            {
                output.Append("<td>" + fontleft + "\n");
                flag_td = true;
                flag_font = true;
            }

            // without font 
            if (!flag_font)
            {
                output.Append(fontleft + "\n");
                flag_font = true;
                flag_caps = false;
            }

            // td in the table 
            if (flag_td)
            {
                if (flag_b_r && !flag_b)
                {
                    output.Append("<b>");
                    flag_b = true;
                }
                if (flag_i_r && !flag_i)
                {
                    output.Append("<i>");
                    flag_i = true;
                }
                if (flag_u_r && !flag_u)
                {
                    output.Append("<u>");
                    flag_u = true;
                }
            }

            output.Append(htmltext);
        }


        void htmlparser_comment(string htmltext)
        {
            htmltext = htmltext.ToUpper();

            if (htmltext.StartsWith("SA ") || htmltext.StartsWith("SB "))
            {
                string[] tokens = htmltext.Split(' ');
                int x = int.Parse(tokens[1]);
                myStruct["SAB"] = x;
                setparbr(x == 0 ? 1 : 0);
                return;
            }

            switch (htmltext)
            {
                case "CAPS":
                    flag_caps = true;
                    break;
                case "ENDCAPS":
                    flag_caps = false;
                    break;
                case "GROUP":
                    spush();
                    break;
                case "UNGROUP":
                    scmp();
                    spop();
                    break;

                case "PARD": // reset 
                    if (getBool(myStruct["QC"]) || getBool(myStruct["QR"]))
                        newpar(ptext + fontleft);

                    myStruct["QC"] = false;
                    myStruct["QR"] = false;

                    myStruct["SAB"] = 0;

                    setparbr(1);
                    break;

                case "PAR":
                    if (getBool(myStruct["QC"]))
                        newpar("<p align=\"center\">" + fontcenter);
                    else if (getBool(myStruct["QR"]))
                        newpar("<p align=\"right\">" + fontright);
                    else
                        newpar(ptext + fontleft);
                    break;

                case "QC":
                    if (!getBool(myStruct["QC"]))
                    {
                        newpar("<p align=\"center\">" + fontcenter);

                        myStruct["QC"] = true;
                        myStruct["QR"] = false;
                    }
                    break;

                case "QR":
                    if (!getBool(myStruct["QR"]))
                    {
                        newpar("<p align=\"right\">" + fontright);

                        myStruct["QC"] = false;
                        myStruct["QR"] = true;
                    }

                    break;

                case "QL":
                case "QJ":
                    if (getBool(myStruct["QC"]) || getBool(myStruct["QR"]))
                    {
                        newpar("ptext" + fontleft);

                        myStruct["QC"] = false;
                        myStruct["QR"] = false;
                    }

                    break;

                case "FOOTNOTE":
                    if (!getBool(myStruct["FOOTNOTE"]))
                    {
                        spush();
                        footnote_stack[STRUCT][footnote_count++] = myStruct;

                        myStruct["FOOTNOTE"] = true;
                        footnote_txt = output.ToString();
                        output.Length = 0;
                    }
                    break;

                case "CHFTN":
                    if (getBool(myStruct["FOOTNOTE"]))
                    {
                        string fname = "<a name=\"" + footnote_count + "\"><a href=\"#xx" + footnote_count + "\">" + footnotecolor1 + "[" + footnote_count + "]" + footnotecolor2 + "</a>";
                        output.Append(" " + fname + " ");
                    }
                    else
                    {
                        footnote_count++;

                        string ftarget = "<a name=\"xx" + footnote_count + "\">&nbsp;<a href=\"#" + footnote_count + "\">" + footnotecolor1 + "[" + footnote_count + "]" + footnotecolor2 + "</a>";
                        output.Append(" " + ftarget + " ");
                        footnote_count--;
                    }
                    break;

                default:
                    output.Append("<!--" + htmltext + "-->");
                    break;
            }
        }

        void htmlparser_command(string htmltext)
        {
            string htmltext0 = htmltext.ToLower();

            switch (htmltext0)
            {
                case "li":
                    flag_li = true;
                    output.Append("<li>");
                    break;

                case "/li":
                    flag_li = false;
                    output.Append("</li>");
                    break;

                case "ul":
                    flag_ul = true;
                    output.Append("<ul>");
                    break;

                case "/ul":
                    flag_ul = false;
                    output.Append("</ul>");
                    break;

                case "sup":
                    output.Append("<sup>");
                    myStruct["SUP"] = true;
                    break;

                case "/sup":
                    output.Append("</sup>");
                    myStruct["SUP"] = false;
                    break;


                case "/font":
                    output.Append("</font>");
                    flag_font = false;
                    flag_caps = false;
                    break;

                case "b":
                    if (!flag_b)
                    {
                        output.Append("<b>");
                        flag_b = true;
                        flag_b_r = true;
                    }
                    break;


                case "i":
                    if (!flag_i)
                    {
                        output.Append("<i>");
                        flag_i = true;
                        flag_i_r = true;
                    }
                    break;


                case "u":
                    if (!flag_u)
                    {
                        output.Append("<u>");
                        flag_u = true;
                        flag_u_r = true;
                    }
                    break;

                case "/b":
                    flag_b_r = false;
                    if (flag_b)
                    {
                        output.Append("</b>");
                        flag_b = false;
                        flag_b_r = false;
                    }
                    break;


                case "/i":
                    flag_i_r = false;
                    if (flag_i)
                    {
                        output.Append("</i>");
                        flag_i = false;
                        flag_i_r = false;
                    }
                    break;

                case "/u":
                    flag_u_r = false;
                    if (flag_u)
                    {
                        output.Append("</u>");
                        flag_u = false;
                        flag_u_r = false;
                    }
                    break;


                default:
                    if (special_footnotes != 0 && !special_footnotes_flag && !no_small_footnotes)
                    {
                        if (htmltext0.StartsWith("a href=\"#xx"))
                        {
                            special_footnotes_flag = true;
                            fontleft = fontfootnote;

                            output.Append("<" + htmltext + ">");
                            return;
                        }
                        break;
                    }

                    if (htmltext0.StartsWith("font"))
                    {
                        if (special_footnotes_flag)
                        {

                            output.Append(fontleft);
                            flag_font = true;
                            flag_caps = false;
                            return;
                        }

                        flag_font = true;
                        flag_caps = false;
                        output.Append("<" + htmltext + ">");
                        return;
                    }
                    else if (htmltext0.StartsWith("table"))
                    {
                        if (flag_table && table_special)
                            output.Append("<tr>");
                        else
                        {
                            if (flag_image)
                            {
                                output.Append("</b></i></u></td></table>");
                                if (flag_b)
                                    output.Append("<b>");
                                if (flag_i)
                                    output.Append("<i>");
                                if (flag_u)
                                    output.Append("<u>");
                                flag_image = false;
                            }

                            spush();
                            output.Append("<" + htmltext + ">");
                            flag_table = true;
                        }
                        return;
                    }
                    else if (htmltext0.StartsWith("/table"))
                    {
                        if (!table_special)
                        {
                            spop();
                            output.Append("<" + htmltext + ">");
                            flag_table = false;
                        }
                        else if (flag_table)
                            output.Append("<tr>");
                        return;
                    }
                    else if (htmltext0.StartsWith("td"))
                    {
                        if (flag_td)
                        {
                            if (flag_font)
                                output.Append("</font>");
                            output.Append("</td>");
                        }
                        flag_td = true;
                        flag_font = false;

                        flag_b = false;
                        flag_i = false;
                        flag_u = false;

                        output.Append("<" + htmltext + ">");
                        return;
                    }
                    else if (htmltext0.StartsWith("/td"))
                    {
                        if (flag_b)
                        {
                            output.Append("</b>");
                            flag_b = false;
                        }
                        if (flag_i)
                        {
                            output.Append("</i>");
                            flag_i = false;
                        }
                        if (flag_u)
                        {
                            output.Append("</u>");
                            flag_u = false;
                        }
                        if (flag_font)
                            output.Append("</font>");
                        flag_td = false;
                        flag_font = false;
                        output.Append("<" + htmltext + ">");
                        return;
                    }

                    output.Append("<" + htmltext + ">");
                    break;
            }
        }


        void htmlparser(string i)
        {
            int off = 0;
            int lng = i.Length;
            int maxx = lng + 1;

            int comm_pos = -1;
            int cmd_pos = -1;

            do
            {
                if (off >= maxx)
                    return;

                if (off > comm_pos)
                {
                    comm_pos = i.IndexOf("<!--", off);
                    if (comm_pos == -1)
                        comm_pos = maxx;
                }

                if (off > cmd_pos)
                {
                    cmd_pos = i.IndexOf("<", off);
                    if (cmd_pos == -1)
                        cmd_pos = maxx;
                }

                off++;
                if (off >= lng)
                    return;

                if ((cmd_pos > off) && (comm_pos > off))
                {
                    int end_text = Math.Min(cmd_pos, comm_pos);
                    string htmltext = i.Substring(off, end_text - off);
                    if (!htmltext.Equals(""))
                        htmlparser_text(htmltext);
                    off = end_text;
                }
                else if (comm_pos <= off)
                {
                    int end_text = i.IndexOf("-->", off);
                    if (end_text == -1)
                        end_text = maxx;
                    string htmltext = i.Substring(comm_pos + 4, end_text - comm_pos - 4);
                    htmltext = htmltext.Trim();

                    if (!htmltext.Equals(""))
                        htmlparser_comment(htmltext);
                    off = end_text + 2;
                }
                else if (cmd_pos <= off)
                {
                    int end_text = i.IndexOf(">", off);
                    if (end_text == -1)
                        end_text = maxx;

                    string htmltext = i.Substring(cmd_pos + 1, end_text - cmd_pos - 1);
                    htmltext = htmltext.Trim();
                    if (!htmltext.Equals(""))
                        htmlparser_command(htmltext);
                    off = end_text;
                }
            } while (true);

        }

        string correctHtml(string input)
        {
            string[] pattern = new string[] { @"(\[[ ]*\]|<!--NONE-->|<font[^>]*>\n</font>)", "((<b>[ ]?[^<]*)<b>)", "((<i>[ ]?[^<]*)<i>)", "((<u>[ ]?[^<]*)<u>)", "(<b>[ ]?(<[^>]*>)</b>)", "(<i>[ ]?(<[^>]*>)</i>)", "(<u>[ ]?(<[^>]*>)</u>)", "(<b>[ ]*</b>)", "(<i>[ ]*</i>)", "(<u>[ ]*</u>)", "(<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'></([uib])>)", "(<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'></([uib])>)", "(<i><font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>)", "(<b><font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>)", "(<u><font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>)", "(<i><font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>)", "(<b><font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>)", "(<u><font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>)", "(</font>[\n ]*<br><font[^>]*>)" };
            string[] replace = new string[] { "", "$2", "$2", "$2", "$2", "$2", "$2", "", "", "", "</$2><font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>", "</$2><font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>", "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'><i>", "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'><b>", "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'><u>", "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'><i>", "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'><b>", "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'><u>", "\n<br>" };
            for (int i = 0; i < pattern.Length; ++i)
            {
                input = Regex.Replace(input, pattern[i], replace[i]);
            }

            string outp = input;
            do
            {
                input = outp;
                outp = Regex.Replace(input,
                           "(<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>([\n ]*)<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>)",
                           "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>$2");
            } while (!input.Equals(outp));
            do
            {
                input = outp;
                outp = Regex.Replace(input,
                           "(<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>([\n ]*)<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>)",
                           "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>$2");
            } while (!input.Equals(outp));
            do
            {
                input = outp;
                outp = Regex.Replace(input,
                           "(<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>([\n ]*)<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>)",
                           "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>$2");
            } while (!input.Equals(outp));
            do
            {
                input = outp;
                outp = Regex.Replace(input,
                           "(<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='1'>([\n ]*)<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='1'>)",
                           "<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='1'>$2");
            } while (!input.Equals(outp));

            pattern = new string[] { "(</li></ul><br><ul><li>|</li></ul>\n<br>\n<ul><li>)" };
            replace = new string[] { "</li></ul>\n<ul><li>" };
            for (int i = 0; i < pattern.Length; ++i)
            {
                input = Regex.Replace(input, pattern[i], replace[i]);
            }
            outp = input;

            do
            {
                input = outp;
                outp = Regex.Replace(input,
                           "(([a-zA-Z0-9])<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'>([a-zA-Z0-9]))",
                           "$2$3");
            } while (!input.Equals(outp));
            do
            {
                input = outp;
                outp = Regex.Replace(input,
                           "(([a-zA-Z0-9])<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='3'>([a-zA-Z0-9]))",
                           "$2$3");
            } while (!input.Equals(outp));
            do
            {
                input = outp;
                outp = Regex.Replace(input,
                           "(([a-zA-Z0-9])<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='1'>([a-zA-Z0-9]))",
                           "$2$3");
            } while (!input.Equals(outp));

            pattern = new string[] { "(<font face='Verdana, Helvetica CE, Arial CE, Helvetica, Arial' size='2'></font>)", "(<font)", "(<p>)", "(<br>)", "(<a>)", "(</a>)", "(<!--SPACE-->)" };
            replace = new string[] { "", "</font><font", "<p>\r\n", "<br>\r\n", "\r\n<a>", "</a>\r\n", " " };
            for (int i = 0; i < pattern.Length; ++i)
            {
                input = Regex.Replace(input, pattern[i], replace[i]);
            }

            return input;
        }

    }
}