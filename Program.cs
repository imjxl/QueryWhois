using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace queryWhois
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("begin");
            char[] ch = new char[]{'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
            //Task.Factory.StartNew(()=>getWho("whois.crsnic.net"));
            foreach(char c in ch)
            {
               foreach(char h in ch)
                {
                    string name="hi"+c.ToString()+h.ToString()+ ".com";
                    Console.WriteLine("Name:"+name+" "+GetWhois(name));
                }
                
            }
            Console.WriteLine("over");
            Console.Read();
        }

        public static string GetWhois(string name)
        {
            
            TcpClient client =new TcpClient();
            return client.ConnectAsync("whois.crsnic.net",43).ContinueWith((t) =>
             {
                 string result="";
                 if (t.IsCompletedSuccessfully)
                 {
                     NetworkStream stream = client.GetStream();
                     byte[] buffer = Encoding.ASCII.GetBytes(name + "\rn");
                     return stream.WriteAsync(buffer, 0, buffer.Length).ContinueWith((tk) =>
                      {
                        if (tk.IsCompletedSuccessfully)
                        {
                            byte[] buf = new byte[10240];
                            int len = stream.Read(buf, 0, buf.Length);
                            string msg = Encoding.ASCII.GetString(buf, 0, len);
                           result=msg.Substring(0,msg.IndexOf(">>>"));
                           if(result.ToLower().Contains("no match"))
                           {
                               return "No register";
                           }
                           else
                           {
                               return "sorry,it's already register";
                           }
                           
                       }
                        else
                        {
                            result=tk.Exception.InnerException.Message;
                        }
                        return result;
                    }).Result;

                 }
                 else
                 {
                     result=t.Exception.Message;
                 }
                 return  result;
             }).Result;
        }
    }
}
