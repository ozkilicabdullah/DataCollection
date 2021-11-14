using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Consumers.Service.PackageService
{
    public interface IPackageService
    {
        List<BasketParams> BasketList();
        List<SearchParams> SearchList();
        List<ReturnedParams> ReturnedList();
        List<ViewParams> ViewList();
        List<WishParams> WishList();
        List<SuccessFullCheckoutParams> PurchaseList();
        void ClearBasketList();
        void ClearSearchList();
        void ClearReturnedList();
        void ClearViewList();
        void ClearWishList();
        void ClearPurchaseList();
    }
    public class PackageService : IPackageService
    {
        public List<BasketParams> ListBasket = new List<BasketParams>();
        public List<SearchParams> ListSearch = new List<SearchParams>();
        public List<ReturnedParams> ListReturned = new List<ReturnedParams>();
        public List<ViewParams> ListView = new List<ViewParams>();
        public List<WishParams> ListWish = new List<WishParams>();
        public List<SuccessFullCheckoutParams> ListPurchase = new List<SuccessFullCheckoutParams>();

        public List<BasketParams> BasketList()
        {
            return ListBasket;
        }

        public List<SuccessFullCheckoutParams> PurchaseList()
        {
            return ListPurchase;
        }


        public List<ReturnedParams> ReturnedList()
        {
            return ListReturned;
        }

        public List<SearchParams> SearchList()
        {
            return ListSearch;
        }

        public List<ViewParams> ViewList()
        {
            return ListView;
        }

        public List<WishParams> WishList()
        {
            return ListWish;
        }
        public void ClearBasketList()
        {
            ListBasket.Clear();
        }
        public void ClearSearchList()
        {
            ListSearch.Clear();
        }

        public void ClearReturnedList()
        {
            ListReturned.Clear();
        }

        public void ClearViewList()
        {
            ListView.Clear();
        }

        public void ClearWishList()
        {
            ListWish.Clear();
        }

        public void ClearPurchaseList()
        {
            ListPurchase.Clear();
        }
    }
}
