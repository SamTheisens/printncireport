using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;

namespace PrintNCIAgent
{
    public partial class AlarmScreen
    {
        public AlarmScreen()
        {
            InitializeComponent();
        }

        private ObservableCollection<NotifyObject> notifyContent;
        /// <summary>
        /// A collection of NotifyObjects that the main window can add to.
        /// </summary>
        public ObservableCollection<NotifyObject> NotifyContent
        {
            get
            {
                if (notifyContent == null)
                {
                    // Not yet created.
                    // Create it.
                    NotifyContent = new ObservableCollection<NotifyObject>();
                }

                return notifyContent;
            }
            set
            {
                notifyContent = value;
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            var hyperlink = sender as Hyperlink;

            if (hyperlink == null)
                return;

            var notifyObject = hyperlink.Tag as NotifyObject;
            if (notifyObject != null)
            {
                MessageBox.Show("\"" + notifyObject.Message + "\"" + " clicked!");
            }
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            ForceHidden();
        }
    }
}