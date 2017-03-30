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

namespace FurnitureManager {
    
    public partial class ProductForm : Form {
        private OperationEnum operation;
        private RegularUserForm regularUserForm;
        public OperationEnum Operation {
            set {
                operation = value;
                if (value == OperationEnum.Add)
                    operationButton.Text = "Add new product";
                else if (value == OperationEnum.Modify) {
                    operationButton.Text = "Modify product";
                }
            }
        }

        public Product Product {
            set {
                idTextBox.Text = value.id.ToString();
                titleTextBox.Text = value.title;
                colorTextBox.Text = value.color;
                widthUpDown.Value = value.width;
                heightUpDown.Value = value.height;
                depthUpDown.Value = value.depth;
                priceTextBox.Text = value.price.ToString();
                stockUpDown.Value = value.stock;
            }
        }

        public ProductForm(RegularUserForm ruf) {
            InitializeComponent();
            regularUserForm = ruf;
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            regularUserForm.Enabled = true;
            this.Close();
        }

        private void showErrorMessage(string text) {
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Product constructProduct() {
            Product p = new Product();
            if (idTextBox.Text.Length > 0)
                p.id = int.Parse(idTextBox.Text);
            p.title = titleTextBox.Text;
            p.color = colorTextBox.Text;
            p.width = (int)widthUpDown.Value;
            p.height = (int)heightUpDown.Value;
            p.depth = (int)depthUpDown.Value;
            p.price = float.Parse(priceTextBox.Text);
            p.stock = (int)stockUpDown.Value;
            return p;
        }

        private void operationButton_Click(object sender, EventArgs e) {
            if (operation == OperationEnum.Add) {
                Product p;
                try {
                    p = constructProduct();
                } catch (FormatException ex) {
                    showErrorMessage("Please introduce a valid price. It must be greather or equal " +
                        "than 0.");
                    return;
                }
                Controller.ProductController pc = new Controller.ProductController();
                try {
                    pc.addNewProduct(regularUserForm.RegularUserName, p);
                    regularUserForm.reloadProducts();
                    regularUserForm.Enabled = true;
                    this.Close();
                } catch (Controller.Exceptions.InvalidProductTitleException ex) {
                    showErrorMessage("Please introduce a valid product title. It must be at " +
                        "least 3 characters length and must be unique in the table.");
                } catch (Controller.Exceptions.InvalidProductSizeException ex) {
                    showErrorMessage("Please introduce valid product size (width, height, depth). " +
                        "These must be between 1..1000.");
                } catch (Controller.Exceptions.InvalidProductPriceException ex) {
                    showErrorMessage("Please introduce a valid price. It must be greather or equal " +
                        "than 0.");
                } catch (Controller.Exceptions.InvalidProductStockException ex) {
                    showErrorMessage("Please introduce a valid stock. It must be greather or equal " +
                        "than 0.");
                } catch (DataManagement.Exceptions.MySqlConnectionException ex) {
                    showErrorMessage("Error when connecting to the database.");
                }
            } else if (operation == OperationEnum.Modify) {
                Product p;
                try {
                    p = constructProduct();
                } catch (FormatException ex) {
                    showErrorMessage("Please introduce a valid price. It must be greather or equal " +
                   "than 0.");
                    return;
                }
                Controller.ProductController pc = new Controller.ProductController();
                try {
                    pc.modifyProduct(regularUserForm.RegularUserName, p);
                    regularUserForm.reloadProducts();
                    regularUserForm.Enabled = true;
                    this.Close();       
                } catch (Controller.Exceptions.InvalidProductTitleException ex) {
                    showErrorMessage("Please introduce a valid product title. It must be at " +
                        "least 3 characters length and must be unique in the table.");
                } catch (Controller.Exceptions.InvalidProductSizeException ex) {
                    showErrorMessage("Please introduce valid product size (width, height, depth). " +
                        "These must be between 1..1000.");
                } catch (Controller.Exceptions.InvalidProductPriceException ex) {
                    showErrorMessage("Please introduce a valid price. It must be greather or equal " +
                        "than 0.");
                } catch (Controller.Exceptions.InvalidProductStockException ex) {
                    showErrorMessage("Please introduce a valid stock. It must be greather or equal " +
                        "than 0.");
                } catch (DataManagement.Exceptions.MySqlConnectionException ex) {
                    showErrorMessage("Error when connecting to the database.");
                }
            }
        }

    }
}
