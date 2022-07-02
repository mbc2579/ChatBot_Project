using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           //Add to process Async Task
using Microsoft.Bot.Connector;          //Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    //Add for Dialog Class
using System.Net.Http;                  //Add for internet
using GreatWall.Helpers;                //Add for CardHelper
using MySql.Data.MySqlClient;

namespace GreatWall
{
    [Serializable]
    public class OrderDialog : IDialog<string>
    {
        int a=0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0, i = 0, j = 0, k = 0, l = 0;
        string strMessage;
        string strOrder;
        string strServerUrl = "http://localhost:3984/Images/";
        String[] place = new String[50], location = new String[50], picture = new String[50], naver_url = new String[50], naver_hotel = new String[50]
            , how_going = new String[50], introduce = new String[50], naver_eat = new String[50];
        int Sea, Valley, Mountain, Camping, Cave, Ski, Park, Bunji, Fishing, Spa, River, Flower;
        String wantplace;
        String[] splitplace;
        List<string> ch = new List<string>();

        
        public void sql(int Sea, int Valley, int Mountain, int Camping, int Cave, int Ski, int Park, int Bunji, int Fishing, int Spa, int River, int Flower) // sql
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=travel;Uid=root;Pwd=inha1958"))
            {
                try//예외 처리
                {
                    String sql2="";
                    if(Sea == 1)
                    {
                        sql2 = " or Sea = 1";
                    }
                    if (Valley == 1)
                    {
                        sql2 += " or Valley = 1";
                    }

                    if (Camping == 1)
                    {
                        sql2 += " or Camping = 1";
                    }

                    if (Cave == 1)
                    {
                        sql2 += " or Cave = 1";
                    }

                    if (Ski == 1)
                    {
                        sql2 += " or Ski = 1";
                    }

                    if (Mountain == 1)
                    {
                        sql2 += " or Mountain = 1";
                    }

                    if (Park == 1)
                    {
                        sql2 += " or Park = 1";
                    }

                    if (Bunji == 1)
                    {
                        sql2 += " or Bunji = 1";
                    }

                    if (Fishing == 1)
                    {
                        sql2 += " or Fishing = 1";
                    }

                    if (Spa == 1)
                    {
                        sql2 += " or Spa = 1";
                    }

                    if (River == 1)
                    {
                        sql2 += " or River = 1";
                    }

                    if (Flower == 1)
                    {
                        sql2 += " or Flower = 1";
                    }

                    String sql3 = sql2.Substring(4);

                    connection.Open();
                    string sql = "SELECT place, location, picture, naver_url, naver_hotel, how_going, indroduce, naver_eat  FROM tra_place where " + sql3;

                    //ExecuteReader를 이용하여
                    //연결 모드로 데이타 가져오기
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader table = cmd.ExecuteReader();
                    int idx = 0;
                    while (table.Read())
                    {
                        Console.WriteLine(table["location"]);
                        place[idx] = table["place"].ToString();
                        location[idx] = table["location"].ToString();
                        picture[idx] = table["picture"].ToString();
                        naver_url[idx] = table["naver_url"].ToString();
                        naver_hotel[idx] = table["naver_hotel"].ToString();
                        how_going[idx] = table["how_going"].ToString();
                        introduce[idx] = table["indroduce"].ToString();
                        naver_eat[idx] = table["naver_eat"].ToString();
                        idx++;
                    }
                    table.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("실패");
                    Console.WriteLine(ex.ToString());
                }

            }
        }


        public async Task StartAsync(IDialogContext context) //시작
        {
            strMessage = null;
            strOrder = "지금까지 선택하신 여행지 : ";


            //Called MessageReceivedAsync() without user input message
            await this.MessageReceivedAsync(context, null);
        }
        
        

        private async Task MessageReceivedAsync(IDialogContext context, //메시지띄우는거
                                               IAwaitable<object> result)
        {
            var travel = context.MakeMessage(); //메시지만들기
            if (result != null)
            {

                Activity activity = await result as Activity;
                for (int i = 0; i < place.Length; i++)
                {
                    if (place[i] == "") break;
                    if (activity.Text.Trim() == i.ToString())
                    {
                        ch.Add(place[i]);
                        var top = context.MakeMessage();

                        top.Attachments.Add(CardHelper.GetHeroCard(place[i], location[i],
                                            picture[i], "", ""));

                        top.Attachments.Add(new HeroCard { Text = introduce[i] }.ToAttachment());
                        top.Attachments.Add(new HeroCard { Title = "네이버에 검색", Subtitle = naver_url[i] }.ToAttachment());
                        top.Attachments.Add(new HeroCard { Title = "대중교통 이용하여 가는법", Subtitle = how_going[i] }.ToAttachment());
                        top.Attachments.Add(new HeroCard { Title = "숙소", Subtitle = naver_hotel[i] }.ToAttachment());
                        top.Attachments.Add(new HeroCard { Title = "근처 식당", Subtitle = naver_eat[i] }.ToAttachment());

                        /*                        top.Attachments.Add(CardHelper.GetHeroCard("네이버에 검색", naver_url[i],
                                                                    "", "", ""));
                                                top.Attachments.Add(CardHelper.GetHeroCard("대중교통 이용하여 가는법", how_going[i],
                                                                    "", "", ""));
                                                top.Attachments.Add(CardHelper.GetHeroCard("숙소", naver_hotel[i],
                                                                    "", "", ""));
                                                top.Attachments.Add(CardHelper.GetHeroCard("근처 식당", naver_eat[i],
                                                                    "", "", ""));*/

                        top.AttachmentLayout = "list";              //Setting Menu Layout Format

                        await context.PostAsync(top);

                        //Output message 

                        /*place[i] = null;
                        naver_url[i] = null;
                        how_going[i] = null;
                        naver_hotel[i] = null;
                        naver_eat[i] = null;
                        string temp = null;
                        string temp1 = null;
                        string temp2 = null;
                        string temp3 = null;
                        string temp4 = null;*/
                        /*for (int n = 0; n < place.Length; n ++)
                        {
                            if (place[n] == null)
                            {
                                temp = place[n + 1];
                                place[n] = place[n + 1];
                                place[n + 1] = null;
                                temp1 = naver_url[n + 1];
                                naver_url[n] = naver_url[n + 1];
                                naver_url[n + 1] = null;
                                temp2 = how_going[n + 1];
                                how_going[n] = how_going[n + 1];
                                how_going[n + 1] = null;
                                temp3 = naver_hotel[n + 1];
                                naver_hotel[n] = naver_hotel[n + 1];
                                naver_hotel[n + 1] = null;
                                temp4 = naver_eat[n + 1];
                                naver_eat[n] = naver_eat[n + 1];
                                naver_eat[n + 1] = null;

                            }
                        }*/

                        //int temp2 = 0;
                        for (int j = 0; j < place.Length; j++)
                        {
                            int temp = 0;
                            /*if (temp2 == 0) {
                                await context.PostAsync("모든 여행지를 구경했습니다");
                            }*/
                            if (place[j] == null) break;
                            for (int k = 0; k < ch.Count; k++)
                            {
                                if (place[j] == ch[k])
                                {
                                    temp = 1;
                                    break;
                                }
                            }
                            if (temp == 1) continue;
                            //temp2 = 1;
                            travel.Attachments.Add(CardHelper.GetHeroCard(place[j], location[j],
                                            picture[j], place[j], j.ToString()));
                        }
                        travel.AttachmentLayout = "carousel";              //Setting Menu Layout Format
                        await context.PostAsync(travel);                   //Output message 
                        //context.Wait(this.MessageReceivedAsync);
                    }
                }
                if (activity.Text.Trim() == "선택완료")
                {
                    await context.PostAsync(strOrder);    //return our reply to the user
                    strOrder = null;
                    splitplace = wantplace.Split(' ');
                    for (int m = 0; m < splitplace.Length; m++) //맨처음에 선택한곳 받아서 sql에 던지기
                    {
                        if (splitplace[m] == "바다")
                        {
                            a = 1;
                        }
                        if (splitplace[m] == "계곡")
                        {
                            b = 1;
                        }
                        if (splitplace[m] == "산")
                        {
                            c = 1;
                        }
                        if (splitplace[m] == "캠핑")
                        {
                            d = 1;
                        }
                        if (splitplace[m] == "동굴")
                        {
                            e = 1;
                        }
                        if (splitplace[m] == "스키")
                        {
                            f = 1;
                        }
                        if (splitplace[m] == "공원")
                        {
                            g = 1;
                        }
                        if (splitplace[m] == "번지점프")
                        {
                            h = 1;
                        }
                        if (splitplace[m] == "낚시")
                        {
                            i = 1;
                        }
                        if (splitplace[m] == "온천")
                        {
                            j = 1;
                        }
                        if (splitplace[m] == "강")
                        {
                            k = 1;
                        }
                        if (splitplace[m] == "꽃구경")
                        {
                            l = 1;
                        }
                        
                    }
                    sql(a, b, c, d, e, f, g, h, i, j, k, l);
                    // --------------------------------------------------------------------------------------
                    for (int i = 0; i < place.Length; i++)
                    {

                        if (place[i] == null) // 동검도 누르면 0 나오는부분
                        {
                            break;
                        }
                        travel.Attachments.Add(CardHelper.GetHeroCard(place[i], location[i],
                                        picture[i], place[i], i.ToString()));
                    }
                    travel.AttachmentLayout = "carousel";              //Setting Menu Layout Format

                    await context.PostAsync(travel);                   //Output message 
                    //context.Wait(this.MessageReceivedAsync);

                }
                
                else //Exit 가 아니라 바다 계곡 등 계속 선택하면
                {
                    strMessage = string.Format("{0} 를(을)선택하셨습니다. 그만 선택하실려면 선택완료를 입력해주세요.", activity.Text);
                    wantplace += activity.Text + " ";
                    strOrder += activity.Text + "\n";
                    await context.PostAsync(strMessage);    //return our reply to the user
                    context.Wait(this.MessageReceivedAsync);

                }
                
            }
            else //바다 계곡 등 선택
            {
                strMessage = "가고싶은 여행지를 선택하세요";
                await context.PostAsync(strMessage);    //return our reply to the user

                //Menu
                var message = context.MakeMessage();                 //Create message      

                //Hero Card-01~04 attachment 

                message.Attachments.Add(CardHelper.GetHeroCard("바다", "Sea", 
                                        this.strServerUrl + "Sea.jpg", "바다", "바다"));

                message.Attachments.Add(CardHelper.GetHeroCard("계곡", "Valley",
                                        this.strServerUrl + "Valley.jpg", "계곡", "계곡"));

                message.Attachments.Add(CardHelper.GetHeroCard("산", "Mountain",
                                        this.strServerUrl + "Mountain.jpg", "산", "산"));

                message.Attachments.Add(CardHelper.GetHeroCard("캠핑", "Camping",
                                        this.strServerUrl + "Camping.jpg", "캠핑", "캠핑"));

                message.Attachments.Add(CardHelper.GetHeroCard("동굴", "Cave",
                                        this.strServerUrl + "Cave.jpg", "동굴", "동굴"));

                message.Attachments.Add(CardHelper.GetHeroCard("스키", "Ski",
                                        this.strServerUrl + "Ski.jpg", "스키", "스키"));

                message.Attachments.Add(CardHelper.GetHeroCard("공원", "Park",
                                        this.strServerUrl + "Park.jpg", "공원", "공원"));

                message.Attachments.Add(CardHelper.GetHeroCard("번지점프", "Bunji",
                                        this.strServerUrl + "Bunji.jpg", "번지점프", "번지점프"));

                message.Attachments.Add(CardHelper.GetHeroCard("낚시", "Fishing",
                                        this.strServerUrl + "Fishing.jpg", "낚시", "낚시"));

                message.Attachments.Add(CardHelper.GetHeroCard("온천", "Valley",
                                        this.strServerUrl + "Valley.jpg", "온천", "온천"));

                message.Attachments.Add(CardHelper.GetHeroCard("강", "River",
                                        this.strServerUrl + "River.jpg", "강", "강"));

                message.Attachments.Add(CardHelper.GetHeroCard("꽃구경", "Flower",
                                        this.strServerUrl + "Flower.jpg", "꽃구경", "꽃구경"));

                message.Attachments.Add(CardHelper.GetHeroCard("선택완료", "선택완료",
                                        null, "선택완료", "선택완료"));

                message.AttachmentLayout = "carousel";              //Setting Menu Layout Format

                await context.PostAsync(message);                   //Output message 

                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}