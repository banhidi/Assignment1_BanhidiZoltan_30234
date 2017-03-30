using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataManagement;
using Controller;

namespace FurnitureManager {
    public partial class RegularUserForm : Form, IAbstractForm {
        private Product selectedProduct;

        public int SelectedOrderId {
            get {
                if (orderGrid.SelectedRows.Count == 1)
                    return ((Order)orderGrid.SelectedRows[0].DataBoundItem).id;
                else
                    return -1;
            }
        }

        public string RegularUserName {
            set {
                usernameLabel.Text = value;
            }

            get {
                return usernameLabel.Text;
            }
        }
        public RegularUserForm(string username) {
            InitializeComponent();
            usernameLabel.Text = username;
            reloadProducts();
            reloadOrdersTab();
        }

        public void setNewUsername(string username) {
            usernameLabel.Text = username;
        }

        public void setEnabled() {
            this.Enabled = true;
        }

        public void reloadProducts() {
            productGrid.DataSource = 
                new Controller.ProductController().searchProducts(productTitleTextBox.Text);
        }

        public void reloadOrdersTab() {
            Controller.OrderController oc = new Controller.OrderController();
            IList<Order> list = oc.getOrders(usernameLabel.Text);
            orderGrid.AutoGenerateColumns = false;
            orderGrid.DataSource = list;
            reloadShoppingCart();
        }

        private void reloadShoppingCart() {
            if (orderGrid.SelectedRows.Count == 1) {
                Order o = (Order)orderGrid.SelectedRows[0].DataBoundItem;
                shoppingCartGrid.AutoGenerateColumns = false;
                shoppingCartGrid.DataSource = o.getShoppingCart();
            }
        }

        private void signOutButton_Click(object sender, EventArgs e) {
            this.Owner.Show();
            this.Close();
        }

        private void searchProductButton_Click(object sender, EventArgs e) {
            reloadProducts();
        }

        private void productGrid_SelectionChanged(object sender, EventArgs e) {
            if (productGrid.SelectedRows.Count == 1) {
                selectedProduct =
                    (Product) productGrid.SelectedRows[0].DataBoundItem;
                deleteProductButton.Enabled = true;
                modifyProductButton.Enabled = true;
            } else {
                deleteProductButton.Enabled = false;
                modifyProductButton.Enabled = false;
            }
        }

        private void showErrorMessage(string text) {
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void deleteProductButton_Click(object sender, EventArgs e) {
            try {
                Controller.ProductController pc = new Controller.ProductController();
                pc.deleteProduct(usernameLabel.Text, selectedProduct.id);
                reloadProducts();
            } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                showErrorMessage("Error when connecting to database.");
            }
        }

        private void addNewProductButton_Click(object sender, EventArgs e) {
            ProductForm pf = new FurnitureManager.ProductForm(this);
            pf.Operation = OperationEnum.Add;
            pf.Show();
            this.Enabled = false;
        }

        private void modifyProductButton_Click(object sender, EventArgs e) {
            ProductForm pf = new FurnitureManager.ProductForm(this);
            pf.Operation = OperationEnum.Modify;
            pf.Product = selectedProduct;
            pf.Show();
            this.Enabled = false;
        }

        private void modifyAccountButton_Click(object sender, EventArgs e) {
            IUserManager mgr = new MySqlUserManager();
            User u = (User)mgr.getUser(usernameLabel.Text);
            AccountForm af = new AccountForm(this);
            af.User = u;
            af.Show();
            this.Enabled = false;
        }

        private void orderGrid_SelectionChanged(object sender, EventArgs e) {
            reloadShoppingCart();
            totalPriceTextBox.Text = "";
        }

        private void addNewOrderButton_Click(object sender, EventArgs e) {
            OrderForm of = new OrderForm(this);
            of.Show();
            this.Enabled = false;
        }

        private void deleteOrderButton_Click(object sender, EventArgs e) {
            if (orderGrid.SelectedRows.Count == 1) {
                try {
                    OrderController oc = new OrderController();
                    int orderId = ((Order)orderGrid.SelectedRows[0].DataBoundItem).id;
                    oc.deleteOrder(orderId);
                    reloadOrdersTab();
                } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                    showErrorMessage("Error when connecting to the database.");
                }
            }
        }

        private void modifyOrderButton_Click(object sender, EventArgs e) {
            if (orderGrid.SelectedRows.Count == 1) {
                Order o = (Order)orderGrid.SelectedRows[0].DataBoundItem;
                OrderForm of = new OrderForm(this);
                of.Operation = OperationEnum.Modify;
                of.OrderId = o.id;
                of.CustomerName = o.customerName;
                of.DeliveryAdress = o.deliveryAdress;
                of.DeliveryDate = o.deliveryDate;
                of.OrderStatus = o.orderStatus;
                of.Show();
                this.Enabled = false;
            }
            

        }

        private void addNewItemButton_Click(object sender, EventArgs e) {
            ShoppingCartSelector selector = new ShoppingCartSelector(this);
            selector.Show();
            this.Enabled = false;
        }

        private void modifyItemButton_Click(object sender, EventArgs e) {
            if (shoppingCartGrid.SelectedRows.Count == 1) {
                ShoppingCartItem s = (ShoppingCartItem)shoppingCartGrid.SelectedRows[0].DataBoundItem;
                ShoppingCartSelector selector = new ShoppingCartSelector(this);
                selector.Quantity = s.Quantity;
                IProductManager mgr = new MySqlProductManager();
                selector.setOnlyProduct((Product)mgr.getProduct(s.productId));
                selector.Operation = OperationEnum.Modify;
                selector.Show();
                this.Enabled = false;
            }
        }

        private void deleteItemButton_Click(object sender, EventArgs e) {
            if (shoppingCartGrid.SelectedRows.Count == 1 && orderGrid.SelectedRows.Count == 1) {
                OrderController oc = new OrderController();
                ShoppingCartItem s = (ShoppingCartItem)shoppingCartGrid.SelectedRows[0].DataBoundItem;
                Order o = (Order)orderGrid.SelectedRows[0].DataBoundItem;
                oc.removeProductFromShoppingCart(s.productId, o.id, RegularUserName);
                reloadOrdersTab();
                reloadProducts();
            }
        }

        private void calculateTotalPriceButton_Click(object sender, EventArgs e) {
            if (orderGrid.SelectedRows.Count == 1) {
                OrderController oc = new OrderController();
                int orderId = ((Order)orderGrid.SelectedRows[0].DataBoundItem).id;
                totalPriceTextBox.Text = oc.calculateTotalPrice(orderId).ToString();
            } else
                totalPriceTextBox.Clear();
        }
    }
}
