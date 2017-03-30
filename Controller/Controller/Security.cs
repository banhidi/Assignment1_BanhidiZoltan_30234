using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller {
    class Security : ISecurity {
        public string applyHashFunction(string text) {
            System.Security.Cryptography.HashAlgorithm hashFunction = 
                System.Security.Cryptography.SHA256.Create();
            byte[] originalData = Encoding.UTF8.GetBytes(text.ToCharArray());
            byte[] cryptedData = hashFunction.ComputeHash(originalData);
            StringBuilder s = new StringBuilder();
            foreach (byte b in cryptedData)
                s.Append(b);
            return s.ToString();
        }
    }
}
