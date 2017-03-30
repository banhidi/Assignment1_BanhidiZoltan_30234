using DataManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FurnitureManager {
    public partial class ShoppingCartSelector : Form {
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
                        operationButton.Text = "Add product to shopping cart";
                        break;
                    case OperationEnum.Modify:
                        operationButton.Text = "Modify product in shopping cart";
                        break;
                }
            }
        }

        public int Quantity {
            set {
                if (value > 0 && value < 1000)
                quantityUpDown.Value = value;
            }
        }

        public ShoppingCartSelector(RegularUserForm ruf) {
            InitializeComponent();
            Operation = OperationEnum.Add;
            regularUserForm = ruf;
            reloadShoppingProducts();
        }

        private void reloadShoppingProducts() {
            IProductManager mgr = new MySqlProductManager();
            shoppingProductGrid.DataSource = mgr.getAvailableProducts();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            regularUserForm.Enabled = true;
            this.Close();
        }

        private void showErrorMessage(string text) {
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void setOnlyProduct(Product p) {
            IList<Product> list = new List<Product>();
            list.Add(p);
            shoppingProductGrid.DataSource = list;
        }

        private void add() {
            if (shoppingProductGrid.SelectedRows.Count == 1) {
                try {
                    Controller.OrderController oc = new Controller.OrderController();
                    Product p = (Product)shoppingProductGrid.SelectedRows[0].DataBoundItem;
                    oc.addProductToOrder(p.id, (int)quantityUpDown.Value,
                        regularUserForm.SelectedOrderId, regularUserForm.RegularUserName);
                    regularUserForm.reloadOrdersTab();
                    regularUserForm.reloadProducts();
                    regularUserForm.Enabled = true;
                    this.Close();
                } catch(DataManagement.Exceptions.IllegalQuantityException e) {
                    showErrorMessage("The introduced quantity is invalid.");
                } catch(DataManagement.Exceptions.ProductAlreadyInShoppingCartException ex) {
                    showErrorMessage("The introduced product is already in the shopping cart.");
                } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                    showErrorMessage("Error when connecting to the database.");
                }
            }
        }

        private void modify() {
            try {
                Controller.OrderController oc = new Controller.OrderController();
                Product p = (Product)shoppingProductGrid.SelectedRows[0].DataBoundItem;
                oc.modifyProductInShoppingCart(p.id, (int)quantityUpDown.Value,
                    regularUserForm.SelectedOrderId, regularUserForm.RegularUserName);
                regularUserForm.reloadOrdersTab();
                regularUserForm.reloadProducts();
                regularUserForm.Enabled = true;
                this.Close();
            } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                showErrorMessage("Error when connecting to the database.");
            } catch (DataManagement.Exceptions.IllegalQuantityException e) {
                showErrorMessage("The introduced quantity is invalid.");
            }
        }

        private void operationButton_Click(object sender, EventArgs e) {
            switch(operation) {
                case OperationEnum.Add:
                    add();
                    break;
                case OperationEnum.Modify:
                    modify();
                    break;
            }
        }
    }
}
