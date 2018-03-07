using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend4;
using Frontend4.Hardware;
/*SENG301_Winter2017_Assignment4
 * Author: Alejandro Garcia (T06), Muzhou Zhai (T06)
 * Submission date: Apirl, 2nd, 2017
 */
namespace seng301_asgn4.src
{
    //This class will talk to the HardwareFacade and nothing else
    public class ProductFacade
    {
        public EventHandler<ProductEventArgs> productsAddedEvent;

        private HardwareFacade Hardware;
        public ProductFacade(HardwareFacade hardware)
        {
            this.Hardware = hardware;
            for (int i = 0; i < Hardware.ProductRacks.Count(); i++)
            {
                Hardware.ProductRacks[i].ProductAdded += new EventHandler<ProductEventArgs>(notifyProductsAdded);
            }
        }
        //===Products have been added, example event to communicate with listeners
        public void notifyProductsAdded(object sender, ProductEventArgs args)
        {
            this.productsAddedEvent(sender, args);
        }
        //===Get the cost of the product via index number
        public Cents getCost(int index)
        {
            return this.Hardware.ProductKinds[index].Cost;
        }
        //====Get the product's name via index number
        public string getName(int index)
        {
            return this.Hardware.ProductKinds[index].Name;
        }
        //====Dispense the product via index number
        public void vendProduct(int index)
        {
            this.Hardware.ProductRacks[index].DispenseProduct();
        }
        //===Maybe some extension would automatically load products? Anyways this handles that
        public void loadProducts(int[] productCounts)
        {
            this.Hardware.LoadProducts(productCounts);
        }
        public bool hasProduct(int index)
        {
            return this.Hardware.ProductRacks[index].Count != 0;
        }
        public int getIndex(Product p)
        {
            return this.Hardware.CoinRacks.ToList().FindIndex(b => b.Equals(p));//get the button index
        }


    }
}
