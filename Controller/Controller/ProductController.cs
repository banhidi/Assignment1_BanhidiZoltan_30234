using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManagement;

namespace Controller {
    public class ProductController {

        public IList<Product> searchProducts(string searchText) {
            IProductManager mgr = new MySqlProductManager();
            if (searchText.Length < 3)
                return mgr.getAllProducts();
            else
                return mgr.getProducts(searchText);
        }

        public void addNewProduct(string username, Product p) {
            int sizeUpperBound = 1000;
            if (p.title.Length < 3)
                throw new Exceptions.InvalidProductTitleException();
            if (p.width < 0 || p.width > sizeUpperBound)
                throw new Exceptions.InvalidProductSizeException();
            if (p.height < 0 || p.height > sizeUpperBound)
                throw new Exceptions.InvalidProductSizeException();
            if (p.depth < 0 || p.depth > sizeUpperBound)
                throw new Exceptions.InvalidProductSizeException();
            if (p.price < 0.0f)
                throw new Exceptions.InvalidProductPriceException();
            if (p.stock < 0)
                throw new Exceptions.InvalidProductStockException();
            IProductManager mgr = new MySqlProductManager();
            if (mgr.getProduct(p.title) != null)
                throw new Exceptions.InvalidProductTitleException();
            mgr.addNewProduct(p);

            Product addedProduct = (Product) mgr.getProduct(p.title);
            IUserActivityManager logMgr = new MySqlUserActivityManager();
            IUserManager userMgr = new MySqlUserManager();
            UserActivity ua = new UserActivity();
            ua.addNewProduct(((User) userMgr.getUser(username)).id, addedProduct);
            logMgr.addNewUserActivity(ua);
        }

        public void modifyProduct(string username, Product p) {
            int sizeUpperBound = 1000;
            if (p.title.Length < 3)
                throw new Exceptions.InvalidProductTitleException();
            if (p.width < 0 || p.width > sizeUpperBound)
                throw new Exceptions.InvalidProductSizeException();
            if (p.height < 0 || p.height > sizeUpperBound)
                throw new Exceptions.InvalidProductSizeException();
            if (p.depth < 0 || p.depth > sizeUpperBound)
                throw new Exceptions.InvalidProductSizeException();
            if (p.price < 0.0f)
                throw new Exceptions.InvalidProductPriceException();
            if (p.stock < 0)
                throw new Exceptions.InvalidProductStockException();
            IProductManager mgr = new MySqlProductManager();
            Product? u = mgr.getProduct(p.title);
            Product before;

            if (u == null) {
                before = (Product)mgr.getProduct(p.id);                
                mgr.modifyProduct(p);
            } else {
                before = (Product)u;
                if (before.id == p.id) 
                    mgr.modifyProduct(p);
                else
                    throw new Exceptions.InvalidProductTitleException();
            }

            UserActivity ua = new UserActivity();
            IUserManager userMgr = new MySqlUserManager();
            ua.modifyProduct(((User)userMgr.getUser(username)).id, before, p);
            IUserActivityManager userActivityMgr = new MySqlUserActivityManager();
            userActivityMgr.addNewUserActivity(ua);
        }

        public void deleteProduct(string username, int idProduct) {
            IProductManager productMgr = new MySqlProductManager();
            Product p = (Product)productMgr.getProduct(idProduct);
            productMgr.deleteProduct(idProduct);

            IUserManager userMgr = new MySqlUserManager();
            IUserActivityManager userActivityMgr = new MySqlUserActivityManager();
            UserActivity ua = new UserActivity();
            ua.deleteProduct(((User)userMgr.getUser(username)).id, p);
            userActivityMgr.addNewUserActivity(ua);
        }

    }
}
