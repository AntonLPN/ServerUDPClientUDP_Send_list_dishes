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
namespace WF_ClientTCP
{
    public partial class ClientTCP : Form
    {

        int remotePort; // Порт для отправки сообщений
        UdpClient receiver;      
         int localPort; // локальный порт для прослушивания входящих подключений
        Dish menuDish;
        Thread receiveThread;

        public ClientTCP()
        {
            InitializeComponent();
            localPort = 8001;
            remotePort = 8002;

            menuDish = new Dish();
            
            menuDish.makeMenu();//создаем меню
        }

        /// <summary>
        /// Метод добавления текстра при textbox.BeginInvoke
        /// </summary>
        /// <param name="str"></param>
        public void AddText(string str)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(str);
            textBox1.Text += builder.ToString()+Environment.NewLine;
        }

        private  void SendMessage()
        {
            List<Product> productsList = new List<Product>();
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
               
                  

              
                //А теперь немного колхоза
                //добавляем название продукта в список
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    if (item.ToString()=="Хлеб")
                    {
                        productsList.Add(new Product() {NameProduct="Хлеб" });
                    }
                    else if(item.ToString()== "Картошка")
                    {
                        productsList.Add(new Product() { NameProduct = "Картошка" });
                    }
                    else if (item.ToString() == "Колбаса")
                    {
                        productsList.Add(new Product() { NameProduct = "Колбаса" });
                    }
                    else if (item.ToString() == "Рис")
                    {
                        productsList.Add(new Product() { NameProduct = "Рис" });
                    }
                    else if (item.ToString() == "Сахар")
                    {
                        productsList.Add(new Product() { NameProduct = "Сахар" });
                    }
                    else if (item.ToString() == "Вода")
                    {
                        productsList.Add(new Product() { NameProduct = "Вода" });
                    }               
                    else if (item.ToString() == "Сыр")
                    {
                        productsList.Add(new Product() { NameProduct = "Сыр" });
                    }
                    else if (item.ToString() == "Постное масло")
                    {
                        productsList.Add(new Product() { NameProduct = "Постное масло" });
                    }
                    else if (item.ToString() == "Соль")
                    {
                        productsList.Add(new Product() { NameProduct = "Соль" });
                    }
                    else if (item.ToString() == "Мясо")
                    {
                        productsList.Add(new Product() { NameProduct = "Мясо" });
                    }
                }
                //проводим сериализацию обьекта для отправки на сервер
                string message = JsonSerializer.Serialize<List<Product>>(productsList); // сообщение для отправки
                byte[] data = Encoding.Unicode.GetBytes(message);


                    sender.Send(data, data.Length, IPAddress.Loopback.ToString(), remotePort); // отправка


                    this.textBox1.BeginInvoke(new Action<string>(AddText), "Сообщение отправлено \n");


                
            }
            catch (SocketException ex)
            {
                return;
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message,"MyError",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                sender.Close();
            }
        }


        private  void ReceiveMessage()
        {
            if (receiver!=null)
            {
                receiver.Close();
            }
          
            receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
          
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    this.textBox1.BeginInvoke(new Action<string>(AddText), $"Собеседник: {message}");
                  
                }
                
            }
            //Исключение WSACancelBlockingCall при срабатывании метода Close(), магия не трогать
            catch (SocketException ex) when (ex.ErrorCode == 10004)
            {
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "MyError2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                receiver.Close();
                
            }
         
        }

        private void StartThread()
        {
            receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.Start();
            
            SendMessage(); // отправляем сообщение
        }
        private  void buttonConnect_Click(object sender, EventArgs e)
        {



            StartThread();



        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (receiver!=null)
            {
                receiver.Close();
            }
           
            this.Close();
            Application.Exit();
        }
    }
}
