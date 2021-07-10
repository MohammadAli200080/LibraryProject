using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Project.Resources.Classes
{
    public struct MassengerUsers
    {
        public string Username { get; set; }
        public byte[] Image { get; set; }
    }
    public enum MassengeType { employee, member }
    public interface IMessanger { }
    /// <summary>
    /// a generic class for messages. consists of 3 fields:
    /// SenderUsername => username of person who has sent the message
    /// RecieverUsername => username of person who has sent the message can be either Member or Employee
    /// </summary>
    /// <typeparam name="T">specify type of reciever</typeparam>

    public class Message
    {
        private static Message instance;

        private string _senderUsername;
        private string _recieverUsername;
        private string _messageText;
        private int _row;
        private MassengeType _senderType;

        public string SenderUsername { get => _senderUsername; set => _senderUsername = value; }
        public string RecieverUsername { get => _recieverUsername; set => _recieverUsername = value; }
        public string MessageText { get => _messageText; set => _messageText = value; }
        public MassengeType SenderType { get => _senderType; set => _senderType = value; }
        public int Row { get => _row; set => _row = value; }

        public static Message Instance
        {
            get
            {
                if (instance == null) instance = new Message();
                return instance;
            }
        }

        private Message()
        {
        }

        public Message(int Row,string senderUsername, string recieverUsername, string messageText, MassengeType senderType)
        {
            SenderUsername = senderUsername;
            RecieverUsername = recieverUsername;
            MessageText = messageText;
            SenderType = senderType;
            this.Row = Row;
        }
        public Message(string senderUsername, string recieverUsername, string messageText, MassengeType senderType)
        {
            SenderUsername = senderUsername;
            RecieverUsername = recieverUsername;
            MessageText = messageText;
            SenderType = senderType;
        }
        public bool SendMessage()
        {
            string command = "INSERT INTO T_Messages VALUES('" + this.SenderUsername + "', '" + this.RecieverUsername + "', N'" + this.MessageText + "', '" + this.SenderType.ToString() + "')";

            return DatabaseControl.Exe(command);
        }

        public Message[] RecieveAllMessages(string reciever, string sender)
        {
            Row = 1;
            string command = "SELECT * FROM T_Messages WHERE senderUsername='" + sender + "'";
            var data = DatabaseControl.Select(command);

            List<Message> messages = new List<Message>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["recieverUsername"].ToString().ToLower() == reciever.ToLower())
                {
                    var message = new Message(Row, data.Rows[i]["senderUsername"].ToString(), data.Rows[i]["recieverUsername"].ToString(), data.Rows[i]["message"].ToString(), (MassengeType)Enum.Parse(typeof(MassengeType), data.Rows[i]["typeofSender"].ToString()));
                    messages.Add(message);
                    Row++;
                }
            }

            return messages.ToArray();
        }

        public ObservableCollection<string> AllSenders(string reciever, MassengeType typeofSender)
        {
            string command = "SELECT DISTINCT senderUsername,typeofSender FROM T_Messages WHERE recieverUsername='" + reciever + "'";
            var data = DatabaseControl.Select(command);

            ObservableCollection<string> names = new ObservableCollection<string>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["typeofSender"].ToString() == typeofSender.ToString())
                {
                    string sender = data.Rows[i]["senderUsername"].ToString();
                    names.Add(sender);
                }
            }

            return names;
        }

        public List<MassengerUsers> GetMassengerUsers(MassengeType type)
        {
            DataTable data = new DataTable();
            List<MassengerUsers> members = new List<MassengerUsers>();
            string userName;
            byte[] image;
            if (type == MassengeType.member) data = DatabaseControl.Select("SELECT username,imgSrc FROM T_Members");
            else data = DatabaseControl.Select("SELECT username,imgSrc FROM T_Employees");

            for (int i = 0; i < data.Rows.Count; i++)
            {
                userName = data.Rows[i]["username"].ToString();
                image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());

                members.Add(new MassengerUsers { Username = userName, Image = image });
            }
            return members;
        }
    }
}
