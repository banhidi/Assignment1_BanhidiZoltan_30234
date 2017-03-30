using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;

namespace FurnitureManager {
    public partial class OrderForm : Form {
        private RegularUserForm regularUserForm;
        private OperationEnum operation;

        public OperationEnum Operation {
            get {
                return operation;
            }

            set {
                operation = value;
                switch(operation) {
                    case OperationEnum.Add:
                        operationButton.Text = "Add new order";
                        break;
                    case OperationEnum.Modify:
                        operationButton.Text = "Modify order";
                        break;
                }                 
            }
        }

        public int OrderId {
            get {
                return int.Parse(idTextBox.Text);
            }

            set {
                idTextBox.Text = value.ToString();
            }
        }

        public string CustomerName {
            get {
                return customerTextBox.Text;
            }

            set {
                customerTextBox.Text = value;
            }
        }

        public String DeliveryAdress {
            get {
                return deliveryAdressTextBox.Text;
            }

            set {
                deliveryAdressTextBox.Text = value;
            }
        }

        public DateTime DeliveryDate {
            get {
                return deliveryDateTimePicker.Value;
            }

            set {
                deliveryDateTimePicker.Value = value;
            }
        }

        public bool OrderStatus {
            get {
                return orderStatusCheckBox.Checked;
            }

            set {
                orderStatusCheckBox.Checked = value;
            }
        }

        public OrderForm(RegularUserForm ruf) {
            InitializeComponent();
            regularUserForm = ruf;
            Operation = OperationEnum.Add;
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            regularUserForm.Enabled = true;
            this.Close();
        }

        private void showErrorMessage(string text) {
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void addOrder() {
            try {
                OrderController oc = new OrderController();
                oc.addNewOrder(CustomerName, DeliveryAdress, DeliveryDate, OrderStatus,
                    regularUserForm.RegularUserName);
                regularUserForm.reloadOrdersTab();
                regularUserForm.Enabled = true;
                this.Close();
            } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                showErrorMessage("Error when connecting to the database.");
            }
        }

        private void modifyOrder() {
            try {
                OrderController oc = new OrderController();
                oc.modifyOrder(OrderId, CustomerName, DeliveryAdress, DeliveryDate, OrderStatus);
                regularUserForm.reloadOrdersTab();
                regularUserForm.Enabled = true;
                this.Close();
            } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                showErrorMessage("Error when connecting to the database.");
            }
        }

        private void operationButton_Click(object sender, EventArgs e) {
            switch(operation) {
                case OperationEnum.Add:
                    addOrder();
                    break;
                case OperationEnum.Modify:
                    modifyOrder();
                    break;
            }
        }
    }
}
