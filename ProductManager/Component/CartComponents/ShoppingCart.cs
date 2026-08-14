namespace ProductManager.Component.CartComponents
{
	public class ShoppingCart
	{
		public readonly List<CartItem> Items = new List<CartItem>();

		public void AddItem(CartItem cartItem)
		{
			// Get Item that has same Id
			CartItem? existItem = Items.FirstOrDefault(i => i.ProductId == cartItem.ProductId);
			// Check if has item with same ID
			if (existItem != null)
			{
				// Sum the quanitity instead of add new item to cart
				existItem.Quantity += cartItem.Quantity;
			}
			else
				Items.Add(cartItem);
		}
		
		public int GetCartItemCount()
		{
			return Items.Count;
		}

		public void Clear() 
		{
			Items.Clear();	
		}
	}

	public class CartItem
	{
		public int ProductId {get; set;}
		public string Name {get; set;}
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal TotalPrice => Quantity * UnitPrice;
	}	
}
