
#define MYTEST


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLibrary_Product_Dishes;


//Тема: Протокол TCP.
//Задание №1
//Создайте серверное приложение с помощью, которого можно узнавать
//кулинарные рецепты. Типичный пример работы:
// клиентское приложение подключается к серверу;
// клиентское приложение посылает запрос с указанием списка
//продуктов;
// сервер возвращает рецепты, содержащие указанные
// продукты;
//клиент может послать новый запрос или отключиться.
//Одновременно к серверу может быть подключено большое количество
//клиентов. Используйте UDP сокеты для решения этой задачи.
namespace WF_ServerListeberSendAnswerRecipeDish
{

 public class Server
    {
       
        public void Listen(int localPort,Socket listeningSocket)
        {
            try
            {
                //Прослушка по адресу
                IPEndPoint localIP = new IPEndPoint(IPAddress.Loopback, localPort);
                listeningSocket.Bind(localIP);

                while (true)
                {
                    StringBuilder builder = new StringBuilder();//для получения сообщения
                    int bytes = 0;//количество полученных байтов
                    byte[] data = new byte[256];//буфер для входящих данных

                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0); //адрес, с которого пришли данные

                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                    } while (listeningSocket.Available>0);

                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;//получаем данные о подключении


                }



            }
            catch (Exception)
            {

                throw;
            }
        }


      


    }
    


    public partial class ServerUDP : Form
    {

        Dish dish;
       // List<Product> ProductsFromClient;
        int port;
        public ServerUDP()
        {
            InitializeComponent();
            port = 11000;
        }

        private void ServerTCP_Load(object sender, EventArgs e)
        {
            dish = new Dish();
            //Загружаем обьекты в классе dish
            dish.makeMenu();


           
        }
        /// <summary>
        /// Метод добавления текстра при textbox.BeginInvoke
        /// </summary>
        /// <param name="str"></param>
        public void AddText(string str)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(str);
            textBox1.Text = builder.ToString();
        }

       


        private  void buttonStartServer_Click(object sender, EventArgs e)
        {
            //дебаг работа
#if !MYTEST
            ProductsFromClient = new List<Product>();
            ProductsFromClient.Add(new Product() { NameProduct = "Колбаса" });
            ProductsFromClient.Add(new Product() { NameProduct = "Сыр" });
            ProductsFromClient.Add(new Product() { NameProduct = "Хлеб" });

            bool res = dish.Buterbrod(ProductsFromClient);

            textBox1.Text = "ddfdffdfdfd";
#endif
             Socket listeningSocket =new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
            //статический клас для приема сообщений
          //  Server.Listen(port,listeningSocket);

               


        }
    }
}
