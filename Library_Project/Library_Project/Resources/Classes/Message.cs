using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Project.Resources.Classes
{

    public enum MassengeType { employee, member }
    public interface IMessanger { }
    /// <summary>
    /// a generic class for messages. consists of 3 fields:
    /// SenderUsername => username of person who has sent the message
    /// RecieverUsername => username of person who has sent the message can be either Member or Employee
    /// </summary>
    /// <typeparam name="T">specify type of reciever</typeparam>

    public class Message<T> where T : IMessanger
    {
        private static Message<T> instance;

        private string _senderUsername;
        private string _recieverUsername;
        private string _messageText;
        private MassengeType _typeOfReciever;

        public string SenderUsername { get => _senderUsername; set => _senderUsername = value; }
        public string RecieverUsername { get => _recieverUsername; set => _recieverUsername = value; }
        public string MessageText { get => _messageText; set => _messageText = value; }
        public MassengeType TypeOfReciever { get => _typeOfReciever; set => _typeOfReciever = value; }

        public static Message<T> Instance
        {
            get
            {
                if (instance == null) instance = new Message<T>();
                return instance;
            }
        }

        private Message()
        {
        }

        public Message(string senderUsername, string recieverUsername, string messageText, MassengeType receiverType)
        {
            SenderUsername = senderUsername;
            RecieverUsername = recieverUsername;
            MessageText = messageText;
            TypeOfReciever = receiverType;
        }

        public bool SendMessage()
        {
            string command = "INSERT INTO T_Message VALUES('" + this.SenderUsername + "', '" + this.RecieverUsername + "', '" + this.MessageText + "', '" + this.TypeOfReciever.ToString() + "')";
            return DatabaseControl.Exe(command);
        }

        public static Message<T>[] RecieveAllMessages(string reciever, string sender)
        {
            string command = "SELECT * FROM T_Message WHERE senderUsername='" + sender + "' AND recieverUsername='" + reciever + "'";
            var data = DatabaseControl.Select(command);

            List<Message<T>> messages = new List<Message<T>>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var message = new Message<T>(data.Rows[i]["senderUsername"].ToString(), data.Rows[i]["recieverUsername"].ToString(), data.Rows[i]["message"].ToString(), (MassengeType)Enum.Parse(typeof(MassengeType), data.Rows[i]["receiverType"].ToString()));
                messages.Add(message);
            }

            return messages.ToArray();
        }

        public static string[] AllSenders(string reciever, MassengeType typeofSender)
        {
            string command = "SELECT * FROM T_Message WHERE recieverUsername='" + reciever + "' AND typeofSender='" + typeofSender.ToString().ToLower() + "'";
            var data = DatabaseControl.Select(command);

            List<string> names = new List<string>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string sender = data.Rows[i]["senderUsername"].ToString();
                names.Add(sender);
            }

            return names.ToArray();
        }
    }
}
