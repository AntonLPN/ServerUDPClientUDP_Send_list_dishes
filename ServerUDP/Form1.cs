using MyLibrary_Product_Dishes;
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



namespace ServerUDP
{
    public partial class Form1 : Form
    {
         int localPort; // порт приема сообщений
      
     

      

        int remotePort; // Порт для отправки сообщений
    
        UdpClient receiver;
        UdpClient sender;
        Thread receiveThread;
        Dish dish;//для отправки ответа

        public Form1()
        {
            InitializeComponent();
         
     
          


            localPort = 8002;
            remotePort = 8001;
            dish = new Dish();
        }
    



      


       

        /// <summary>
        /// Метод добавления текстра при textbox.BeginInvoke
        /// </summary>
        /// <param name="str"></param>
        public void AddText(string str)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(str);
        
            textBox1.Text+= builder.ToString()+Environment.NewLine;
        }


       

   

        /// <summary>
        /// Для отправки ответа клиенту принимает аргумент список продуктов для приготовления блюда
        /// </summary>
        /// <param name="productsList"></param>
        private void SendMessage(List<Product> productsList)
        {
            sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                
                    string message = "Сообщение от сервера\r\n"; // сообщение для отправки
                    byte[] data = Encoding.Unicode.GetBytes(message);


                bool match = false;//для проверки совпадения по всем блюдам
                //Делаем проверку того какое блюдо у нас получится из того набора продуктов которые отправил клиент
                if (dish.Buterbrod(productsList)==true)
                {
                    message += "Бутерброд" + Environment.NewLine;
                    match=true;
                }
                if (dish.FrenchFries(productsList)==true)
                {
                    message += "Жаренная картошка" + Environment.NewLine;
                    match = true;
                }
                if(dish.Plov(productsList)==true)
                {
                    message += "Плов" + Environment.NewLine;
                    match = true;
                }


                //если есть хоть одно совпадение то  по блюдам отправлям их клиенту в виде строки иначе отправляем ответ что невозможно приготовить блюда из 
                //того набора продуктов котоый прислали
                if (match==true)
                {
                    data= Encoding.Unicode.GetBytes(message);
                    sender.Send(data, data.Length, IPAddress.Loopback.ToString(), remotePort); // отправка


                    this.textBox1.BeginInvoke(new Action<string>(AddText), "Сообщение отправлено: Есть соврадение по рецептам ");
                }
                else
                {
                    data = Encoding.Unicode.GetBytes("Нет совпадения по рецептам из того набора продуктов который вы прислали");
                    sender.Send(data, data.Length, IPAddress.Loopback.ToString(), remotePort); // отправка
                    this.textBox1.BeginInvoke(new Action<string>(AddText), "Сообщение отправлено: Без рецепта так как небыло совбадения по блюдам");
                }


                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
               sender.Close();
            }
        }


        private void ReceiveMessage()
        {
            if (receiver != null)
            {
                receiver.Close();
            }
            receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                // получаем ответ
           
               
                //int bytes = 0;
                byte[] data = new byte[1204];
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    data = receiver.Receive(ref remoteIp); // получаем данные
                    
                    string message = Encoding.Unicode.GetString(data);

                    builder.Append(Encoding.Unicode.GetString(data));


                  
                    this.textBox1.BeginInvoke(new Action<string>(AddText), $"Собеседник:  {remoteIp.Port}");

                    //принимаем список продуктов и десиарилизуем его
                    //десиариализация колекции улиц
                    try
                    {
                       // List<Product> restoreProduct = new List<Product>();
                      var  restoreProduct = JsonSerializer.Deserialize<List<Product>>(builder.ToString());
                        //когда пришел запрос отправляем ответ в виде строки с названием блюда

                        SendMessage(restoreProduct);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.StackTrace,"MyError Server 01",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    
                }
            }
            //Исключение WSACancelBlockingCall при срабатывании метода Close(), магия не трогать
            catch (SocketException ex) when (ex.ErrorCode == 10004)
            {
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }

        private  void buttonStart_Click(object sender, EventArgs e)
        {



            receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.Start();
            textBox1.Text += "Start server..........";
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // Controls.Add(textBox1);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (receiver != null)
            {
                receiver.Close();
            }

            this.Close();
            Application.Exit();
        }
    }
}
