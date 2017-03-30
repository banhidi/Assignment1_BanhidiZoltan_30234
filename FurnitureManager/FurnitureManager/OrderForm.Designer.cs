namespace FurnitureManager {
    partial class OrderForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.customerTextBox = new System.Windows.Forms.TextBox();
            this.deliveryAdressTextBox = new System.Windows.Forms.TextBox();
            this.deliveryDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.orderStatusCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.operationButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // idTextBox
            // 
            this.idTextBox.Location = new System.Drawing.Point(113, 25);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.ReadOnly = true;
            this.idTextBox.Size = new System.Drawing.Size(201, 20);
            this.idTextBox.TabIndex = 0;
            // 
            // customerTextBox
            // 
            this.customerTextBox.Location = new System.Drawing.Point(113, 51);
            this.customerTextBox.Name = "customerTextBox";
            this.customerTextBox.Size = new System.Drawing.Size(201, 20);
            this.customerTextBox.TabIndex = 1;
            // 
            // deliveryAdressTextBox
            // 
            this.deliveryAdressTextBox.Location = new System.Drawing.Point(113, 77);
            this.deliveryAdressTextBox.Name = "deliveryAdressTextBox";
            this.deliveryAdressTextBox.Size = new System.Drawing.Size(201, 20);
            this.deliveryAdressTextBox.TabIndex = 2;
            // 
            // deliveryDateTimePicker
            // 
            this.deliveryDateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.deliveryDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.deliveryDateTimePicker.Location = new System.Drawing.Point(114, 103);
            this.deliveryDateTimePicker.Name = "deliveryDateTimePicker";
            this.deliveryDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.deliveryDateTimePicker.TabIndex = 3;
            // 
            // orderStatusCheckBox
            // 
            this.orderStatusCheckBox.AutoSize = true;
            this.orderStatusCheckBox.Location = new System.Drawing.Point(114, 129);
            this.orderStatusCheckBox.Name = "orderStatusCheckBox";
            this.orderStatusCheckBox.Size = new System.Drawing.Size(92, 17);
            this.orderStatusCheckBox.TabIndex = 4;
            this.orderStatusCheckBox.Text = "Finished order";
            this.orderStatusCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Customer:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Delivery adress:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Delivery date:";
            // 
            // operationButton
            // 
            this.operationButton.AutoSize = true;
            this.operationButton.Location = new System.Drawing.Point(89, 186);
            this.operationButton.Name = "operationButton";
            this.operationButton.Size = new System.Drawing.Size(75, 23);
            this.operationButton.TabIndex = 9;
            this.operationButton.Text = "Operation";
            this.operationButton.UseVisualStyleBackColor = true;
            this.operationButton.Click += new System.EventHandler(this.operationButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(223, 186);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 234);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.operationButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.orderStatusCheckBox);
            this.Controls.Add(this.deliveryDateTimePicker);
            this.Controls.Add(this.deliveryAdressTextBox);
            this.Controls.Add(this.customerTextBox);
            this.Controls.Add(this.idTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderForm";
            this.Text = "Order";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.TextBox customerTextBox;
        private System.Windows.Forms.TextBox deliveryAdressTextBox;
        private System.Windows.Forms.DateTimePicker deliveryDateTimePicker;
        private System.Windows.Forms.CheckBox orderStatusCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button operationButton;
        private System.Windows.Forms.Button cancelButton;
    }
}