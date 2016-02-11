using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace CsGoComp
{
    public partial class 
        Form1 : Form
    {
        private HttpListener listener;
        private String feedString = "";
    
        TaskScheduler scheduler = TaskScheduler.Current;
        delegate void SetTextCallback(string text,Label x);
        private JObject json;
        //private JObject xmljson;
        private int[] losses;
        private int bonus = 0;
        private int lbon = 1400;
        private String to = "none";
        private bool android = false;
        private bool stop = false;

        Color flash = ColorTranslator.FromHtml("#f5f5f5");
        Color unflash = ColorTranslator.FromHtml("#616161");

        Color smoke = ColorTranslator.FromHtml("#212121");
        Color unsmoke = ColorTranslator.FromHtml("#949494");

        Color burn = ColorTranslator.FromHtml("#bf360c");
        Color unburn = ColorTranslator.FromHtml("#3e2723");

        Color full = ColorTranslator.FromHtml("#2e7d32");
        Color medium = ColorTranslator.FromHtml("#ffc400");
        Color low = ColorTranslator.FromHtml("#b71c1c");
        Color dead = ColorTranslator.FromHtml("#2d2d2d");

        Color freezetime = ColorTranslator.FromHtml("#ffc400");
        Color live = ColorTranslator.FromHtml("#2e7d32");
        Color over = ColorTranslator.FromHtml("#b71c1c");
        Color warmup = ColorTranslator.FromHtml("#64b5f6");
        String str;
        int overflowwin = 0;
        int overflowloss = 0;

        private bool unsaved = true;

        public Form1()
        {
            /*updateTimer = new System.Timers.Timer();
            updateTimer.Interval = 500;
            updateTimer.Elapsed += OnTimedEvent;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;*/

            InitializeComponent();
            CSListenerInitializer();
            losses = new int[100];
            
           



        }

       

        public void CSListenerInitializer()
        {
            String prefix = "http://127.0.0.1:3000/";

            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
           
            listener = new HttpListener();
            // Add the prefixes.
            
                listener.Prefixes.Add(prefix);
            
           
           
        }

        private void loop()
        {

            if (stop)
            {
                return;
            }
                IAsyncResult result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);


                result.AsyncWaitHandle.WaitOne();

                Console.WriteLine("Request processed asyncronously.");



            loop();
            //Console.WriteLine(request.ContentLength64);
            
          

            
        }
        public void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            // Call EndGetContext to complete the asynchronous operation.
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            
            //Console.WriteLine(request.ContentLength64);
            // Construct a response.
            /* string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
             byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
             // Get a response stream and write the response to it.
             response.ContentLength64 = buffer.Length;
             System.IO.Stream output = response.OutputStream;
             output.Write(buffer, 0, buffer.Length);
             // You must close the output stream.
             output.Close();*/
            Stream receiveStream = request.InputStream;
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
           
            feedString = readStream.ReadToEnd();
            response.Close();
            Console.WriteLine(feedString);
            str = feedString;
            
           parse_Json(feedString);
          /*  if (!to.Equals("none") && android)
            {
                sendToAndroid(feedString);
            }
            //feed.Text = feedString;
            //SetText(feedString);*/



        }

        private void SetText(String text, Label x)
        {
            if (x.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, x });
            }
            else
            {
                x.Text = text;
            }

        }

       

       


        private void parse_Json(String str)
        {
            json = JObject.Parse(str);
            Task upd = new Task(update);
            upd.Start();
            //update();
        }


        private void update()
        {
            //Set standard values 
           // Console.WriteLine("Test" + json.ToString());
            String name = "Name";
            String ctscore = "0";
            String tscore = "0";
            String mapname = "map";
            String gamemode = "gamemode";
            String teamstring = "team";
            String killstring = "0";
            String deathstring = "0";
            String assiststring = "0";
            String scorestring = "0";
            String kdstring = "0";
            int flashed = 0;
            int smoked = 0;
            int burning = 0;
            int health = 100;
            String roundkills = "0";
            String roundkillhs = "0";
            String phase = "";
            String winner = "";
            int roundnr = 0;
            int money = 0;
            int rounds = 0;
            String mvps = "0";
            
            



            JObject provider = (JObject)json.GetValue("provider");
            JObject player = (JObject)json.GetValue("player");
            JObject matchstats = (JObject)player.GetValue("match_stats");
            JObject playerstate = (JObject)player.GetValue("state");
            JObject map = (JObject)json.GetValue("map");
            JObject round = (JObject)json.GetValue("round");
            JObject ct = null;
            JObject t = null;

            String providerid = (String)provider.GetValue("steamid");
            String playerid = (String)player.GetValue("steamid");
            name = (String)player.GetValue("name");
          
                if(map !=null)
                {
                    ct = (JObject)map.GetValue("team_ct");
                    t = (JObject)map.GetValue("team_t");
                    ctscore = (String)ct.GetValue("score");
                    tscore = (String)t.GetValue("score");
                    mapname = (String)map.GetValue("name");
                    gamemode = (String)map.GetValue("mode");
                    roundnr = (int)map.GetValue("round");
                    phase = (String)map.GetValue("phase");
                }
                teamstring = (String)player.GetValue("team");
               
                if(matchstats != null)
                {
                    killstring = (String)matchstats.GetValue("kills");
                    deathstring = (String)matchstats.GetValue("deaths");
                    assiststring = (String)matchstats.GetValue("assists");
                    scorestring = (String)matchstats.GetValue("score");
                    mvps = (String)matchstats.GetValue("mvps");
                double k = (double)matchstats.GetValue("kills");
                    double d = (double)matchstats.GetValue("deaths");
                    
                    double kd = k / d;
                     kdstring = kd.ToString();



            }
                if(playerstate != null)
                {
                    flashed = (int)playerstate.GetValue("flashed");
                    smoked = (int)playerstate.GetValue("smoked");
                    burning = (int)playerstate.GetValue("burning");
                    health = (int)playerstate.GetValue("health");
                    roundkills = (String)playerstate.GetValue("round_kills");
                    roundkillhs = (String)playerstate.GetValue("round_killhs");
                    money = (int)playerstate.GetValue("money");
                    
                }
            if (round != null)
            {
                Console.WriteLine(round.ToString());
                if (!phase.Equals("warmup") && !phase.Equals("gameover"))
                {
                    phase = (String)round.GetValue("phase");
                }
                if (phase.Equals("over"))
                {
                    winner = (String)round.GetValue("win_team");
                    losscalc(winner, teamstring, roundnr);

                }
            }



            if (gamemode.Equals("competitive"))
            {
                rounds = 30 - roundnr;
            }
            else if (gamemode.Equals("casual"))
            {
                rounds = 15 - roundnr;
            }
            else
            {
                rounds = 1;
            }



                SetText(ctscore, ctsc);
                SetText(tscore, tsc);
                SetText(rounds.ToString(), roundsleft);
                SetText(providerid, steamid);
            to = providerid;

            

            if (providerid.Equals(playerid))
                {
                    
                    //Set labels
                    SetText(name, nick);
                    SetText(mapname, maplabel);
                    SetText(gamemode, gametypelabel);
                    SetText(killstring, killscore);
                    SetText(deathstring, deathscore);
                    SetText(assiststring, assistscore);
                    SetText(scorestring, score);
                    SetText(roundkills, kills);
                    SetText(roundkillhs, headshots);
                    SetText(teamstring, team);
                    
                    SetText("$" + lbon.ToString(), lossbonuslabel);
                    SetText(kdstring, kd);
                
                    SetText(winmoneycalc(money,gamemode), moneywin);
                    SetText(lossmoneycalc(money,gamemode), moneyloss);

                    SetText("$" + overflowwin.ToString() + " over cap", overflowlabelwin);
                    SetText("$" + overflowloss.ToString() + " over cap", overflowlabelloss);
                SetText(mvps, mvp);




                if (flashed > 0)
                    {
                        flashedpanel.BackColor = flash;
                    }
                    else
                    {
                        flashedpanel.BackColor = unflash;
                    }

                    if(smoked > 0)
                    {
                        smokedpanel.BackColor = smoke;
                    }
                    else
                    {
                        smokedpanel.BackColor = unsmoke;
                    }

                    if (burning > 0)
                    {
                        burningpanel.BackColor = burn;
                    }
                    else
                    {
                        burningpanel.BackColor = unburn;
                    }

                    if(health >= 75)
                    {
                        healthpanel.BackColor = full;
                    }
                    else if(health >= 25)
                    {
                        healthpanel.BackColor = medium;
                    }
                    else if(health > 0)
                    {
                        healthpanel.BackColor = low;
                    }
                    else
                    {
                        healthpanel.BackColor = dead;
                    }

               


                

            }
            if (phase.Equals("live"))
            {
                roundpanel.BackColor = live;
            }
            else if (phase.Equals("freezetime"))
            {
                roundpanel.BackColor = freezetime;
            }
            else if (phase.Equals("over") || phase.Equals("gameover"))
            {
                roundpanel.BackColor = over;
            }
            else if (phase.Equals("warmup"))
            {
                roundpanel.BackColor = warmup;
            }

            if (gamemode.Equals("competitive"))
            {
                if (phase.Equals("gameover"))
                {
                    if (unsaved)
                    {
                        saveComptetitiveToXml();
                        unsaved = false;
                    }
                }
                else
                {
                    unsaved = true;
                }
            }
        }

        private String winmoneycalc(int money, string gamemode)
        {
            if (gamemode.Equals("competitive"))
            {
                int ret = money + 3250;
                overflowwin = 16000 - ret;
                ret = Math.Min(ret, 16000);
               
                if(overflowwin > 0)
                {
                    overflowwin = 0;
                }
                else
                {
                    overflowwin = -overflowwin;
                }
                return "$" + ret.ToString();
            }
            else if (gamemode.Equals("casual")){
                int ret = money + 2700;
                overflowwin = 10000 - ret;
                ret = Math.Min(ret, 10000);
               
                if (overflowwin > 0)
                {
                    overflowwin = 0;
                }
                else
                {
                    overflowwin = -overflowwin;
                }
                return "$" + ret.ToString();
            }
            else
            {
                return "$0";
            }
        }
        private String lossmoneycalc(int money, string gamemode)
        {
            if (gamemode.Equals("competitive"))
            {
                int ret = money + lbon;
                overflowloss = 16000 - ret;
                ret = Math.Min(ret, 16000);
              
                if (overflowloss > 0)
                {
                    overflowloss = 0;
                }
                else
                {
                    overflowloss = -overflowloss;
                }
                return "$" + ret.ToString();
            }
            else if (gamemode.Equals("casual"))
            {
                int ret = money + 2400;
                overflowloss = 10000 - ret;
                ret = Math.Min(ret, 10000);
                
                if (overflowloss > 0)
                {
                    overflowloss = 0;
                }
                else
                {
                    overflowloss = -overflowloss;
                }
                lbon = 2400;
                return "$" + ret.ToString();
            }
            else
            {
                return "$0";
            }

            
        }

        private void losscalc(String winner, String team, int round)
        {
            if (winner.Equals(team))
            {
                losses[round] = 0;
            }
            else
            {
                losses[round] = 1;
            }
            int tmp = 0;
            if (round != 15 && round <= 30)
            {
                for (int i = round; i >= 0; i--)
                {
                    if (losses[i] == 0)
                    {
                        break;
                    }
                    else
                    {
                        tmp++;
                    }
                }
            }
            bonus = tmp;

            switch (bonus)
            {
                case (0):
                    lbon = 1400;
                    break;
                case (1):
                    lbon = 1900;
                    break;
                case (2):
                    lbon = 2400;
                    break;
                case (3):
                    lbon = 2900;
                    break;
                default:
                    lbon = 3400;
                    break;
                    
            }
        }

        

        private void saveComptetitiveToXml()
        {
            String kills = killscore.Text;
            String deaths = deathscore.Text;
            String mvps = mvp.Text;

            String ct = ctsc.Text;
            String t = tsc.Text;

            String map = maplabel.Text;
            String team = this.team.Text;
            XDocument doc = XDocument.Load("C:\\Users\\Johan\\Desktop\\StateTrak\\competitive.xml");

            /* XDocument doc = new XDocument(new XElement("game",
                 new XElement("kills", kills),
                 new XElement("deaths", deaths),
                 new XElement("mvps", mvps),
                 new XElement("ct", ct),
                 new XElement("t", t),
                 new XElement("map", map)
                 ));
                 */
            var x = doc.Descendants().First();
            x.Add(new XElement("game",
            new XElement("kills", kills),
            new XElement("deaths", deaths),
            new XElement("mvps", mvps),
            new XElement("ct", ct),
            new XElement("t", t),
            new XElement("team", team),
            new XElement("map", map)));

            doc.Save("C:\\Users\\Johan\\Desktop\\StateTrak\\competitive.xml");

            

        }
        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listener.Start();
            Console.WriteLine("Listening...");
            stop = false;
            //feedtext.Text = "Test";


            Task http = new Task(loop);
            http.Start();

            startServerToolStripMenuItem.Enabled = false;

            String startingdir = Path.GetDirectoryName(Application.ExecutablePath);
            Console.WriteLine("String: " + startingdir);



        }
        
        private void sendToAndroidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            android = true;
            sendToAndroidToolStripMenuItem.Enabled = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void saveToXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*String test = "{ root : { " + json.ToString() + " } }";
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(test);
            Console.WriteLine(doc.ToString());*/
            Console.WriteLine("Saving to xml from debug..");
            saveComptetitiveToXml();

        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }
    }
}
