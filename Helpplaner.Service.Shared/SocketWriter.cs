using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace Helpplaner.Service.Shared
{
    public class SocketWriter
    {
        Socket socket;
        IServiceLogger logger;
     



        public SocketWriter(Socket socket, IServiceLogger logger)
        {
            this.logger = logger;
            this.socket = socket;


        }

        public void Send(string message)
        {
            try
            {
                Message Message = new Message();
                byte[] buffer = new byte[1024];
             
                Message.Type = message.GetType().ToString();
                Message.Content = message;

                buffer = JsonSerializer.SerializeToUtf8Bytes(Message);

                socket.Send(buffer);
            }
            catch (Exception e)
            {

                logger.Log(e.Message, "red");
            }


        }
        public void SendObject(object obj)
        {
            try
            {
                Message Message = new Message();

                byte[] buffer = new byte[10000];
                Message = new Message();
               
                    Message.Type = obj.GetType().ToString();
                    Message.Content = JsonSerializer.Serialize(obj);
                buffer = JsonSerializer.SerializeToUtf8Bytes(Message);    
             
                socket.Send(buffer);    

                
            }
            catch (Exception e)
            {

                logger.Log(e.Message, "red");
            }   
        }
        public void SendObjectArray(object[] obj)
        {
            try
            {
                Message InfoMessage = new Message();
               Message message = new Message();
                byte[] buffer = new byte[10000];
                byte[] infoBuffer = new byte[10000];
                byte[] revievingBuffer = new byte[10000];
             

                InfoMessage.Type = obj.GetType().ToString();
                InfoMessage.Content = "" + obj.Length;
                infoBuffer = JsonSerializer.SerializeToUtf8Bytes(InfoMessage);  
                socket.Send(infoBuffer);    
                socket.Receive(revievingBuffer);
                revievingBuffer = new byte[10000];  
             

                foreach (object obje in obj)
                {
                    string serializedObj = JsonSerializer.Serialize(obje);
                       byte[] arr =  JsonSerializer.SerializeToUtf8Bytes(obje);
                    object ob =  JsonSerializer.Deserialize<object>(arr);   
                    socket.Send(arr);
                    socket.Receive(revievingBuffer); 
                    Console.WriteLine(Encoding.UTF8.GetString(buffer)); 

                }
             


              
                
           
            }
            catch (Exception e)
            {

                logger.Log(e.Message, "red");
            }
        } 
        public byte[][] Split(byte[] array, int size)
        {
            int arrayLength = array.Length;
            byte[][] result = new byte[(int)Math.Ceiling((double)arrayLength / size)][];
            int offset = 0;
            for (int i = 0; i < result.Length; i++)
            {
                int length = Math.Min(size, arrayLength - offset);
                byte[] temp = new byte[length];
                Array.Copy(array, offset, temp, 0, length);
                result[i] = temp;
                offset += length;
            }
            return result;
        }   
    }
}
