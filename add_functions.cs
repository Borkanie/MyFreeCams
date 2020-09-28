using System;
using System.Collections.Generic;
using System.Threading;
using Traffic_Booster.Guest_2._0;
using Traffic_Booster.Member_2._0;

namespace Traffic_Booster
{
    //Singletron
    //aici creem threaduri pentru guiesti si membrii atat noi cat si vechi
    class add_functions
    {
        #region methods
        //Generaza un mebru cu browser
        //chemam constructorul pentru membrii
        static public void addmember(int n, string[] links, string prx, Consola_Form Consola, MainMFC main)
        {
            for(int i=0;i< links.Length;i++)
            {
                if(links[i].Contains(":"))
                {
                    links[i] = links[i].Split(':')[0] + "/";
                }
            }
            Consola.WriteLine("[" + DateTime.Now + "] " + "Generating Member Browser #" + n.ToString());
            var chk = main.GetMemberVisibleCheck();
            var referer = main.GetReferer();
            Random rand = new Random();
            int rand2 = rand.Next(main.GetMemberListBox().Count);
            string mk = main.GetMemberListBox()[n].ToString();
            string[] data = mk.Split('+');
            int num = 2;
            num = main.GetRandomNumber(0, main.GetUserAgents().Length);

            Members baiatu = new Members(prx, chk, main.GetUserAgents()[num], links);
            try
            {
                baiatu.Login_on_site(data[0], data[1], referer);
                if (baiatu.is_on() == true)
                {
                    if (main.GetLines().Length > 1)
                    {
                        Thread dummythread = new Thread(() => baiatu.Write_on_chat(main.GetLines()));
                        main.AddThreadToList(dummythread);
                        dummythread.Start();
                    }
                    main.jasmin_online_members();
                    main.AddMemberToList(baiatu);
                    baiatu.Navigate(links);
                    if (main.GetMemberCount() <= Convert.ToInt32(main.GetMemberTextBox()))
                    {
                        main.IncreaseFirstMember();
                    }
                    Consola.WriteLine("[" + DateTime.Now + "]" + "Member number #" + n.ToString() + " is on");
                }
                else
                {
                    baiatu.Kill();
                    main.RemoveThread(Thread.CurrentThread);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Consola.WriteLine("[" + DateTime.Now + "] " + s);
            }
            main.RemoveThread(Thread.CurrentThread);
        }
        //generam un guest cu browser
        //chemam constructorul pentru guestii
        static public void addguest(string[] links, int nr, Consola_Form Consola, MainMFC main)
        {
            for (int i = 0; i < links.Length; i++)
            {
                if (links[i].Contains(":"))
                {
                    links[i] = links[i].Split(':')[0];
                }
            }
            Random rand = new Random();
            int random = rand.Next(main.GetGuetsListBoxCount());
            string prx = main.GetGuetsProxyListBox()[random].ToString();
            Guest baiatu = new Guest(prx, main.GetGuestVisible());
            try
            {
                baiatu.StartMultipleTabs(links);
                Thread.Sleep(3000);
                if (baiatu.is_on() == true)
                {
                    main.jasmin_online_guests();
                    main.AddGuestToList(baiatu);
                    Consola.WriteLine("[" + DateTime.Now + "]" + "Guest number #" + nr.ToString() + " is on");
                }
                else
                    baiatu.Kill();
                main.RemoveThread(Thread.CurrentThread);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Consola.WriteLine("[" + DateTime.Now + "] " + s);
                baiatu.Kill();
            }

        }
        //deschid trheaduri pentru addmember
        //aici deschidem membrii
        static private void LoadMembers(Consola_Form Consola, MainMFC main)
        {
           
                for (int i = 0; i < main.GetMemberListBox().Count; i++)
                {
                    try
                    {
                        //verifica daca are deja un proxy in listbox
                        string prx;
                        //daca are il ia de acolo
                        //asta e dilau si nu mere 
                        string[] data1 = main.GetMemberListBox()[i].ToString().Split('+');
                        prx = data1[2];
                        Thread t1 = new Thread(() => addmember(i, main.GetProfiles(), prx, Consola, main));
                        t1.Start();
                        main.AddThreadToList(t1);
                        /// TIMPUL DE LOGARE INTRE MEBRII
                        Thread.Sleep(10000);
                        //aici putem pune c etimp vrem continui mia tarziu
                    }
                    catch (Exception ex)
                    {
                        string s = ex.Message;
                        Consola.WriteLine("[" + DateTime.Now + "] " + s);
                    }
                }
            //main.RemoveThread(Thread.CurrentThread); //sa nu ramana parazita
        }
        //de aici incepam sa generam tot
        //aici incepem sa facem threadurile pentru deschiderea browserelor
        static public void jasmin_Launch(Consola_Form Consola, MainMFC main)
        {
            //
            try
            {
                if (main.GetMemberTextBox() != "0")
                {
                    if (main.GetMemberV2Checkbox())
                        LoadNewMembers(Consola, main);
                    else
                        LoadMembers(Consola, main);

                }
                if(main.GetGuestTextBox()!="0")
                {
                    if(main.GetGuestWebSocketCheckBox())
                    {
                        main.generator = new generateSocketCodes();
                        main.generator.generateGuests(main, Consola);
                    }
                    else
                    {
                        LoadOldGuests(Consola, main);
                    }
                }
                
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Consola.WriteLine("[" + DateTime.Now + "] " + s);
            }

            main.jasmin_hide();
            if (main.GetJasminNr() != 0)
                main.jasmin_timer_start();
        }
        //generam membrii noi
        //inca nu merge
        private static void LoadNewMembers(Consola_Form consola, MainMFC main)
        {
            List<Model> models = new List<Model>();
            for (int i = 0; i < main.GetProfiles().Length; i++)
            {
                if (main.GetProfiles()[i].Split('/')[0].Split(':').Length == 2)
                    models.Add(new Model() { Name = main.GetProfiles()[i].Split('/')[0].Split(':')[0], Code = Convert.ToInt32(main.GetProfiles()[i].Split('/')[0].Split(':')[1]), Number = i });
                else
                    consola.WriteLine(main.GetProfiles()[i].Split('/')[0] + " doesn't have a code");
            }
            for (int i = 0; i < main.GetMemberListBox().Count; i++)
            {
                try
                {
                    //verifica daca are deja un proxy in listbox
                    string prx;
                    //daca are il ia de acolo
                    //asta e dilau si nu mere 
                    string[] data1 = main.GetMemberListBox()[i].ToString().Split('+');
                    prx = data1[2];
                  
                    MfcMember member = new MfcMember(models,new WebSharerProxy(prx),consola,new MemberData(main.GetMemberListBox()[i].ToString()));
                    //Thread t1 = new Thread(() => addmember(i, , prx, consola, main));
                    //t1.Start();
                    //main.AddThreadToList(t1);
                    Thread.Sleep(10000);
                    //aici putem pune c etimp vrem continui mia tarziu
                }
                catch (Exception ex)
                {
                    consola.WriteLine("[" + DateTime.Now + "] " + ex.Message);
                }
            }
           
        }
        //geenram membrii vechi
        //aici facem Threadurile pentru fiecare
        public static void LoadOldGuests(Consola_Form Consola, MainMFC main)
        {
            int nr = main.GetMemberCount() / main.GetProfiles().Length;
            Thread.Sleep(3000);
            for (int i = 0; i < Convert.ToInt32(main.GetGuestTextBox()); i++)
            {
                try
                {
                    Consola.WriteLine("[" + DateTime.Now + "] " + "Generating Member Guest #" + i.ToString());
                    if (i % 15 == 0 && i != 0)
                    {


                        for (int j = 0; j < 60; j++)
                        {
                            Thread.Sleep(3000);
                            if (main.GetONlineGuests() + Guest.get_nr_failed() == i)
                                break;

                        }
                        //Thread.Sleep(30000);
                    }
                    Thread t1 = new Thread(() => addguest(main.GetProfiles(), i, Consola, main));
                    t1.Start();
                    main.AddThreadToList(t1);
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    Consola.WriteLine("[" + DateTime.Now + "] " + s);
                }
            }
            while (main.GetMemberCount() < main.GetMemberListBox().Count * 9 / 10 || main.GetGuestsCount() < Convert.ToInt32(main.GetGuestTextBox()) * 9 / 10)
            {
                Thread.Sleep(5000);
            }
        }
        #endregion
    }
}
