using System.Collections.Generic;

namespace Library_Project.Resources.Classes
{
    /// <summary>
    /// a struct for defining the type of subsription
    /// it has two properties
    /// </summary>
    public sealed class KindOfSubscription
    {
        private static KindOfSubscription instance;
        private KindOfSubscription()
        {
        }
        public static KindOfSubscription Instance
        {
            get
            {
                if (instance == null)
                    instance = new KindOfSubscription();
                return instance;
            }
        }

        public string Name { get; set; }
        public decimal Cost { get; set; }
        public double Duration { get; set; }

        /// <summary>
        /// a method for initizalizing a collection of different kinds of subscriptions
        /// </summary>
        /// <returns>an array of KindOfSubscription</returns>
        public KindOfSubscription[] GetSubsriptions()
        {
            var kind1 = new KindOfSubscription { Name = "یک روزه", Cost = 5000, Duration = 1 };
            var kind2 = new KindOfSubscription{ Name = "یک هفته", Cost = 20000, Duration = 7};
            var kind3 = new KindOfSubscription { Name = "یک ماهه", Cost = 70000, Duration = 30};
            var kind4 = new KindOfSubscription { Name = "سه ماهه", Cost = 200000, Duration = 90 };
            var kind5 = new KindOfSubscription { Name = "شش ماهه", Cost = 350000, Duration = 180};
            var kind6 = new KindOfSubscription { Name = "یک ساله", Cost = 600000, Duration = 360};

            var kinds = new KindOfSubscription[6] { kind1, kind2, kind3, kind4, kind5, kind6 };
            return kinds;
        }

        /// <summary>
        /// get all names of kinds inside KindOfSubscription collection
        /// </summary>
        /// <returns>a string array</returns>
        public string[] GetNames()
        {
            var kinds = Instance.GetSubsriptions();
            List<string> names = new List<string>();
            for (int i = 0; i < kinds.Length; i++)
                names.Add(kinds[i].Name);
            return names.ToArray();
        }
        /// <summary>
        /// a method for calculating cost of a selected combo item in combobox
        /// if found the name it will return the cost of subscription
        /// else returns -1
        /// </summary>
        /// <param name="selected">value of selected combo item in combobox</param>
        /// <returns>cost of subscription</returns>
        public decimal GetCost(string selected)
        {
            var kinds = Instance.GetSubsriptions();

            for (int i = 0; i < kinds.Length; i++)
            {
                if (kinds[i].Name == selected)
                {
                    return kinds[i].Cost;
                }
            }

            return -1;
        }
    }
}