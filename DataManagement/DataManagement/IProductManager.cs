using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public interface IProductManager {
        Product? getProduct(int id);
        Product? getProduct(string title);
        void addNewProduct(Product pWithoutId);
        void modifyProduct(Product pWithValidId);
        void deleteProduct(int id);
        IList<Product> getAllProducts();
        IList<Product> getAvailableProducts();
        IList<Product> getProducts(string productTitle);
    }
}
