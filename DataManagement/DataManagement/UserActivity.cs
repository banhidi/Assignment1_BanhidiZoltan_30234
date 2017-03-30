
namespace DataManagement {
    public enum TableNameEnum { Product, CustomerOrder, ShoppingCart };
    public struct UserActivity {
        public int id { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public int itemId { get; set; }
        public string itemTitle { get; set; }
        public string description { get; set; }
        public TableNameEnum tableName { get; set; }
        public System.DateTime activityDateTime { get; set; }

        public static string stringTableName(TableNameEnum tableName) {
            switch(tableName) {
                case TableNameEnum.Product:
                    return "Product";
                case TableNameEnum.CustomerOrder:
                    return "CustomerOrder";
                case TableNameEnum.ShoppingCart:
                    return "ShoppingCart";
                default:
                    return "";
            }
        }

        public void addNewProduct(int userId, Product productWithValidId) {
            this.userId = userId;
            itemId = productWithValidId.id;
            tableName = TableNameEnum.Product;
            activityDateTime = System.DateTime.Now;
            description = "add new product(" + productWithValidId.ToString() + ")";
        }

        public void deleteProduct(int userId, Product productWithValidId) {
            this.userId = userId;
            itemId = productWithValidId.id;
            tableName = TableNameEnum.Product;
            activityDateTime = System.DateTime.Now;
            description = "delete product(" + productWithValidId.ToString() + ")";
        }

        public void modifyProduct(int userId, Product beforeWithValidId, Product afterWithValidId) {
            this.userId = userId;
            itemId = beforeWithValidId.id;
            tableName = TableNameEnum.Product;
            activityDateTime = System.DateTime.Now;
            description = "modify product from (" + beforeWithValidId.ToString() +
                ") to (" + afterWithValidId.ToString() + ")";
        }

    }
}